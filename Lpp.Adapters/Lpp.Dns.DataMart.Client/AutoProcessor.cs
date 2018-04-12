using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Lib.Utils;
using Lpp.Dns.DataMart.Model;
using DMClient = Lpp.Dns.DataMart.Client;
using log4net;
using System.Reactive.Linq;
using System.Collections.Concurrent;

namespace Lpp.Dns.DataMart.Client
{
    public class AutoProcessor : IDisposable
    {
        readonly ILog Log = LogManager.GetLogger(typeof(AutoProcessor));
        
        readonly ConcurrentDictionary<string, ProcessingStatus> RequestStatuses;
        readonly object Lock = new object();
        NetWorkSetting _networkSetting;
        Timer _timer;
        int _queriesProcessedCount = 0;

        enum ProcessingStatus { InProcessing, Complete, CannotRunAndUpload }        

        public AutoProcessor(NetWorkSetting networkSetting)
        {
            _networkSetting = networkSetting;
            RequestStatuses = new ConcurrentDictionary<string, ProcessingStatus>();
            _timer = new Timer {
                Interval = _networkSetting.RefreshRate * 1000,
                AutoReset = false
            };
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();

            Log.Info("For network: " + networkSetting.NetworkName + ", Automated processing worker started. Refresh rate:" + networkSetting.RefreshRate + " seconds.");
        }

        

        public void UpdateNetworkSetting(NetWorkSetting networkSetting)
        {
            _timer.Stop();

            lock (Lock)
            {
                _networkSetting = networkSetting;
                Log.Debug("Network settings for autoprocessor updated, Network:" + _networkSetting.NetworkName);
            }

            _timer.Dispose();

            _timer = new Timer {
                Interval = _networkSetting.RefreshRate * 1000,
                AutoReset = false
            };

            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            Task.Run(async () => {
                try
                {
                    Log.Info("For network: " + _networkSetting.NetworkName + ", checking Request Queue for requests requiring automated processing.");

                    HubRequest[] requests = await GetRequestsAsync();

                    Log.Debug($"For network: { _networkSetting.NetworkName }, found { requests.Length } requests to process.");

                    HashSet<string> newRequestKeys = new HashSet<string>();

                    for(int i = 0; i < requests.Length; i++)
                    {
                        var key = MakeKey(requests[i]);

                        newRequestKeys.Add(key);

                        if (!RequestStatuses.ContainsKey(key))
                        {
                            AutoProcess(new KeyValuePair<string, HubRequest>(key, requests[i]));
                        }
                    }
                    
                    var toRemove = RequestStatuses.Where(kv => kv.Value == ProcessingStatus.Complete || (kv.Value == ProcessingStatus.CannotRunAndUpload && !newRequestKeys.Contains(kv.Key))).ToArray();
                    for(int i = 0; i < toRemove.Length; i++)
                    {
                        ProcessingStatus v;
                        RequestStatuses.TryRemove(toRemove[i].Key, out v);
                    }

                }
                catch(Exception ex)
                {
                    Log.Error("For network: " + _networkSetting.NetworkName + ", An error occurred during automated processing", ex);
                }
                finally
                {
                    _timer.Start();
                }
            });
        }

