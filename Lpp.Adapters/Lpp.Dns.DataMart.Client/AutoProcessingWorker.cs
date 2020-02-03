using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using log4net;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Lib.Utils;
using Lpp.Dns.DataMart.Model;
using DMClient = Lpp.Dns.DataMart.Client;

namespace Lpp.Dns.DataMart.Client
{
    public class AutoProcessingWorker : BackgroundWorker
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static AutoProcessingWorker _worker;
        private static object _lock = new object();
        private int _queriesProcessedCount = 0;

        public static AutoProcessingWorker Instance
        {
            get
            {
                lock ( _lock )
                {
                    if ( _worker != null ) return _worker;
                    _worker = new AutoProcessingWorker();
                    _worker.RunWorkerAsync();
                    return _worker;
                }
            }
        }

        private AutoProcessingWorker() : base() {}
                
        protected override void OnDoWork( DoWorkEventArgs e )
        {
            _log.Info("Automated processing worker started.");
            while (true)
            {
                _log.Info("Checking Master Request Queue for requests requiring automated processing.");
                SystemTray.UpdateNotificationIcon(IconType.IconBusy, "Processing Requests");

               var reqs = from ns in Configuration.Instance.NetworkSettingCollection.NetWorkSettings.Cast<NetWorkSetting>().ToObservable()
                           where ns.NetworkStatus == Util.ConnectionOKStatus

                           let dmIds = ns.DataMartList
                                        // BMS: Note the ProcessAndNotUpload feature has been temporarilty disabled until we fix the processing around this feature
                                        .Where(dm => dm.AllowUnattendedOperation && (dm.NotifyOfNewQueries || /* dm.ProcessQueriesAndNotUpload || */ dm.ProcessQueriesAndUploadAutomatically))
                                        .Select(dm => dm.DataMartId)
                                        .ToArray()
                           where dmIds.Any()

                           from list in DnsServiceManager.GetRequestList(ns, 0, Properties.Settings.Default.AutoProcessingBatchSize,
                                new RequestFilter { Statuses = new []{ Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted, DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted }, DataMartIds = dmIds }, null, null)

                           from rl in list.Segment.EmptyIfNull().ToObservable()
                           where rl.AllowUnattendedProcessing
                           from r in RequestCache.ForNetwork(ns).LoadRequest(rl.ID, rl.DataMartID)
                           select new { Request = r, NetworkSetting = ns };

#pragma warning disable CS0618 // Type or member is obsolete
                reqs
                    .Do(r =>
                    {
                        var request = r.Request;
                        var datamartDescription = Configuration.Instance.GetDataMartDescription(request.NetworkId, request.DataMartId);
                        var modelDescription = Configuration.Instance.GetModelDescription(request.NetworkId, request.DataMartId, request.Source.ModelID);
                        var networkSetting = r.NetworkSetting;

                        var packageIdentifier = new Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier { Identifier = request.Source.RequestTypePackageIdentifier, Version = request.Source.AdapterPackageVersion };
                        if (!System.IO.File.Exists(System.IO.Path.Combine(Configuration.PackagesFolderPath, packageIdentifier.PackageName())))
                        {
                            DnsServiceManager.DownloadPackage(r.NetworkSetting, packageIdentifier);
                        }

                        using (var domainManager = new DomainManger.DomainManager(Configuration.PackagesFolderPath))
                        {
                            domainManager.Load(request.Source.RequestTypePackageIdentifier, request.Source.AdapterPackageVersion);
                            IModelProcessor processor = domainManager.GetProcessor(modelDescription.ProcessorId);
                            ProcessorManager.UpdateProcessorSettings(modelDescription, processor);

                            if (processor is IEarlyInitializeModelProcessor)
                            {
                                ((IEarlyInitializeModelProcessor)processor).Initialize(modelDescription.ModelId, request.Documents.Select(d => new DocumentWithStream(d.ID, new Document(d.ID, d.Document.MimeType, d.Document.Name, d.Document.IsViewable, Convert.ToInt32(d.Document.Size), d.Document.Kind), new DocumentChunkStream(d.ID, r.NetworkSetting))).ToArray());
                            }

                            if (processor != null
                                && processor.ModelMetadata != null
                                && processor.ModelMetadata.Capabilities != null
                                && processor.ModelMetadata.Capabilities.ContainsKey("CanRunAndUpload")
                                && !(bool)processor.ModelMetadata.Capabilities["CanRunAndUpload"])
                            {
                                //can't be run, don't attempt autoprocessing
                                return;
                            }


                            request.Processor = processor;

                            if (request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted)
                            {
                                if (processor != null)
                                {
                                    SystemTray.generate_notification(request, request.NetworkId);

                                    if (datamartDescription.NotifyOfNewQueries)
                                    {
                                        SystemTray.UpdateNotificationIcon(IconType.IconBusy, string.Format("Query Submitted by {0}", request.Source.Author.Username));
                                    }
                                    else
                                    {
                                        processor.SetRequestProperties(request.Source.ID.ToString(), request.Properties);
                                        
                                        _log.Info(String.Format("BackgroundProcess:  Processing request {0} (RequestID: {3}, DataMartId: {1}, NetworkId: {2})", request.Source.Identifier, request.DataMartId, networkSetting.NetworkId, request.Source.ID));

                                        RequestStatus.StatusCode statusCode;
                                        HubRequestStatus hubRequestStatus;

                                        try
                                        {
                                            string requestId = ProcessRequest(request, request.Processor, networkSetting);

                                            _log.Info(String.Format("The following query {3} (ID: {0}) was executed on behalf of {1}:\n\n{2}", request.Source.ID, networkSetting.Profile.Username, "", request.Source.Identifier));

                                            statusCode = request.Processor.Status(requestId).Code;
                                            hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(requestId));

                                            if (statusCode == RequestStatus.StatusCode.Error)
                                            {   
                                                hubRequestStatus.Message = request.Processor.Status(requestId).Message;
                                            }

                                            if (datamartDescription.ProcessQueriesAndUploadAutomatically && (statusCode == RequestStatus.StatusCode.Complete || statusCode == RequestStatus.StatusCode.CompleteWithMessage))
                                            {
                                                // Post process requests that are automatically uploaded
                                                processor.PostProcess(request.Source.ID.ToString());

                                                statusCode = request.Processor.Status(requestId).Code;
                                                hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(requestId));

                                                if (statusCode == RequestStatus.StatusCode.Error)
                                                {
                                                    hubRequestStatus.Message = request.Processor.Status(requestId).Message;

                                                }
                                                else if (statusCode == RequestStatus.StatusCode.Complete || statusCode == RequestStatus.StatusCode.CompleteWithMessage)
                                                {
                                                    UploadRequest(request, networkSetting);
                                                }

                                            }

                                            statusCode = request.Processor.Status(request.Source.ID.ToString()).Code;
                                            hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(requestId));

                                            // Increment counter
                                            _queriesProcessedCount++;
                                            SystemTray.update_notify_text(_queriesProcessedCount, request.DataMartName, request.NetworkId);

                                        }
                                        catch (Exception ex)
                                        {
                                            string message = string.Format("BackgroundProcess: An error occurred when processing request {0} (RequestID: {3}, DataMartId: {1}, NetworkId: {2}", request.Source.Identifier, request.DataMartId, networkSetting.NetworkId, request.Source.ID);
                                            _log.Error(message, ex);

                                            hubRequestStatus = new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed, message);
                                        }

                                        //update the routing status
                                        DnsServiceManager.SetRequestStatus(request, hubRequestStatus, request.Properties, networkSetting);

                                        _log.Info(String.Format("BackgroundProcess:  Finished processing request {0} (RequestID: {3}, DataMartId: {1}, NetworkId: {2})", request.Source.Identifier, request.DataMartId, networkSetting.NetworkId, request.Source.ID));


                                    }
                                }
                            }
                            else if (request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval)
                            {
                                if (datamartDescription.ProcessQueriesAndUploadAutomatically)
                                {
                                    // Increment counter
                                    _queriesProcessedCount++;
                                    SystemTray.update_notify_text(_queriesProcessedCount, request.DataMartName, request.NetworkId);
                                    UploadRequest(request, networkSetting);
                                }
                            }
                        }

                    })
                    .LogExceptions(_log.Error)
                    .Catch()
                    .LastOrDefault();
