﻿using log4net;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Lib.Utils;
using Lpp.Dns.DataMart.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client
{
    public class AutoProcessor : IDisposable
    {
        readonly ILog Log = LogManager.GetLogger(typeof(AutoProcessor));
        
        readonly ConcurrentDictionary<string, ProcessingStatus> RequestStatuses;
        readonly object Lock = new object();
        NetWorkSetting _networkSetting;
        int _queriesProcessedCount = 0;

        TimeSpan _refreshRate = TimeSpan.FromMinutes(5);
        bool _enableProcessing = false;
        readonly int _processingBatchSize;

        enum ProcessingStatus { InProcessing, Complete, CannotRunAndUpload, PendingUpload }        

        public AutoProcessor(NetWorkSetting networkSetting)
        {
            _networkSetting = networkSetting;
            RequestStatuses = new ConcurrentDictionary<string, ProcessingStatus>();

            _processingBatchSize = Properties.Settings.Default.AutoProcessingBatchSize;
            if (_processingBatchSize < 1)
            {
                _processingBatchSize = 10;
            }


            _refreshRate = TimeSpan.FromSeconds(_networkSetting.RefreshRate);
            Log.Info("For network: " + networkSetting.NetworkName + ", Automated processing worker started. Refresh rate:" + networkSetting.RefreshRate + " seconds.");
            _enableProcessing = true;

            StartAutoProcessing();
        }

        

        public void UpdateNetworkSetting(NetWorkSetting networkSetting)
        {
            Log.Info("For network: " + networkSetting.NetworkName + ", Automated processing worker stopped due to network setting being refreshed. Refresh rate:" + networkSetting.RefreshRate + " seconds.");

            lock (Lock)
            {
                _networkSetting = networkSetting;
                Log.Debug("Network settings for autoprocessor updated, Network:" + _networkSetting.NetworkName);

                _refreshRate = TimeSpan.FromSeconds(_networkSetting.RefreshRate);
            }

            StartAutoProcessing();
        }

        public void Stop()
        {
            Log.Info("Stop called to AutoProcessor.");
            _enableProcessing = false;
        }

        void StartAutoProcessing()
        {
            Task.Run(async () => {

                while (_enableProcessing)
                {
                    try
                    {
                        Log.Info("For network: " + _networkSetting.NetworkName + ", checking Request Queue for requests requiring automated processing.");

                        HubRequest[] requests = await GetRequestsAsync();

                        Dictionary<string, HubRequest> newRequests = requests.ToDictionary(k => MakeKey(k));

                        Log.Debug($"For network: { _networkSetting.NetworkName }, found { newRequests.Where(k => RequestStatuses.ContainsKey(k.Key) == false).Count() }/{ newRequests.Count } new requests that require automated processing.");


                        foreach (var pair in newRequests)
                        {
                            if (!RequestStatuses.ContainsKey(pair.Key))
                            {
                                AutoProcess(pair);
                            }
                            else
                            {
                                Log.Debug($"For network:{ _networkSetting.NetworkName }, the AutoProcessor status tracking collection already has an entry for request:{ pair.Value.Source.MSRequestID }/{ pair.Value.DataMartName } (status: { pair.Value.RoutingStatus.ToString() }) with processing status of { RequestStatuses[pair.Key] }. Skipping auto-processing.");
                            }
                        }

                        var toRemove = RequestStatuses.Where(kv => kv.Value == ProcessingStatus.Complete || (kv.Value == ProcessingStatus.CannotRunAndUpload && !newRequests.ContainsKey(kv.Key))).ToArray();
                        Log.Debug($"For network: {_networkSetting.NetworkName }, removing { toRemove.Length } requests from the AutoProcessor status tracking collection.");
                        for (int i = 0; i < toRemove.Length; i++)
                        {
                            ProcessingStatus v;
                            if (RequestStatuses.TryRemove(toRemove[i].Key, out v))
                            {
                                Log.Debug($"For network: { _networkSetting.NetworkName }, removed AutoProcessing status tracking collection item with the key:{ toRemove[i].Key }.");
                            }
                        }

                        toRemove = null;
                        newRequests = null;
                        requests = null;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("For network: " + _networkSetting.NetworkName + ", An error occurred during automated processing", ex);
                    }
                    finally
                    {
                        Log.Debug($"For network:{_networkSetting.NetworkName}, AutoProcessor starting to sleep for { _refreshRate.TotalSeconds } seconds.");
                        System.Threading.Thread.Sleep(_refreshRate);
                    }
                }

            });
        }

        private void AutoProcess(KeyValuePair<string, HubRequest> input)
        {

            var request = input.Value;

            var datamartDescription = Configuration.Instance.GetDataMartDescription(request.NetworkId, request.DataMartId);

            if (datamartDescription.ProcessQueriesAndUploadAutomatically == false && datamartDescription.ProcessQueriesAndNotUpload == false)
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
                processor.Settings.Add("NetworkId", request.NetworkId);

                Lib.Caching.DocumentCacheManager cache = new Lib.Caching.DocumentCacheManager(request.NetworkId, request.DataMartId, request.Source.ID, request.Source.Responses.Where(x => x.DataMartID == request.DataMartId).FirstOrDefault().ResponseID);

                //need to initialize before checking the capabilities and settings of the processor since they may change based on the type of request being sent.
                if (processor is IEarlyInitializeModelProcessor)
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

                if (cache.HasResponseDocuments == false && (request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted))
                {
                    if (processor != null)
                    {
                        SystemTray.GenerateNotification(request, request.NetworkId);
                        StartProcessingRequest(request, processor, datamartDescription, domainManager, cache);
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

                        StartUploadingRequest(request, domainManager, cache);
                        return;
                    }
                }
                else if (cache.HasResponseDocuments)
                {
                    RequestStatuses.TryAdd(input.Key, ProcessingStatus.PendingUpload);
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

        private void UploadRequest(HubRequest request, Lib.Caching.DocumentCacheManager cache)
        {
            

            string requestId = request.Source.ID.ToString();

            Document[] responseDocuments;
            if (cache.Enabled)
            {
                responseDocuments = cache.GetResponseDocuments().ToArray();
            }
            else
            {
                responseDocuments = request.Processor.Response(requestId);
            }

            if (responseDocuments != null && responseDocuments.Length > 0)
            {
                Log.Info(string.Format("Uploading {4} documents for query: {0} (DataMart:{1}, Network:{2}, RequestID: {3:D})", request.Source.Identifier, request.DataMartName, request.NetworkName, request.Source.ID, responseDocuments.Length));

                if (_networkSetting.SupportsUploadV2)
                {
                    Log.Debug($"Network settings indicate document upload v2 supported for query: {request.Source.Identifier}, (DataMart: {request.DataMartName}, Network: { request.NetworkName }, RequestID: { request.Source.ID }");
                    try
                    {
                        Parallel.ForEach(responseDocuments,
                                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                                (doc) =>
                                {
                                    string uploadIdentifier = ("[" + Utilities.Crypto.Hash(Guid.NewGuid()) + "]").PadRight(16);

                                    System.IO.Stream stream = null;
                                    try
                                    {
                                        if (cache.Enabled)
                                        {
                                            stream = cache.GetDocumentStream(Guid.Parse(doc.DocumentID));
                                        }
                                        else
                                        {
                                            request.Processor.ResponseDocument(requestId, doc.DocumentID, out stream, doc.Size);
                                        }

                                        var dto = new DTO.DataMartClient.Criteria.DocumentMetadata
                                        {
                                            ID = Guid.Parse(doc.DocumentID),
                                            DataMartID = request.DataMartId,
                                            RequestID = Guid.Parse(requestId),
                                            IsViewable = doc.IsViewable,
                                            Size = doc.Size,
                                            MimeType = doc.MimeType,
                                            Kind = doc.Kind,
                                            Name = doc.Filename,
                                            CurrentChunkIndex = 0
                                        };

                                        DnsServiceManager.PostDocumentChunk(uploadIdentifier, dto, stream, _networkSetting);
                                    }
                                    finally
                                    {
                                        stream.Dispose();
                                    }

                                });
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"An error occurred during concurrent uploading of response documents for query: {request.Source.Identifier}, (DataMart: {request.DataMartName}, Network: { request.NetworkName }, RequestID: { request.Source.ID }", ex);
                        throw;
                    }
                }
                else
                {
                    //since the ID of the document returned from the API is not known when posting, and the order cannot be assumed in the response
                    //loop over each document and post the metadata and the content as a single unit of work per document.
                    Log.Debug($"Network settings indicates document upload v2 IS NOT supported for query: {request.Source.Identifier}, (DataMart: {request.DataMartName}, Network: { request.NetworkName }, RequestID: { request.Source.ID }");
                    for (int i = 0; i < responseDocuments.Length; i++)
                    {
                        string uploadIdentifier = ("[" + Utilities.Crypto.Hash(Guid.NewGuid()) + "]").PadRight(16);
                        Guid[] metaDataPostResponse = DnsServiceManager.PostResponseDocuments(uploadIdentifier, requestId, request.DataMartId, new[] { responseDocuments[i] }, _networkSetting);

                        System.IO.Stream contentStream = null;
                        try
                        {
                            if (cache.Enabled)
                            {
                                contentStream = cache.GetDocumentStream(Guid.Parse(responseDocuments[i].DocumentID));
                            }
                            else
                            {
                                request.Processor.ResponseDocument(requestId, responseDocuments[i].DocumentID, out contentStream, 60000);
                            }

                            DnsServiceManager.PostResponseDocumentContent(uploadIdentifier, requestId, request.DataMartId, metaDataPostResponse[0], responseDocuments[i].Filename, contentStream, _networkSetting)
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
            }
            else
            {
                Log.Info(string.Format("No documents to upload for query: {0} (DataMart:{1}, Network:{2}, RequestID: {3:D})", request.Source.Identifier, request.DataMartName, request.NetworkName, request.Source.ID, (responseDocuments != null ? responseDocuments.Length : 0)));
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

        private void StartUploadingRequest(HubRequest request, DomainManger.DomainManager domainManager, Lib.Caching.DocumentCacheManager cache)
        {
            RunTask(MakeKey(request), domainManager, () => UploadRequest(request, cache));
        }

        private void StartProcessingRequest(HubRequest request, IModelProcessor processor, DataMartDescription datamartDescription, DomainManger.DomainManager domainManager, Lib.Caching.DocumentCacheManager cache)
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

                if (cache.Enabled)
                {
                    Document[] responseDocuments = processor.Response(request.Source.ID.ToString());
                    cache.Add(responseDocuments.Select(doc => {
                        System.IO.Stream data;
                        processor.ResponseDocument(request.Source.ID.ToString(), doc.DocumentID, out data, doc.Size);

                        Guid documentID;
                        if (!Guid.TryParse(doc.DocumentID, out documentID))
                        {
                            documentID = Utilities.DatabaseEx.NewGuid();
                            doc.DocumentID = documentID.ToString();
                        }

                        return new DocumentWithStream(documentID, doc, data);
                    }));
                }

                if(datamartDescription.ProcessQueriesAndNotUpload && (statusCode == RequestStatus.StatusCode.Complete || statusCode == RequestStatus.StatusCode.CompleteWithMessage))
                {
                    RequestStatuses.TryAdd(MakeKey(request), ProcessingStatus.PendingUpload);
                }
                else if (datamartDescription.ProcessQueriesAndUploadAutomatically && (statusCode == RequestStatus.StatusCode.Complete || statusCode == RequestStatus.StatusCode.CompleteWithMessage))
                {
                    // Post process requests that are automatically uploaded
                    processor.PostProcess(request.Source.ID.ToString());

                    if (cache.Enabled)
                    {
                        cache.ClearCache();
                        Document[] responseDocuments = processor.Response(request.Source.ID.ToString());
                        cache.Add(responseDocuments.Select(doc => {
                            System.IO.Stream data;
                            processor.ResponseDocument(request.Source.ID.ToString(), doc.DocumentID, out data, doc.Size);

                            Guid documentID;
                            if (!Guid.TryParse(doc.DocumentID, out documentID))
                            {
                                documentID = Utilities.DatabaseEx.NewGuid();
                                doc.DocumentID = documentID.ToString();
                            }

                            return new DocumentWithStream(documentID, doc, data);
                        }));
                    }

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
                            UploadRequest(request, cache);
                            statusCode = request.Processor.Status(request.Source.ID.ToString()).Code;
                            hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(request.Source.ID.ToString()));
                            hubRequestStatus.Message = request.Processor.Status(request.Source.ID.ToString()).Message;
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("An error occurred while attempting unattended processing of the following query {0} (ID: {1}, DataMart: {2}, Network: {3})", request.Source.Identifier, request.Source.ID, request.DataMartName, request.NetworkName);
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

                    if(statusCode == RequestStatus.StatusCode.Error || statusCode == RequestStatus.StatusCode.Complete || statusCode == RequestStatus.StatusCode.CompleteWithMessage || statusCode == RequestStatus.StatusCode.AwaitingResponseApproval)
                    {
                        //mark auto-processing complete in the status cache if the processing resulted in either an error or a completed status
                        var key = MakeKey(request);
                        if (RequestStatuses.ContainsKey(key))
                        {
                            RequestStatuses[key] = ProcessingStatus.Complete;
                        }
                    }
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
                var key = MakeKey(request);
                if (RequestStatuses.ContainsKey(key))
                {
                    RequestStatuses[key] = ProcessingStatus.InProcessing;
                }

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
                    Log.Debug("Downloading document" + requestDocument.Filename + $" for Request: {request.Source.MSRequestID}, DataMart: { request.DataMartName }");
                    DocumentChunkStream requestDocumentStream = new DocumentChunkStream(Guid.Parse(requestDocument.DocumentID), _networkSetting);
                    processor.RequestDocument(requestId, requestDocument.DocumentID, requestDocumentStream);
                    Log.Debug("Successfully Downloaded document" + requestDocument.Filename + $" for Request: {request.Source.MSRequestID}, DataMart: { request.DataMartName }");
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
                                .Where(dm => dm.AllowUnattendedOperation && (dm.NotifyOfNewQueries || dm.ProcessQueriesAndNotUpload || dm.ProcessQueriesAndUploadAutomatically))
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

            var requests = Observable.Create<DTO.DataMartClient.RequestList>(async observer =>
            {
                int index = 0;
                DTO.DataMartClient.RequestList rl = null;

                while (rl == null || (index < rl.TotalCount))
                {
                    rl = await DnsServiceManager.GetRequestList("AutoProcessor", _networkSetting, index, _processingBatchSize, requestFilter, DTO.DataMartClient.RequestSortColumn.RequestTime, true);

                    if (rl == null || rl.TotalCount == 0)
                    {
                        break;
                    }

                    observer.OnNext(rl);

                    index += _processingBatchSize;
                }

                observer.OnCompleted();
            }).DefaultIfEmpty()
            .Aggregate((requestList1, requestList2) =>
            {
                if (requestList1 == null && requestList2 == null)
                {
                    return new DTO.DataMartClient.RequestList
                    {
                        Segment = Array.Empty<DTO.DataMartClient.RequestListRow>(),
                        SortedAscending = true,
                        SortedByColumn = DTO.DataMartClient.RequestSortColumn.RequestTime
                    };
                }
                else if (requestList1 != null && requestList2 == null)
                {
                    return requestList1;
                }
                else if (requestList1 == null && requestList2 != null)
                {
                    return requestList2;
                }
                else
                {
                    return new DTO.DataMartClient.RequestList
                    {
                        Segment = requestList1.Segment.EmptyIfNull().Concat(requestList2.Segment.EmptyIfNull()).ToArray(),
                        SortedAscending = requestList1.SortedAscending,
                        SortedByColumn = requestList1.SortedByColumn,
                        StartIndex = requestList1.StartIndex,
                        TotalCount = requestList1.TotalCount
                    };
                }
            })
           .SelectMany(requestList =>
           {
               if (requestList == null)
               {
                   return Array.Empty<DTO.DataMartClient.RequestListRow>();
               }

               return requestList.Segment.Where(s => s != null && s.AllowUnattendedProcessing).DefaultIfEmpty();
           })
           .SelectMany(rlr =>
           //skipping using the request cache to always use the most recent request dto.
            rlr == null ? Observable.Empty<HubRequest>() : LoadRequestImpl(rlr.ID, rlr.DataMartID)
           ).ToArray();

            return await requests;
        }

        IObservable<HubRequest> LoadRequestImpl(Guid id, Guid dataMartId)
        {
            var dm = _networkSetting.DataMartList.FirstOrDefault(d => d.DataMartId == dataMartId);
            return from r in DnsServiceManager.GetRequests(_networkSetting, new[] { id }, dataMartId).SkipWhile(r => r.ID != id).Take(1)

                   let rt = r.Routings.EmptyIfNull().FirstOrDefault(x => x.DataMartID == dataMartId)
                   where rt != null

                   select new HubRequest
                   {
                       Source = r,
                       DataMartId = dataMartId,
                       NetworkId = _networkSetting.NetworkId,
                       NetworkName = _networkSetting.NetworkName,
                       DataMartName = dm == null ? null : dm.DataMartName,
                       DataMartOrgId = dm == null ? null : dm.OrganizationId,
                       DataMartOrgName = dm == null ? null : dm.OrganizationName,
                       ProjectName = r != null && r.Project != null ? r.Project.Name : null,

                       Properties = rt.Properties.EmptyIfNull().GroupBy(p => p.Name).ToDictionary(pp => pp.Key, pp => pp.First().Value),
                       Rights = r.Routings.Where(rn => rn.DataMartID == dataMartId).Select(rn => (HubRequestRights)rn.Rights).FirstOrDefault(),
                       RoutingStatus = rt.Status,
                       SubmittedDataMarts = string.Join(", ", from rtng in r.Routings
                                                              join dmt in _networkSetting.DataMartList on rtng.DataMartID equals dmt.DataMartId
                                                              select dmt.DataMartName),
                       Documents = r.Documents.EmptyIfNull().ToArray()
                   };
        }

        public void Dispose()
        {
            if (_enableProcessing)
            {
                _enableProcessing = false;
            }
        }
    }
}