        private void AutoProcess(KeyValuePair<string, HubRequest> input)
        {

            var request = input.Value;

            var datamartDescription = Configuration.Instance.GetDataMartDescription(request.NetworkId, request.DataMartId);

            if (datamartDescription.ProcessQueriesAndUploadAutomatically == false)
            {
                //just notify, do not process
                string message = $"New query submitted and awaiting processing in { request.ProjectName } Project: { request.Source.Name } ({request.Source.Identifier})";
                SystemTray.DisplayNewQueryNotificationToolTip(message);
                RequestStatuses.TryAdd(input.Key, ProcessingStatus.CannotRunAndUpload);
                return;
            }


            var modelDescription = Configuration.Instance.GetModelDescription(request.NetworkId, request.DataMartId, request.Source.ModelID);
            
            var packageIdentifier = new Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier { Identifier = request.Source.RequestTypePackageIdentifier, Version = request.Source.AdapterPackageVersion };

            if (!System.IO.File.Exists(System.IO.Path.Combine(Configuration.PackagesFolderPath, packageIdentifier.PackageName())))
            {
                DnsServiceManager.DownloadPackage(_networkSetting, packageIdentifier);
            }

            var domainManager = new DomainManger.DomainManager(Configuration.PackagesFolderPath);

            try
            {
                domainManager.Load(request.Source.RequestTypePackageIdentifier, request.Source.AdapterPackageVersion);
                IModelProcessor processor = domainManager.GetProcessor(modelDescription.ProcessorId);
                ProcessorManager.UpdateProcessorSettings(modelDescription, processor);

                //need to initialize before checking the capabilities and settings of the processor since they may change based on the type of request being sent.
                if(processor is IEarlyInitializeModelProcessor)
                {
                    ((IEarlyInitializeModelProcessor)processor).Initialize(modelDescription.ModelId, request.Documents.Select(d => new DocumentWithStream(d.ID, new Document(d.ID, d.Document.MimeType, d.Document.Name, d.Document.IsViewable, Convert.ToInt32(d.Document.Size), d.Document.Kind), new DocumentChunkStream(d.ID, _networkSetting))).ToArray());
                }

                if (processor != null
                    && processor.ModelMetadata != null
                    && processor.ModelMetadata.Capabilities != null
                    && processor.ModelMetadata.Capabilities.ContainsKey("CanRunAndUpload")
                    && !(bool)processor.ModelMetadata.Capabilities["CanRunAndUpload"])
                {
                    //can't be run, don't attempt autoprocessing
                    RequestStatuses.TryAdd(input.Key, ProcessingStatus.CannotRunAndUpload);

                    domainManager.Dispose();
                    return;
                }

                request.Processor = processor;

                if (request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted)
                {
                    if (processor != null)
                    {
                        SystemTray.GenerateNotification(request, request.NetworkId);
                        StartProcessingRequest(request, processor, datamartDescription, domainManager);
                        return;
                    }
                }
                else if (request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval)
                {
                    if (datamartDescription.ProcessQueriesAndUploadAutomatically)
                    {
                        // Increment counter
                        System.Threading.Interlocked.Increment(ref _queriesProcessedCount);

                        SystemTray.UpdateNotifyText(_queriesProcessedCount, request.DataMartName, request.NetworkId);

                        StartUploadingRequest(request, domainManager);
                        return;
                    }
                }

                domainManager.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error($"Error autoprocessing Request: { request.Source.Identifier }, DataMartId: { request.DataMartId }, NetworkId: {request.NetworkId}", ex);

                domainManager.Dispose();
                throw;
            }
        }

        private void UploadRequest(HubRequest request)
        {
            Log.Info(string.Format("BackgroundProcess:  Processing query {0} (RequestId: {3}, DataMartId: {1}, NetworkId: {2})", request.Source.Identifier, request.DataMartId, request.NetworkId, request.Source.ID));

            string requestId = request.Source.ID.ToString();

            Lpp.Dns.DataMart.Model.Document[] responseDocuments = request.Processor.Response(requestId);
            if (responseDocuments != null && responseDocuments.Length > 0)
            {
                Guid[] documentIds = DnsServiceManager.PostResponseDocuments(requestId, request.DataMartId, responseDocuments, _networkSetting);
                Log.Info(string.Format("Number of portal document ids returned by PostResponseDocuments: {0} (Request Identifier: {1}, DataMart: {2}, Network: {3})", documentIds.Length, request.Source.Identifier, request.DataMartName, request.NetworkName));
                for (int i = 0; i < documentIds.Length; i++)
                {
                    Log.Info(string.Format("About to post content for portal document id: " + documentIds[i] + " corresponding to response document id: " + responseDocuments[i].DocumentID + " (Request Identifier: {0}, DataMart: {1}, Network: {2})", request.Source.Identifier, request.DataMartName, request.NetworkName));
                    System.IO.Stream contentStream = null;
                    try
                    {
                        request.Processor.ResponseDocument(requestId, responseDocuments[i].DocumentID, out contentStream, 60000);
                        DnsServiceManager.PostResponseDocumentContent(documentIds[i], contentStream, _networkSetting)
                            .LogExceptions(Log.Error)
                            .Catch()
                            .Wait();
                    }
                    finally
                    {
                        contentStream.CallDispose();
                    }
                }
            }
            else
            {
                Log.Info(string.Format("No documents to upload for query {0} (RequestId: {1}, DataMart: {2}, Network: {3})", request.Source.Identifier, request.Source.ID, request.DataMartName, request.NetworkName));
            }

            if (request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed)
            {
                HubRequestStatus hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(requestId));
                hubRequestStatus.Message = request.Processor.Status(requestId).Message;
                DnsServiceManager.SetRequestStatus(request, hubRequestStatus, request.Properties, _networkSetting);
                Log.Info(string.Format("The following query {0} (ID: {3}, DataMart: {4}, Network: {5}) failed to execute on behalf of {1}:\n\n{2}", request.Source.Identifier, _networkSetting.Profile.Username, hubRequestStatus.Message, request.Source.ID, request.DataMartName, request.NetworkName));
            }
            else
            {
                Log.Info(string.Format("The following query {0} (ID: {2}, DataMart: {3}, Network: {4}) was executed and results were uploaded automatically on behalf of {1}", request.Source.Identifier, _networkSetting.Profile.Username, request.Source.ID, request.DataMartName, request.NetworkName));
            }
        }