#pragma warning restore CS0618 // Type or member is obsolete

                SystemTray.UpdateNotificationIcon(IconType.IconDefault, null);
                Thread.Sleep(DMClient.Properties.Settings.Default.RefreshRate);
            }
        }

        private void UploadRequest(HubRequest request, NetWorkSetting networkSetting)
        {
            try
            {
                string requestId = request.Source.ID.ToString();

                Lpp.Dns.DataMart.Model.Document[] responseDocuments = request.Processor.Response(requestId);
                Guid[] documentIds = DnsServiceManager.PostResponseDocuments(requestId, request.DataMartId, responseDocuments, networkSetting);

                _log.Info("Number of portal document ids returned by PostResponseDocuments: " + documentIds.Length);

                for (int i = 0; i < documentIds.Length; i++)
                {
                    _log.Info("About to post content for portal document id: " + documentIds[i] + " corresponding to response document id: " + responseDocuments[i].DocumentID);

                    Stream contentStream = null;
                    try
                    {
                        request.Processor.ResponseDocument(requestId, responseDocuments[i].DocumentID, out contentStream, 60000);

                        DnsServiceManager.PostResponseDocumentContent(documentIds[i], contentStream, networkSetting)
                            .LogExceptions(_log.Error)
                            .Catch()
                            .SubscribeOn(Scheduler.Default)
                            .LastOrDefault();
                    }
                    finally
                    {
                        contentStream.CallDispose();
                    }

                }

                if (request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed)
                {
                    HubRequestStatus hubRequestStatus = DnsServiceManager.ConvertModelRequestStatus(request.Processor.Status(requestId));
                    hubRequestStatus.Message = request.Processor.Status(requestId).Message;

                    DnsServiceManager.SetRequestStatus(request, hubRequestStatus, request.Properties, networkSetting);

                    _log.Info(String.Format("The following query {0} (ID: {3}) failed to execute on behalf of {1}:\n\n{2}", request.Source.Identifier, networkSetting.Profile.Username, hubRequestStatus.Message, request.Source.ID));
                }
                else
                {
                    //MantisID:6331:Zero out low-cell counts if they exist.
                    //string strReason = request.Processor.Status(requestId).Code == RequestStatus.StatusCode.CompleteWithMessage ? request.Processor.Status(requestId).Message : string.Empty;
                    //DnsServiceManager.SetRequestStatus(request, new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed), request.Properties, networkSetting);
                    //if (string.IsNullOrEmpty(strReason))
                    //{
                        //DnsServiceManager.SetRequestStatus(request, new HubRequestStatus(HubRequestStatus.StatusCode.Completed), request.Properties, networkSetting);
                        _log.Info(String.Format("The following query {0} (ID: {2}) was executed and results were uploaded automatically on behalf of {1}", request.Source.Identifier, networkSetting.Profile.Username, request.Source.ID));
                    //}
                    //else
                    //{
                    //    //DnsServiceManager.SetRequestStatus(request, new HubRequestStatus(HubRequestStatus.StatusCode.Completed, strReason), request.Properties, networkSetting);
                    //    _log.Info(String.Format("The following query {0} (ID: {3}) was executed and results were uploaded automatically on behalf of {1}:\n\n{2}",
                    //        request.Source.Identifier, networkSetting.Profile.Username, strReason, request.Source.ID));
                    //}
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("An error occurred while attempting unattended processing of the following query {0} (ID: {1})", request.Source.Identifier, request.Source.ID);
                _log.Error(message, ex);
                DnsServiceManager.SetRequestStatus(request, new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed, message), request.Properties, networkSetting);
            }

            _log.Info(String.Format("BackgroundProcess:  Finished Processing / Uploading results for query {0} (RequestID: {3}, DataMartId: {1}, NetworkId: {2})", request.Source.Identifier, request.DataMartId, networkSetting.NetworkId, request.Source.ID));
        }

        string ProcessRequest(HubRequest request, IModelProcessor processor, NetWorkSetting networkSetting)
        {
            try
            {
                Document[] requestDocuments = request.Documents.Select(d => new Lpp.Dns.DataMart.Model.Document(d.ID.ToString("D"), d.Document.MimeType, d.Document.Name) { IsViewable = d.Document.IsViewable, Size = Convert.ToInt32(d.Document.Size), Kind = d.Document.Kind }).ToArray();
                Document[] desiredDocuments;
                string requestId = request.Source.ID.ToString();
                IDictionary<string, string> requestProperties;
                processor.Request(requestId, networkSetting.CreateInterfaceMetadata(), request.CreateInterfaceMetadata(), requestDocuments, out requestProperties, out desiredDocuments);

                _log.Info("Request posted: " + request.Source.Identifier + " (ID: " + requestId + ")");
                _log.Info("Number of documents available: " + requestDocuments.Length);
                _log.Info("Number of documents desired: " + desiredDocuments.Length);
                if (requestProperties != null && requestProperties.Count > 0)
                {
                    _log.Info("Properties: ");
                    foreach (string key in requestProperties.Keys)
                        _log.Info("Key: " + key + "=" + requestProperties[key]);
                }

                // TODO[ddee] Needs to update the requestProperties here, but do not have a proper status.
                // Temporarily using InProgress.
                // BMS: Don't report inprogress status until portal is fixed to display status in routings
                // DnsServiceManager.SetRequestStatus(request, DnsServiceManager.ConvertModelRequestStatus(processor.Status(requestId)), requestProperties, networkSetting);

                foreach (Lpp.Dns.DataMart.Model.Document requestDocument in desiredDocuments)
                {
                    _log.Info("About to post desired document id: " + requestDocument.DocumentID);
                    DocumentChunkStream requestDocumentStream = new DocumentChunkStream(Guid.Parse(requestDocument.DocumentID), networkSetting);
                    processor.RequestDocument(requestId, requestDocument.DocumentID, requestDocumentStream);
                    _log.Info("Posted desired document id: " + requestDocument.DocumentID);
                }

                _log.Info("Starting request with local request: " + request.Source.Identifier + " (ID: " + requestId + ")");
                processor.Start(requestId);
                _log.Info("Start finished on request with local request: " + request.Source.Identifier + " (ID: " + requestId + ")");

                return requestId;
            }
            catch (Exception ex)
            {
                _log.Error("Unexpected exception in Util.ProcessRequest.", ex);
                throw;
            }
        }
    }
}