        private void RunTask(string key, DomainManger.DomainManager domainManager, Action task, Action<Task> continuation = null)
        {
            if (RequestStatuses.TryAdd(key, ProcessingStatus.InProcessing))
            {
                Task t = Task.Run(task);

                Task final = t;
                if (continuation != null)
                {
                    final = t.ContinueWith(continuation);
                }

                final.ContinueWith((finished) =>
                {
                    RequestStatuses[key] = ProcessingStatus.Complete;
                    domainManager.Dispose();
                });
            }
        }

        private void StartUploadingRequest(HubRequest request, DomainManger.DomainManager domainManager)
        {
            RunTask(MakeKey(request), domainManager, () => UploadRequest(request));
        }

        private void StartProcessingRequest(HubRequest request, IModelProcessor processor, DataMartDescription datamartDescription, DomainManger.DomainManager domainManager)
        {
            Action process = () =>
            {
                processor.SetRequestProperties(request.Source.ID.ToString(), request.Properties);
                ProcessRequest(request);
            };

            Action<Task> continuation = (completed) =>
            {
                HubRequestStatus hubRequestStatus = null;
                var statusCode = request.Processor.Status(request.Source.ID.ToString()).Code;

                if (datamartDescription.ProcessQueriesAndUploadAutomatically && (statusCode == RequestStatus.StatusCode.Complete || statusCode == RequestStatus.StatusCode.CompleteWithMessage))
                {
                    // Post process requests that are automatically uploaded
                    processor.PostProcess(request.Source.ID.ToString());

                    // Increment counter
                    System.Threading.Interlocked.Increment(ref _queriesProcessedCount);

                    statusCode = request.Processor.Status(request.Source.ID.ToString()).Code;

                    if(statusCode == RequestStatus.StatusCode.Error)
                    {
                        hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(request.Source.ID.ToString()));
                        hubRequestStatus.Message = request.Processor.Status(request.Source.ID.ToString()).Message;
                    }
                    else if(statusCode == RequestStatus.StatusCode.Complete || statusCode == RequestStatus.StatusCode.CompleteWithMessage)
                    {
                        SystemTray.UpdateNotifyText(_queriesProcessedCount, request.DataMartName, request.NetworkId);
                        try
                        {
                            UploadRequest(request);
                            statusCode = request.Processor.Status(request.Source.ID.ToString()).Code;
                            hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(request.Source.ID.ToString()));
                            hubRequestStatus.Message = request.Processor.Status(request.Source.ID.ToString()).Message;

                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("An error occurred while attempting unattended processing of the following query {0} (ID: {1}, DataMart: {3}, Network: {4})", request.Source.Identifier, request.Source.ID, request.DataMartName, request.NetworkName);
                            Log.Error(message, ex);
                            hubRequestStatus = new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed, message);
                        }
                    }
                    else
                    {
                        statusCode = request.Processor.Status(request.Source.ID.ToString()).Code;
                        hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(request.Source.ID.ToString()));
                        hubRequestStatus.Message = request.Processor.Status(request.Source.ID.ToString()).Message;
                    }

                    DnsServiceManager.SetRequestStatus(request, hubRequestStatus, request.Properties, _networkSetting);

                    Log.Info(string.Format("BackgroundProcess:  Finished Processing / Uploading results for query {0} (RequestID: {3}, DataMart: {1}, Network: {2})", request.Source.Identifier, request.DataMartName, request.NetworkName, request.Source.ID));


                }
            };

            RunTask(MakeKey(request), domainManager, process, continuation);
        }

        /// <summary>
        /// Execute query only and do not upload results
        /// </summary>
        /// <param name="qdm">Query to be processed</param>
        private void ProcessRequest(HubRequest request)
        {
            Log.Info(string.Format("BackgroundProcess:  Processing request {0} (RequestID: {3}, DataMartId: {1}, NetworkId: {2})", request.Source.Identifier, request.DataMartId, request.NetworkId, request.Source.ID));

            try
            {
                string requestId = ProcessRequest(request, request.Processor);

                RequestStatus.StatusCode statusCode = request.Processor.Status(requestId).Code;

                if (statusCode == RequestStatus.StatusCode.Error)
                {
                    HubRequestStatus hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(requestId));
                    hubRequestStatus.Message = request.Processor.Status(requestId).Message;
                    DnsServiceManager.SetRequestStatus(request, hubRequestStatus, request.Properties, _networkSetting);
                }

                Log.Info(String.Format("The following query {3} (ID: {0}) was executed on behalf of {1}:\n\n{2}", request.Source.ID, _networkSetting.Profile.Username, "", request.Source.Identifier));

            }
            catch (Exception ex)
            {
                string message = string.Format("BackgroundProcess: An error occurred when processing request {0} (RequestID: {3}, DataMartId: {1}, NetworkId: {2}", request.Source.Identifier, request.DataMartId, request.NetworkId, request.Source.ID);
                Log.Error(message, ex);
                DnsServiceManager.SetRequestStatus(request, new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed, message), request.Properties, _networkSetting);
            }

            Log.Info(string.Format("BackgroundProcess:  Finished processing request {0} (RequestID: {3}, DataMartId: {1}, NetworkId: {2})", request.Source.Identifier, request.DataMartId, request.NetworkId, request.Source.ID));
        }

        private string ProcessRequest(HubRequest request, IModelProcessor processor)
        {
            try
            {
                Document[] requestDocuments = request.Documents.Select(d => new Lpp.Dns.DataMart.Model.Document(d.ID.ToString("D"), d.Document.MimeType, d.Document.Name) { IsViewable = d.Document.IsViewable, Size = Convert.ToInt32(d.Document.Size), Kind = d.Document.Kind }).ToArray();
                Document[] desiredDocuments;
                string requestId = request.Source.ID.ToString();
                IDictionary<string, string> requestProperties;
                processor.Request(requestId, _networkSetting.CreateInterfaceMetadata(), request.CreateInterfaceMetadata(), requestDocuments, out requestProperties, out desiredDocuments);

                Log.Info("Request posted: " + request.Source.Identifier + " (ID: " + requestId + ")");
                Log.Info("Number of documents available: " + requestDocuments.Length);
                Log.Info("Number of documents desired: " + desiredDocuments.Length);
                if (requestProperties != null && requestProperties.Count > 0)
                {
                    Log.Info("Properties: ");
                    foreach (string key in requestProperties.Keys)
                        Log.Info("Key: " + key + "=" + requestProperties[key]);
                }

                foreach (Lpp.Dns.DataMart.Model.Document requestDocument in desiredDocuments)
                {
                    Log.Info("About to download desired document id: " + requestDocument.DocumentID);
                    DocumentChunkStream requestDocumentStream = new DocumentChunkStream(Guid.Parse(requestDocument.DocumentID), _networkSetting);
                    processor.RequestDocument(requestId, requestDocument.DocumentID, requestDocumentStream);
                    Log.Info("Downloaded desired document id: " + requestDocument.DocumentID);
                }

                Log.Info("Starting request with local request: " + request.Source.Identifier + " (ID: " + requestId + ")");
                processor.Start(requestId);
                Log.Info("Start finished on request with local request: " + request.Source.Identifier + " (ID: " + requestId + ")");

                return requestId;
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected exception in Util.ProcessRequest.", ex);
                throw;
            }
        }


        /// <summary>
        /// Create a unique key for the Request/Data Mart combination. Uses the Guid for the request
        /// combined with the Guid for the Data Mart.
        /// </summary>
        /// <param name="req">The request to generate a key for</param>
        /// <returns>A string formed from the Request ID followed by the Data Mart ID</returns>
        static string MakeKey(HubRequest req)
        {
            return req.Source.ID.ToString("D") + req.DataMartId.ToString("D");
        }

        async Task<HubRequest[]> GetRequestsAsync()
        {
            if(_networkSetting.NetworkStatus != Util.ConnectionOKStatus)
            {
                return new HubRequest[0];
            }

            var datamartIDs = _networkSetting.DataMartList
                                // BMS: Note the ProcessAndNotUpload feature has been temporarilty disabled until we fix the processing around this feature
                                .Where(dm => dm.AllowUnattendedOperation && (dm.NotifyOfNewQueries || /* dm.ProcessQueriesAndNotUpload || */ dm.ProcessQueriesAndUploadAutomatically))
                                .Select(dm => dm.DataMartId).ToArray();

            if(datamartIDs.Length == 0)
            {
                return new HubRequest[0];
            }

            var requestFilter = new RequestFilter
            {
                Statuses = new[] { DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted, DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted },
                DataMartIds = datamartIDs
            };

            var requests = from list in DnsServiceManager.GetRequestList(_networkSetting, 0, Properties.Settings.Default.AutoProcessingBatchSize, requestFilter, null, null)
                           from rl in list.Segment.EmptyIfNull().ToObservable()
                           where rl.AllowUnattendedProcessing
                           from r in RequestCache.ForNetwork(_networkSetting).LoadRequest(rl.ID, rl.DataMartID)
                           select r;

            return await requests.ToArray();
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }
    }
}