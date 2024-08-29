using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Client.Properties;
using System.IO;
using System.Collections;
using Lpp.Dns.DataMart.Lib.Classes;
using System.Reflection;
using Lpp.Dns.DataMart.Client.Controls;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DataMart.Lib.RequestQueue;
using log4net;
using System.Threading;
using Lpp.Dns.DataMart.Lib.Utils;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client
{
    public partial class RequestDetailForm : Form
    {
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static readonly Guid FileDistributionProcessorID = new Guid("C8BC0BD9-A50D-4B9C-9A25-472827C8640A");
        static readonly Guid ModularProgramModelID = new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154");
        static readonly Guid FileDistributionModelID = new Guid("00BF515F-6539-405B-A617-CA9F8AA12970");
        static readonly Guid DistributedRegressionModelID = new Guid("4C8A25DC-6816-4202-88F4-6D17E72A43BC");

        DomainManger.DomainManager _domainManager = new DomainManger.DomainManager(Configuration.PackagesFolderPath);
        IModelProcessor Processor;
        public HubRequest Request;
        readonly ModelDescription ModelDesc;
        bool wasRejected;
        readonly DTO.DataMartClient.Enums.DMCRoutingStatus _initialStatusOnLoad;
        DTO.DataMartClient.Enums.DMCRoutingStatus _initialStatusOnRun = DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted;
        readonly Lib.Caching.DocumentCacheManager Cache;
        IPatientIdentifierProcessor _patientIdentifierProcessor = null;
        BackgroundWorker _patientIdentifierGenerationBackgroundWorker = null;
        BackgroundWorker processRequestWorker = null;
        ProgressForm _patIDprogressForm = new ProgressForm("Generating PatID Lists", "Generating Patient Identifier lists...") { Indeteminate = true, ShowInTaskbar = false };

        public RequestDetailForm()
        {
            InitializeComponent();
            descriptionBrowser.Navigate("about:blank");
        }

        public RequestDetailForm(HubRequest request)
        {
            Request = request;
            _initialStatusOnRun = Request.RoutingStatus;
            _initialStatusOnLoad = Request.RoutingStatus;

            ModelDesc = Configuration.Instance.GetModelDescription(Request.NetworkId, Request.DataMartId, Request.Source.ModelID);

            if (ModelDesc != null && ModelDesc.ProcessorId != Guid.Empty)
            {
                var packageIdentifier = new Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier() { Identifier = Request.Source.RequestTypePackageIdentifier, Version = Request.Source.AdapterPackageVersion };
                if (!System.IO.File.Exists(System.IO.Path.Combine(Configuration.PackagesFolderPath, packageIdentifier.PackageName())))
                {
                    var networkSetting = Configuration.Instance.GetNetworkSetting(request.NetworkId);
                    DnsServiceManager.DownloadPackage(networkSetting, packageIdentifier);
                }

                _domainManager.Load(Request.Source.RequestTypePackageIdentifier, Request.Source.AdapterPackageVersion);
                Processor = _domainManager.GetProcessor(ModelDesc.ProcessorId);
                ProcessorManager.UpdateProcessorSettings(ModelDesc, Processor);
                Processor.Settings.Add("NetworkId", request.NetworkId);

				Cache = new Lib.Caching.DocumentCacheManager(request.NetworkId, request.DataMartId, request.Source.ID, request.Source.Responses.SingleOrDefault(r => r.DataMartID == request.DataMartId).ResponseID);

				if ((Request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || Request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted) && Cache.HasResponseDocuments)
                {
                    //if there are cached response documents and the request has not been run yet set the status to pending upload
                    Request.RoutingStatus = DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload;
                }
            }
            else
            {
                Processor = null;
                Cache = null;
            }

            InitializeComponent();

            string description = Request.Source.Description;
            string linkPattern = @"</?a(?:(?= )[^>]*)?>";
            string imgPattern = @"<img[^>]+\>";
            string iframePattern = @"<iframe[^>]+>.*?<\/iframe>";
            List<string> allPatterns = new List<string>();
            allPatterns.Add(linkPattern);
            allPatterns.Add(imgPattern);
            allPatterns.Add(iframePattern);

            string newDesc = Regex.Replace(description, string.Join("|", allPatterns), "");

            descriptionBrowser.DocumentText = newDesc;

            queryStatusTypeIdTextBox.DataBindings[0].Format += (o, e) =>
            {
                if (e.DesiredType != typeof(string))
                    return;
                //format the routing status enum description based on the value's description attribute.
                e.Value = HubRequestStatus.GetDescription((DTO.DataMartClient.Enums.DMCRoutingStatus)e.Value);
            };

            this.Text = "DataMart Client - " + Request.Source.MSRequestID;
            lblStatusBarProgress.Text = string.Empty;

            tlpRequestDetails.CellPaint += tlpRequestDetails_CellPaint;
            tlpResponseDetails.CellPaint += tlpRequestDetails_CellPaint;

            tlpHeaderDetails.BackColor = Color.Transparent;

            tlpResponseDetails.PerformLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Paint the header background, spans the layout panel and the spacer panel
            int bottom = tabControl1.Location.Y - 1;
            e.Graphics.FillRectangle(SystemBrushes.GradientInactiveCaption, new Rectangle(new Point(0, 0), new Size(this.ClientRectangle.Width, bottom)));
            e.Graphics.DrawLine(SystemPens.GradientActiveCaption, new Point(0, bottom), new Point(this.ClientRectangle.Width, bottom));
        }

        void tlpRequestDetails_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row > 0)
            {
                //Draw divider line between rows for the request and response detail table layout panels
                e.Graphics.DrawLine(SystemPens.ControlDark, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));
            }
        }

        public string RequestId
        {
            get { return Request.Source.ID.ToString(); }
        }


        private void RequestDetailForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (ModelDesc == null)
                {
                    MessageBox.Show(this.ParentForm, "The configuration settings for " + Request.Source.RequestTypeName + " have not been completed.", Application.ProductName + " - Unable to Process Request", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (ModelDesc != null && Processor == null)
                {
                    MessageBox.Show(this.ParentForm, "Unable to process request.\nThe Processor ID for the request type '" + ModelDesc.ModelName + "' has not been defined.", Application.ProductName + " - Unable to Process Request", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (Processor == null || ModelDesc == null)
                {
                    Close();
                    return;
                }

                NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

                // Bind the header data to the Request.
                RefreshRequestHeader();

                Lpp.Dns.DataMart.Model.Document[] requestDocuments = Request.Documents.Select(d => new Lpp.Dns.DataMart.Model.Document(d.ID.ToString("D"), d.Document.MimeType, d.Document.Name) { IsViewable = d.Document.IsViewable, Size = Convert.ToInt32(d.Document.Size), Kind = d.Document.Kind }).ToArray();

                if (Processor is IEarlyInitializeModelProcessor)
                {
                    ((IEarlyInitializeModelProcessor)Processor).Initialize(ModelDesc.ModelId, Request.Documents.Select(d => new DocumentWithStream(d.ID, new Document(d.ID, d.Document.MimeType, d.Document.Name, d.Document.IsViewable, Convert.ToInt32(d.Document.Size), d.Document.Kind), new DocumentChunkStream(d.ID, netWorkSetting))).ToArray());
                }

                if(Processor is IPatientIdentifierProcessor)
                {
                    _patientIdentifierProcessor = (IPatientIdentifierProcessor)Processor;
                    chkGeneratePATIDList.Visible = _patientIdentifierProcessor.CanGenerateLists;

                    if (_patientIdentifierProcessor.CanGenerateLists)
                    {
                        _patientIdentifierGenerationBackgroundWorker = new BackgroundWorker();
                        _patientIdentifierGenerationBackgroundWorker.WorkerSupportsCancellation = false;
                        _patientIdentifierGenerationBackgroundWorker.WorkerReportsProgress = false;
                        _patientIdentifierGenerationBackgroundWorker.DoWork += (object s, DoWorkEventArgs workArgs) => {

                            _patientIdentifierProcessor.GenerateLists(Request.Source.ID, netWorkSetting.CreateInterfaceMetadata(), Request.CreateInterfaceMetadata(), (IDictionary<Guid,string>)workArgs.Argument, "csv");
                            workArgs.Result = workArgs.Argument;

                        };
                        _patientIdentifierGenerationBackgroundWorker.RunWorkerCompleted += (object s, RunWorkerCompletedEventArgs workArgs) => {
                            
                            _patIDprogressForm.Close();

                            if(workArgs.Error != null)
                            {
                                ShowUnexpectedError(workArgs.Error);
                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine("Patient Identifier list sucessfully generated and saved to:");
                                foreach (KeyValuePair<Guid, string> outputPath in ((IDictionary<Guid, string>)workArgs.Result))
                                {
                                    sb.AppendLine(outputPath.Value);
                                }

                                MessageBox.Show(this, sb.ToString(), "Patient Identifier List Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }

                        };
                    }

                }
                else
                {
                    chkGeneratePATIDList.Visible = false;
                }

                vpRequest.Initialize(netWorkSetting, Processor, RequestId, true);
                vpRequest.SetRequestDocuments(requestDocuments);

                //set the initial value of the view request file list checkbox and then hook up the checked change event
                cbRequestFileList.Checked = vpRequest.ShowView == Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.FILELIST;
                cbRequestFileList.CheckedChanged += new System.EventHandler(this.cbRequestFileList_CheckedChanged);

                vpResponse.Initialize(netWorkSetting, Processor, RequestId, false);

                if (Cache == null)
                {
                    vpResponse.DataSource = null;
                    cbResponseFileList.Checked = vpResponse.ShowView == ViewPanel.DisplayType.FILELIST;
                }
                else
                {
                    if (Cache.HasResponseDocuments && (Request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || Request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted))
                    {
                        Request.RoutingStatus = DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload;
                    }

                    vpResponse.InitializeResponseDocumentsFromCache(Cache);
                }

                //cbResponseFileList.CheckedChanged += new EventHandler(cbResponseFileList_CheckedChanged);

                bool isFileDistributionRequest = (Processor.ModelProcessorId == FileDistributionProcessorID ||
                                                  Request.Source.ModelID == FileDistributionModelID ||
                                                  Request.Source.ModelID == ModularProgramModelID ||
                                                  Request.Source.ModelID == DistributedRegressionModelID ||
                                                  //The adapter will indicate that it is a file based request based on the specified terms and updated the capability.
                                                  (Processor.ModelMetadata.Capabilities.ContainsKey("IsFileDistributionRequest") && Processor.ModelMetadata.Capabilities["IsFileDistributionRequest"]));

                if (isFileDistributionRequest)
                {
                    cbRequestFileList.Checked = true;
                    cbResponseFileList.Checked = true;
                }

                if (Request.Source.Attachments != null && Request.Source.Attachments.Any() && DnsServiceManager.CheckUserRight(Request, HubRequestRights.ViewAttachments, netWorkSetting))
                {
                    vpAttachments.Initialize(netWorkSetting, Processor, RequestId, true);
                    vpAttachments.SetAttachments(Request.Source.Attachments.Select(d => new Lpp.Dns.DataMart.Model.Document(d.ID.ToString("D"), d.Document.MimeType, d.Document.Name) { IsViewable = d.Document.IsViewable, Size = Convert.ToInt32(d.Document.Size), Kind = d.Document.Kind }).ToArray());
                }
                else
                {   
                    lblInputAttachments.Visible = false;
                    vpAttachments.Visible = false;
                }

                cbResponseFileList.CheckedChanged += new EventHandler(cbResponseFileList_CheckedChanged);

                EnableDisableButtons();

                RequestStatus.StatusCode statusCode = Processor.Status(RequestId).Code;

                if (statusCode == RequestStatus.StatusCode.Error)
                {
                    MessageBox.Show("There was an error when this request was last run.\nThe error was:\n\n" + Processor.Status(RequestId).Message, Application.ProductName);
                }

                if (Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedAfterUpload || Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected || Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedBeforeUpload)
                {
                    wasRejected = true;
                }

                processRequestWorker = new BackgroundWorker();
                processRequestWorker.WorkerReportsProgress = true;
                processRequestWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.processRequestWorker_ProcessRequest);
                processRequestWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.processRequestWorker_ProgressChanged);
                processRequestWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.processRequestWorker_RunWorkerCompleted);

            }
            catch (ModelProcessorError ex)
            {
                log.Debug(ex);
                ShowPossibleTransientError(ex);
            }
            catch (Exception ex)
            {
                if (Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed)
                    MessageBox.Show("The last run of this request failed.", "Previous Run Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    log.Error(ex);
                    ShowUnexpectedError(ex);
                }
            }
            finally
            {
                EnableDisableButtons();
            }

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (chkGeneratePATIDList.Checked)
            {
                
                var selectionDialog = new OutputPathSelectionForm(_patientIdentifierProcessor.GetQueryIdentifiers());
                if(selectionDialog.ShowDialog(this) == DialogResult.OK)
                {
                    //DialogResult d = MessageBox.Show("Generating PatID list...");
                    
                    _patientIdentifierGenerationBackgroundWorker.RunWorkerAsync(selectionDialog.OutputPaths);
                    _patIDprogressForm.ShowDialog(this);
                    
                }
                return;
            }

            // Warn that result may be different from completed run.
            if (Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed)
            {
                DialogResult d = MessageBox.Show("This request has been previously run and uploaded.\nRe-running it may produce different results due to changed data\nand cannot be re-uploaded.\n\nPress [OK} to run, [Cancel] to return.",
                    Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (d == DialogResult.Cancel)
                    return;
            }

            _initialStatusOnRun = Request.RoutingStatus;
            btnRun.Enabled = false;
            btnRun.Text = "Running...";
            tabControl1.SelectedTab = tabPageResponseDetails;
            vpResponse.ShowProcessingMessage();
            lblStatusBarProgress.Text = "Processing Request... Started at " + DateTime.Now.ToString("MM/d/yyyy h:mm:ss tt");

            //on run clear the cache
            Cache.ClearCache();

            processRequestWorker.RunWorkerAsync(Request);
        }

        private void btnUploadResults_Click(object sender, EventArgs e)
        {
            var netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

            var rejectReasonForm = new RejectReasonForm
            {
                Text = "DataMart Client - Upload Comments",
                ButtonText = "Upload",
                FormText = "Enter comments below if additional information should be provided about your results. The comments below will be returned to the originator of the query along with the results."
            };

            if (rejectReasonForm.ShowDialog() != DialogResult.OK)
                return;

            var rejectReason = rejectReasonForm.RejectReason;

            this.Cursor = Cursors.WaitCursor;

            Document[] responseDocuments;
            if (Cache.Enabled) {
                responseDocuments = Cache.GetResponseDocuments().ToArray();
            }
            else
            {
                responseDocuments = Processor.Response(RequestId);
            }

            if (netWorkSetting.SupportsUploadV2)
            {
                var progress = new ProgressForm("Uploading the result", "Uploading the request results...") { Indeteminate = false };
                double progressPercentage = 0;
                BackgroundWorker uploadWorker = new BackgroundWorker();
                uploadWorker.WorkerSupportsCancellation = false;
                uploadWorker.WorkerReportsProgress = true;

                uploadWorker.DoWork += (object s, DoWorkEventArgs workArgs) => {
                    Parallel.ForEach(responseDocuments,
                        new ParallelOptions { MaxDegreeOfParallelism = 4 },
                        (doc) =>
                        {
                            var cacheDocument = doc as Lib.Caching.CacheDocument;
                            if(cacheDocument != null && !string.IsNullOrEmpty(cacheDocument.UploadedOn))
                            {
                                //document has already been uploaded, do not upload again
                                return;
                            }

                            Stream stream;

                            if (Cache.Enabled)
                            {
                                stream = Cache.GetDocumentStream(Guid.Parse(doc.DocumentID));
                            }
                            else
                            {
                                Processor.ResponseDocument(RequestId, doc.DocumentID, out stream, doc.Size);
                            }

                            var beforeMath = 25 / responseDocuments.Length;

                            progressPercentage = Math.Ceiling(progressPercentage + beforeMath);
                            //TODO: use the calculated percentage
                            uploadWorker.ReportProgress(0);

                            var dto = new DTO.DataMartClient.Criteria.DocumentMetadata
                            {
                                ID = Guid.Parse(doc.DocumentID),
                                DataMartID = Request.DataMartId,
                                RequestID = Guid.Parse(RequestId),
                                IsViewable = doc.IsViewable,
                                Size = doc.Size,
                                MimeType = doc.MimeType,
                                Kind = doc.Kind,
                                Name = doc.Filename,
                                CurrentChunkIndex = 0
                            };

                            string uploadIdentifier = ("[" + Utilities.Crypto.Hash(Guid.NewGuid()) + "]").PadRight(16);
                            DnsServiceManager.PostDocumentChunk(uploadIdentifier, dto, stream, netWorkSetting);

                            double afterMath = 75 / responseDocuments.Length;
                            
                            progressPercentage = Math.Ceiling(progressPercentage + afterMath);
                            //TODO: use the calculated percentage
                            uploadWorker.ReportProgress(0);

                            if(cacheDocument != null)
                            {
                                Cache.MarkUploaded(cacheDocument);
                            }

                        });


                    DnsServiceManager.SetRequestStatus(Request, new HubRequestStatus(DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval, rejectReason), Request.Properties, netWorkSetting);
                    RequestCache.ForNetwork(netWorkSetting).Release(Request);

                };

                uploadWorker.ProgressChanged += (object progressSender, ProgressChangedEventArgs progressArgs) =>
                {
                    progress.Progress = Convert.ToInt32(progressPercentage);
                };

                uploadWorker.RunWorkerCompleted += (object completedSender, RunWorkerCompletedEventArgs workCompletedArgs) =>
                {
                    if (workCompletedArgs.Error != null)
                    {
                        log.Error(workCompletedArgs.Error);
                        if (workCompletedArgs.Error is ModelProcessorError)
                        {
                            ShowPossibleTransientError(workCompletedArgs.Error);
                        }
                        else
                        {
                            ShowUnexpectedError(workCompletedArgs.Error);
                        }
                        progress.Close();
                        progress.Dispose();
                    }
                    else
                    {
                        progress.Close();
                        progress.Dispose();
                        this.Close();
                    }
                };

                uploadWorker.RunWorkerAsync();
                progress.ShowDialog();

            }
            else
            {

                var progress = new ProgressForm("Uploading the result", "Uploading the request results...") { Indeteminate = false };

                var postDocuments = responseDocuments.NullOrEmpty() ? Observable.Return(100) : Observable.Start<Document[]>(() => responseDocuments, Scheduler.Default)
                    .SelectMany(documents =>
                    {
                        return documents.Select((doc, index) =>
                        {
                            return Observable.Defer(() =>
                            {
                                string uploadIdentifier = ("[" + Utilities.Crypto.Hash(Guid.NewGuid()) + "]").PadRight(16);
                                Guid[] serverIDs = DnsServiceManager.PostResponseDocuments(uploadIdentifier, RequestId, Request.DataMartId, new[] { doc }, netWorkSetting);
                                Guid serverDocumentID = serverIDs[0];

                                Thread.Sleep(3000);

                                return Observable.Using(() =>
                                {
                                    if (Cache.Enabled)
                                    {
                                        return Cache.GetDocumentStream(Guid.Parse(doc.DocumentID));
                                    }
                                    else
                                    {
                                        Stream data;
                                        Processor.ResponseDocument(RequestId, doc.DocumentID, out data, doc.Size);
                                        return data;
                                    }
                                },
                                   inStream =>
                                   {
                                       return (inStream == null) ?
                                                     Observable.Return(100 * (index + 1) / responseDocuments.Length) :
                                                     DnsServiceManager.PostResponseDocumentContent(uploadIdentifier, RequestId, Request.DataMartId, serverDocumentID, doc.Filename, inStream, netWorkSetting).Select(bytes => (100 * index + bytes * 100 / Math.Max(1, doc.Size)) / responseDocuments.Length);
                                   }
                                );
                            });


                        }).Concat();
                    });

                postDocuments.ObserveOn(this)
                    .Do(percent => progress.Progress = percent)
                    .Select(_ => Unit.Default)
                    .Concat(Observable.Defer(() => Observable.Start(() =>
                    {
                        DnsServiceManager.SetRequestStatus(Request, new HubRequestStatus(DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval, rejectReason), Request.Properties, netWorkSetting);
                        RequestCache.ForNetwork(netWorkSetting).Release(Request);
                    }, Scheduler.Default)
                      .ObserveOn(this)
                      .Do(_ =>
                      {

                          this.Close();

                      })
                    ))
                    .TakeUntil(progress.ShowAndWaitForCancel(this))
                    .Finally(progress.Dispose)
                    .LogExceptions(log.Error)
                    .Catch((ModelProcessorError ex) => ShowPossibleTransientError(ex))
                    .Catch((Exception ex) => ShowUnexpectedError(ex))
                    .Do(_ => { }, () =>
                    {
                        EnableDisableButtons();
                        RefreshRequestHeader();
                        this.Cursor = Cursors.Default;
                    })
                    .Catch()
                    .Subscribe();
            }
        }

        private void btnRejectQuery_Click(object sender, EventArgs e)
        {
            NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

            // Prompt user to enter the reason for the rejection
            RejectReasonForm f = new RejectReasonForm();
            f.ShowDialog();

            // If user clicked cancel, bail out
            if (f.DialogResult == DialogResult.Cancel)
                return;

            // Update the query status
            try
            {
                DnsServiceManager.SetRequestStatus(Request, new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedBeforeUpload, f.RejectReason), Request.Properties, netWorkSetting);
                Request.RejectReason = f.RejectReason;
                Request.RoutingStatus = Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedBeforeUpload;

                MessageBox.Show("The status of the request has been successfully updated to \"Response Rejected Before Upload\".", Application.ProductName);

                RefreshRequestHeader();

                log.Info(String.Format("User {0} rejected the following request {3} (ID: {1}) with reason \"{2}\"",
                    netWorkSetting.Profile.Username, Request.Source.ID, f.RejectReason, Request.Source.Identifier));

                this.Close();
            }
            catch (ModelProcessorError ex)
            {
                log.Debug(ex);
                ShowPossibleTransientError(ex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUnexpectedError(ex);
            }
            finally
            {
                EnableDisableButtons();
            }
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            if(Request.RoutingStatus != DTO.DataMartClient.Enums.DMCRoutingStatus.Hold)
            {
                NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

                // Prompt user to enter the reason
                RejectReasonForm f = new RejectReasonForm();
                f.Text = "DataMart Client - Hold Reason";
                f.ButtonText = "Hold";
                //f.FormText = "Enter a reason for holding the query below. Query results can still be executed after holding the results. The query status will be marked as 'Awaiting Approval on the Portal and the reason entered below will be returned to the originator of the query.";
                f.FormText = "Enter a reason for holding the query below. The query status will be marked as 'Awaiting Approval' on the Portal and the reason entered below will be returned to the originator of the query.";
                f.ShowDialog();

                // If user clicked cancel, bail out
                if (f.DialogResult == DialogResult.Cancel)
                    return;

                // Update the query status
                try
                {
                    DnsServiceManager.SetRequestStatus(Request, new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold, f.RejectReason), Request.Properties, netWorkSetting);
                    Request.RejectReason = f.RejectReason;
                    Request.RoutingStatus = Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold;

                    MessageBox.Show("The status of the request has been successfully updated to \"Hold\".", Application.ProductName);

                    // Notify parent that the record has changed
                    RefreshRequestHeader();

                    log.Info(String.Format("User {0} put the following request {2} (ID: {1}) on hold.",
                        netWorkSetting.Profile.Username, Request.Source.ID, Request.Source.Identifier));

                    this.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    ShowUnexpectedError(ex);
                }
                finally
                {
                    EnableDisableButtons();
                }
            }
            else
            {
                NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

                // Prompt user to enter the reason
                RejectReasonForm f = new RejectReasonForm();
                f.Text = "DataMart Client - Resume Reason";
                f.ButtonText = "Resume";
                f.FormText = "Enter a reason for resuming the query below. The query status will be marked as 'Submitted/Re-Submitted' on the Portal and the reason entered below will be returned to the originator of the query.";
                f.ShowDialog();

                // If user clicked cancel, bail out
                if (f.DialogResult == DialogResult.Cancel)
                    return;

                // Update the query status
                try
                {
                    DnsServiceManager.SetRequestStatus(Request, new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted, f.RejectReason), Request.Properties, netWorkSetting);

                    var updatedRequest = DnsServiceManager.GetRequests(netWorkSetting, new Guid[] { Request.Source.ID }, Request.DataMartId).GetAwaiter().GetResult();

                    Request.RejectReason = f.RejectReason;
                    Request.RoutingStatus = updatedRequest.Routings.FirstOrDefault(x => x.DataMartID == Request.DataMartId).Status;

                    if (Request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted) {
                        MessageBox.Show("The status of the request has been successfully updated to \"Re-Submitted\".", Application.ProductName);
                    }
                    else
                    {
                        MessageBox.Show("The status of the request has been successfully updated to \"Submitted\".", Application.ProductName);
                    }

                    // Notify parent that the record has changed
                    RefreshRequestHeader();

                    log.Info(String.Format("User {0} put the following request {2} (ID: {1}) on hold.",
                        netWorkSetting.Profile.Username, Request.Source.ID, Request.Source.Identifier));

                    this.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    ShowUnexpectedError(ex);
                }
                finally
                {
                    EnableDisableButtons();
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        const string LowThresholdColumnName = "LowThreshold";
        DataSet TransformJSONToDataSet(string json)
        {
            DataSet ds = new DataSet();

            var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
            serializationSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
            Newtonsoft.Json.JsonSerializer serializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);

            JObject response = JObject.Parse(json);
            if(response.Properties().Any(p => string.Equals("SchemaVersion", p.Name)))
            {

                var multiQueryResponse = response.ToObject<DTO.QueryComposer.QueryComposerResponseDTO>(serializer);

                foreach(var queryResponse in multiQueryResponse.Queries)
                {
                    var table = new DataTable();
                    table.TableName = queryResponse.Name;

                    foreach(var property in queryResponse.Properties)
                    {
                        if (property.As == LowThresholdColumnName)
                            continue;

                        var column = new DataColumn();
                        column.ColumnName = property.As;
                        column.DataType = Nullable.GetUnderlyingType(property.AsType()) ?? property.AsType();
                        column.Caption = property.As;
                        table.Columns.Add(column);
                    }

                    table.BeginLoadData();
                    foreach(var row in queryResponse.Results.First())
                    {
                        table.LoadDataRow(row.Where(r => r.Key != LowThresholdColumnName).Select(r => r.Value == null ? DBNull.Value : r.Value).ToArray(), true);
                    }
                    table.EndLoadData();

                    ds.Tables.Add(table);
                }

            }
            else if(response.Properties().Any(p => string.Equals("Results", p.Name)))
            {
                var singleQueryResponse = response.ToObject<DTO.QueryComposer.QueryComposerResponseQueryResultDTO>(serializer);

                var table = new DataTable();
                table.TableName = string.IsNullOrEmpty(singleQueryResponse.Name) ? "Query 1" : singleQueryResponse.Name;

                foreach (var property in singleQueryResponse.Properties)
                {
                    if (property.As == LowThresholdColumnName)
                        continue;

                    var column = new DataColumn();
                    column.ColumnName = property.As;
                    column.DataType = Nullable.GetUnderlyingType(property.AsType()) ?? property.AsType();
                    column.Caption = property.As;
                    table.Columns.Add(column);
                }

                table.BeginLoadData();
                foreach (var row in singleQueryResponse.Results.First())
                {
                    table.LoadDataRow(row.Where(r => r.Key != LowThresholdColumnName).Select(r => r.Value == null ? DBNull.Value : r.Value).ToArray(), true);
                }
                table.EndLoadData();

                ds.Tables.Add(table);

            }
            else
            {
                throw new NotSupportedException("Unable to translate json into table format.");
            }

            return ds;
        }

        private void btnExportResults_Click(object sender, EventArgs e)
        {
            try
            {

                IEnumerable<Document> responseDocuments;

                if (Cache.Enabled)
                    responseDocuments = Cache.GetResponseDocuments();
                else
                    responseDocuments = Processor.Response(RequestId);

                DataSet dataSet = new DataSet();
                foreach (Lpp.Dns.DataMart.Model.Document responseDocument in responseDocuments)
                {
                    Stream contentStream = null;
                    try
                    {
                        if (responseDocument.IsViewable)
                        {
                            if (Cache.Enabled)
                            {
                                contentStream = Cache.GetDocumentStream(Guid.Parse(responseDocument.DocumentID));
                            }
                            else
                            {
                                Processor.ResponseDocument(RequestId, responseDocument.DocumentID, out contentStream, responseDocument.Size);
                            }

                            if (responseDocument.MimeType != "application/json")
                            {
                                dataSet.ReadXml(contentStream);
                                contentStream.Close();
                            }
                            else
                            {
                                string content = new StreamReader(contentStream).ReadToEnd();
                                dataSet = TransformJSONToDataSet(content);
                            }
                            break;
                        }
                    }
                    finally
                    {
                        contentStream.CallDispose();
                    }
                }

                if (dataSet == null || dataSet.Tables.Count < 1 || dataSet.Tables.Cast<DataTable>().Sum(tbl => tbl.Rows.Count) == 0)
                {
                    MessageBox.Show("No result data available for export.", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Show Message To Get The File Name to Save Exported Content.
                string filename = string.Empty;
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save file as...";
                dialog.Filter = "Excel File (*.xlsx)|*.xlsx|CSV File (*.csv)|*.csv";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    filename = dialog.FileName;
                    ExportData.ExportFormat fileFormat = (dialog.FilterIndex == 1) ? ExportData.ExportFormat.Excel : ExportData.ExportFormat.CSV;

                    ExportData objExport = new ExportData();
                    string exportedFilename = objExport.ExportDetails(dataSet, fileFormat, filename);
                    string exportMessage = $"Results successfully exported.{Environment.NewLine + Environment.NewLine}Export file: {exportedFilename}";
                    if(exportedFilename.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        exportMessage += Environment.NewLine + Environment.NewLine + "** Please note that multi-query CSV exports will be saved as a Zip file with the specified filename.  Each query result will be saved to a separate file using the name of the cohort within the zip file. **";
                    }
                    MessageBox.Show(exportMessage, "Results Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ModelProcessorError ex)
            {
                log.Debug(ex);
                ShowPossibleTransientError(ex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EnableDisableButtons();
            }
        }

        private void btnPostProcess_Click(object sender, EventArgs e)
        {
            Processor.PostProcess(RequestId);
            btnPostProcess.Enabled = false;
            Cache.ClearCache();
            DisplayResults();
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            try
            {
                if(Cache == null)
                {
                    vpResponse.AddResponseDocument();
                }
                else
                {
                    vpResponse.AddResponseDocument(AddDocumentsToCache);
                }
                EnableDisableButtons();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUnexpectedError(ex);
            }
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                vpResponse.DeleteSelectedFiles();
                
                EnableDisableButtons();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUnexpectedError(ex);
            }
        }

        private void processRequestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblStatusBarProgress.Text = string.Empty;
            if (e.Error != null)
            {
                vpResponse.ShowErrorMessage(e.Error.Message);
                ShowUnexpectedError(e.Error);
                return;
            }

            if (Visible)
            {
                try
                {
                    DisplayResponse(Processor.Status(RequestId));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Request.RoutingStatus = Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed;
                    vpResponse.ShowErrorMessage(ex.ToString());
                    return;
                }
                finally
                {

                    btnRun.Text = "Run";
                    EnableDisableButtons();
                }
            }

            if (Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed)
            {
                String errorMessage;
                try
                {
                    errorMessage = Processor.Status(RequestId).Message;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    errorMessage = ex.Message;
                }

                vpResponse.ShowErrorMessage(errorMessage);

                MessageBox.Show(this, string.Format("Request {0} failed to run on DataMart: {1}.{2}" +
                    "There may be data in the response panel but it may be incomplete and cannot be uploaded.{2}" +
                    "This error may be transient and you may try again.{2}Please check log for details.{2}{2}{3}", Request.Source.Identifier, Request.DataMartName, Environment.NewLine, errorMessage), "Run Request Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DisplayResults()
        {
            Lpp.Dns.DataMart.Model.Document[] documents = Processor.Response(RequestId);
            cbResponseFileList.Checked = !documents.Any(s => s.IsViewable);
            vpResponse.SetResponseDocuments(documents);
            AddDocumentsToCache(documents);
        }

        void AddDocumentsToCache(IEnumerable<Document> documents)
        {
            Cache.Add(documents.Select(d => {
                Stream data;
                Processor.ResponseDocument(RequestId, d.DocumentID, out data, d.Size);

                Guid documentID;
                if (!Guid.TryParse(d.DocumentID, out documentID))
                {
                    documentID = Utilities.DatabaseEx.NewGuid();
                    d.DocumentID = documentID.ToString();
                }

                return new DocumentWithStream(documentID, d, data);
            }));
        }

        private void DisplayResponse(RequestStatus status)
        {
            try
            {
                if (status.Code != RequestStatus.StatusCode.Error)
                {
                    DisplayResults();
                }
                else
                {
                    vpResponse.ShowErrorMessage(status.Message);
                }

                if (status.Code == RequestStatus.StatusCode.CompleteWithMessage && !string.IsNullOrEmpty(status.Message))
                {
                    string message = !string.IsNullOrEmpty(status.Message) ? status.Message : "Model processor indicated that there is a message but no message was provided.";
                    DialogResult dlg = MessageBox.Show(message, "Request Processing Completed Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnPostProcess.Enabled = status.PostProcess;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private void processRequestWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Visible)
            {
                RefreshRequestHeader();
                EnableDisableButtons();
            }
        }

        /// <summary>
        /// Refreshes the Request Header by rebinding the Request to the binding source.
        /// </summary>
        private void RefreshRequestHeader()
        {
            queryDataMartBindingSource.DataSource = Request;
            queryDataMartBindingSource.ResetBindings(false);
        }

        private void processRequestWorker_ProcessRequest(object sender, DoWorkEventArgs e)
        {
            NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

            try
            {
                Processor.SetRequestProperties(Request.Source.ID.ToString(), Request.Properties);

                ProcessRequest(Request, Processor, netWorkSetting, false);
                //THE request has finished processing at this point => i.e. Processor.Start() is done doing it's thing.

                RequestStatus.StatusCode statusCode;
                while (((statusCode = Processor.Status(RequestId).Code) & RequestStatus.StatusCode.STOP) != RequestStatus.StatusCode.STOP)
                {
                    Request.RoutingStatus = DnsServiceManager.ConvertModelRequestStatus(Processor.Status(RequestId)).Code;
                    Thread.Sleep(Properties.Settings.Default.RefreshRate / 100);
                }

                // Update status only if not already held, rejected, canceled or completed.
                // NOTE: We cannot simply return if the request is in these states because we DO want to let the user re-run it
                //       although the results cannot be re-uploaded.
                if (Request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold &&
                    Request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                    Request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                    Request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed)
                {
                    var newStatus = DnsServiceManager.ConvertModelRequestStatus(Processor.Status(RequestId));
                    if (!wasRejected)
                    {
                        if (newStatus.Code == DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval && (_initialStatusOnRun == DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || _initialStatusOnRun == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted || _initialStatusOnRun == DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload))
                        {
                            Request.RoutingStatus = DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload;
                        }
                        else
                        {
                            Request.RoutingStatus = newStatus.Code;
                        }
                    }
                    processRequestWorker.ReportProgress(100, Request.RoutingStatus.ToString());

                    // Failed status on manual run are never reported to the Portal.
                    // BMS: Don't report awaiting approval after execute until we fix the portal to display this status in the routings panel
                    //if(Request.RequestStatus != HubRequestStatus.StatusCode.Failed)
                    //    DnsServiceManager.SetRequestStatus(Request, newStatus, Request.Properties, netWorkSetting);

                    if (Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed)
                    {
                        // We have the results, so let's keep this request in cache
                        RequestCache.ForNetwork(netWorkSetting).Lock(Request);
                    }
                }

                log.Info(String.Format("User {0} executed the following query {3} (ID: {1}):\n\n{2}", netWorkSetting.Profile.Username, Request.Source.ID, Request.Source.Name, Request.Source.Identifier));
            }
            catch (ModelProcessorError ex)
            {
                log.Debug(ex);
                Request.RoutingStatus = Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed;

                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                try
                {
                    Request.RoutingStatus = DnsServiceManager.ConvertModelRequestStatus(Processor.Status(RequestId)).Code;
                    RequestCache.ForNetwork(netWorkSetting).Lock(Request);
                }
                catch (Exception ex2)
                {
                    log.Error(ex2.Message, ex2);
                    Request.RoutingStatus = Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed;
                    throw;
                }

                throw;
            }

        }

        /// <summary>
        /// Button states:
        /// 
        /// REQUEST STATUS     Submitted   InProgress   AwaitingApproval   Completed   Hold   Rejected   Failed   Cancelled
        /// Run:                  Y            N              Y             Y (warn)    N        Y         Y          Y
        /// Hold/Resume:          Y            Y              Y                N        Y        N         Y          N
        /// Reject:               Y            Y              Y                N        N        N         Y          N
        /// Add File:             Y            Y              Y                Y        N        Y         Y          Y
        /// Delete File:          Y            Y              Y                Y        N        Y         Y          Y
        /// Export Results:       Y            N              Y                Y        N        Y         N          Y
        /// Upload Results:       N            N              Y                N        N        N         N          N
        /// Close:                Y            Y              Y                Y        Y        Y         Y          Y
        /// </summary>
        private void EnableDisableButtons()
        {
            btnRun.Enabled = true;
            btnRejectQuery.Enabled = btnHold.Enabled = btnUploadResults.Enabled = btnExportResults.Enabled = btnClearCache.Enabled = false;

            if(Processor == null || chkGeneratePATIDList.Checked)
            {
                //Processor can only be null if the form has been disposed when closing
                //no need to do anything
                return;
            }

            Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus hubStatus = Request.RoutingStatus;
            RequestStatus processorStatus = Processor.Status(Request.Source.ID.ToString());
            RequestStatus.StatusCode processorStatusCode = processorStatus.Code;
            NetWorkSetting networkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);
            bool CanRunAndUpload = true;
            bool CanUploadWithoutRun = false;
            bool CanViewSQL = false;

            if(hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Hold)
            {              
                btnHold.Text = "Resume";
                btnHold.Enabled = true;
                btnRejectQuery.Enabled = btnRun.Enabled = btnAddFile.Enabled = btnDeleteFile.Enabled = btnExportResults.Enabled = btnUploadResults.Enabled = false;
                return;
            }

            if (Processor != null
                && Processor.ModelMetadata != null
                && Processor.ModelMetadata.Capabilities != null
                && Processor.ModelMetadata.Capabilities.ContainsKey("CanRunAndUpload")
                && !(bool)Processor.ModelMetadata.Capabilities["CanRunAndUpload"])
            {
                CanRunAndUpload = false;
            }

            if (Processor != null
                && Processor.ModelMetadata != null
                && Processor.ModelMetadata.Capabilities != null
                && Processor.ModelMetadata.Capabilities.ContainsKey("CanUploadWithoutRun")
                && (bool)Processor.ModelMetadata.Capabilities["CanUploadWithoutRun"])
            {
                CanUploadWithoutRun = true;
            }
            else if(Cache.HasResponseDocuments)
            {
                CanUploadWithoutRun = true;
            }

            if (Processor != null
                && Processor.ModelMetadata != null
                && Processor.ModelMetadata.Capabilities != null
                && Processor.ModelMetadata.Capabilities.ContainsKey("CanViewSQL")
                && (bool)Processor.ModelMetadata.Capabilities["CanViewSQL"])
            {
                CanViewSQL = true;
            }

            bool isFileBasedRequest =   (Processor != null && Processor.ModelProcessorId == FileDistributionProcessorID) ||
                                        Request.Source.ModelID == ModularProgramModelID ||
                                        Request.Source.ModelID == FileDistributionModelID ||
                                        Request.Source.ModelID == DistributedRegressionModelID ||
                                        //The adapter will indicate that it is a file based request based on the specified terms and updated the capability.
                                        (Processor != null && Processor.ModelMetadata.Capabilities.ContainsKey("IsFileDistributionRequest") && Processor.ModelMetadata.Capabilities["IsFileDistributionRequest"]);

            btnRun.Enabled = CanRunAndUpload && DnsServiceManager.CheckUserRight(Request, HubRequestRights.Run, networkSetting) && processorStatusCode != RequestStatus.StatusCode.InProgress;

            //can only change status to Hold when the request hasn't been run yet
            btnHold.Enabled = DnsServiceManager.CheckUserRight(Request, HubRequestRights.Hold, networkSetting) &&
                (hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted || hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload
                || (hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.ResultsModified && isFileBasedRequest));

            btnRejectQuery.Enabled = DnsServiceManager.CheckUserRight(Request, HubRequestRights.Reject, networkSetting) &&
                (hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted || hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted || hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload
                || (hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.ResultsModified && isFileBasedRequest));

            //enable document export if the request is not being processed and there are documents that can be downloaded.
            btnExportResults.Enabled = processorStatusCode != RequestStatus.StatusCode.InProgress && hubStatus != DTO.DataMartClient.Enums.DMCRoutingStatus.Failed
                                       && ((Cache.Enabled && Cache.HasResponseDocuments) || (Cache.Enabled == false && vpResponse.HasDocuments))
                                       && isFileBasedRequest == false;

            if (isFileBasedRequest)
            {
                //New upload Results button enabler
                btnUploadResults.Enabled = (CanRunAndUpload || CanUploadWithoutRun || isFileBasedRequest)
                                        && DnsServiceManager.CheckUserRight(Request, HubRequestRights.Run, networkSetting) &&
                                        hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                                        hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed &&
                                        hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                                        _initialStatusOnRun != DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval &&
                                        !wasRejected &&
                                        vpResponse.HasDocuments;

                //Per PMNDEV-4303: The text on the Upload Results button needs to be changed to "Re-Upload Results" when the request has been completed before.
                //Per comment on PMNDEV-6242 by Zac, if the user has the modify results permission they can re-upload if the route status is awaiting response approval.
                if ((hubStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed ||
                        hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.ResultsModified ||
                        hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval)
                    && (processorStatusCode & RequestStatus.StatusCode.Complete) != RequestStatus.StatusCode.Complete)
                {
                    //Per PMNDEV-5920 the ability to modify results is independent of ability to upload results, only applicable to FD and MP requests.
                    btnUploadResults.Enabled = (CanRunAndUpload || CanUploadWithoutRun)
                                            && DnsServiceManager.CheckUserRight(Request, HubRequestRights.ModifyResults, networkSetting) &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                                            !wasRejected &&
                                            vpResponse.HasDocuments;

                    btnUploadResults.Text = "Re-Upload Results";
                }
            }
            else
            {
                var validHubStatus = new[] { DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted, DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload, DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted, DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval };
                btnUploadResults.Enabled = (CanRunAndUpload || CanUploadWithoutRun)
                                            && DnsServiceManager.CheckUserRight(Request, HubRequestRights.Run, networkSetting) 
                                            && validHubStatus.Contains(hubStatus) 
                                            && _initialStatusOnRun != DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval 
                                            &&
                                            (CanUploadWithoutRun ||
                                                    (processorStatusCode & RequestStatus.StatusCode.Complete) == RequestStatus.StatusCode.Complete
                                                )
                                                && !wasRejected;
            }

            btnDeleteFile.Enabled = btnAddFile.Enabled = Processor.ModelMetadata.Capabilities.ContainsKey("AddFiles") && Processor.ModelMetadata.Capabilities["AddFiles"] == true;

            btnPostProcess.Enabled = processorStatus.PostProcess;

            btnViewSQL.Enabled = CanViewSQL;

            btnClearCache.Enabled = Cache.CanClearRequestSpecificCache;
            
        }

        public string ProcessRequest(HubRequest request, IModelProcessor processor, NetWorkSetting networkSetting, bool viewSQL)
        {
            Document[] requestDocuments = request.Documents.Select(d => new Lpp.Dns.DataMart.Model.Document(d.ID.ToString("D"), d.Document.MimeType, d.Document.Name) { IsViewable = d.Document.IsViewable, Size = Convert.ToInt32(d.Document.Size), Kind = d.Document.Kind }).ToArray();
            Document[] desiredDocuments;
            string requestId = request.Source.ID.ToString();
            IDictionary<string, string> requestProperties;

            processor.Request(requestId, networkSetting.CreateInterfaceMetadata(), request.CreateInterfaceMetadata(), requestDocuments, out requestProperties, out desiredDocuments);

            log.Debug("Request identifier posted: " + request.Source.Identifier + " (ID: " + request.Source.ID.ToString("D") + ")");
            log.Debug("Number of documents available: " + requestDocuments.Length);
            log.Debug("Number of documents desired: " + desiredDocuments.Length);
            if (requestProperties != null && requestProperties.Count > 0)
            {
                log.Debug("Properties: ");
                foreach (string key in requestProperties.Keys)
                {
                    log.Debug("Key: " + key + "=" + requestProperties[key]);
                }
            }

            if (!viewSQL)
            {
                if (request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold &&
                     request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                     request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                     request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed &&
                     request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval)
                {
                    var newStatus = DnsServiceManager.ConvertModelRequestStatus(processor.Status(requestId));
                    if (request.RoutingStatus != newStatus.Code && !wasRejected &&
                        //don't update the status from resubmitted to submitted on run
                        !(request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted && newStatus.Code == DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted)
                      )
                    {
                        request.RoutingStatus = newStatus.Code;
                        DnsServiceManager.SetRequestStatus(request, newStatus, requestProperties, networkSetting);
                    }
                }
            }
            foreach (var requestDocument in desiredDocuments)
            {
                log.Debug("Downloading document" + requestDocument.Filename + $" for Request: {Request.Source.MSRequestID}, DataMart: { Request.DataMartName }");
                DocumentChunkStream requestDocumentStream = new DocumentChunkStream(Guid.Parse(requestDocument.DocumentID), networkSetting);
                processor.RequestDocument(requestId, requestDocument.DocumentID, requestDocumentStream);
                log.Debug("Successfully Downloaded document" + requestDocument.Filename + $" for Request: {Request.Source.MSRequestID}, DataMart: { Request.DataMartName }");
            }

            log.Debug("Starting request with local request identifier: " + request.Source.Identifier + " (ID: " + request.Source.ID.ToString("D") + ")");

            if (!viewSQL)
            {
                if (request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold &&
                    request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                    request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                    request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed)
                {
                    processRequestWorker.ReportProgress(0, request.RoutingStatus);
                }
            }

            processor.Start(requestId, viewSQL);

            log.Debug($"Finished executing request with local request identifier: { request.Source.Identifier} (ID: { request.Source.ID.ToString("D") })");

            return requestId;
        }

        private void ShowUnexpectedError(Exception ex)
        {
            string message = "DataMartClient encountered an unexpected error." + Environment.NewLine + "Please see the log for details." + Environment.NewLine + Environment.NewLine;
            message += "The error reported was:" + Environment.NewLine + ex.Message;
            if (ex.InnerException != null)
                message += Environment.NewLine + Environment.NewLine + "The inner exception was:" + Environment.NewLine + ex.InnerException.Message;
            MessageBox.Show(message, "DataMartClient Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowPossibleTransientError(Exception ex)
        {
            string message = "DataMartClient encountered a model processor error." + Environment.NewLine + "This error may be transient and you may try again later." + Environment.NewLine + Environment.NewLine;
            message += "The error reported was:" + Environment.NewLine + ex.Message;
            if (ex.InnerException != null)
                message += Environment.NewLine + Environment.NewLine + "The inner exception was:" + Environment.NewLine + ex.InnerException.Message;
            MessageBox.Show(message, "DataMartClient Model Processor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void cbRequestFileList_CheckedChanged(object sender, EventArgs e)
        {
            vpRequest.ToggleFileView(sender, e);
        }

        private void cbResponseFileList_CheckedChanged(object sender, EventArgs e)
        {
            vpResponse.ToggleFileView(sender, e);
        }

        private void btnViewSQL_Click(object sender, EventArgs e)
        {
            NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);
            try
            {
                Processor.SetRequestProperties(Request.Source.ID.ToString(), Request.Properties);
                ProcessRequest(Request, Processor, netWorkSetting, true);
                RequestStatus.StatusCode statusCode;
                // BMS:  What's up with the status code values???
                while (((statusCode = Processor.Status(RequestId).Code) & RequestStatus.StatusCode.STOP) != RequestStatus.StatusCode.STOP &&
                    ((statusCode = Processor.Status(RequestId).Code) & RequestStatus.StatusCode.InProgress) == RequestStatus.StatusCode.InProgress)
                {
                    Thread.Sleep(Properties.Settings.Default.RefreshRate / 100);
                }
                IList<string> sql = new List<string>();
                Lpp.Dns.DataMart.Model.Document[] documents = Processor.Response(RequestId);
                Document d = documents.Where(f => f.IsViewable).FirstOrDefault();
                if (d != null)
                {
                    Stream contentStream = null;
                    Processor.ResponseDocument(Request.Source.ID.ToString(), d.DocumentID, out contentStream, 100);
                    if (d.MimeType == "x-application/lpp-dns-table")
                    {
                        DataSet dataSet = new DataSet();
                        dataSet.ReadXml(contentStream, XmlReadMode.Auto);
                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            sql.Add(row.ItemArray.GetValue(0) as string);
                        }
                    }
                    else if (d.MimeType == "text/json" || d.MimeType == "application/json")
                    {
                        string json = null;
                        using (System.IO.StreamReader reader = new StreamReader(contentStream))
                        {
                            json = reader.ReadToEnd();
                        }

                        var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
                        serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());

                        DTO.QueryComposer.QueryComposerResponseQueryResultDTO queryObj = null;
                        try
                        {
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO>(json, serializationSettings);
                            if(obj.Errors != null && obj.Errors.Any())
                            {
                                foreach(var error in obj.Errors)
                                {
                                    sql.Add($"### Error building SQL: ###{ Environment.NewLine }{ error.Description}");
                                }
                            }

                            var queries = obj.Queries.ToArray();
                            for (var i = 0; i < queries.Length; i++)
                            {
                                if (queries[i].Results == null)
                                    continue;

                                foreach (var table in queries[i].Results)
                                {
                                    var row = table.First();
                                    string queryName = Lpp.Utilities.ObjectEx.ToStringEx(row["QueryName"]);
                                    string querySQL = Lpp.Utilities.ObjectEx.ToStringEx(row["SQL"]);
                                    sql.Add($"--### Query { i + 1 }: { queryName } ###{ Environment.NewLine }{querySQL}");
                                }
                            }
                        }
                        catch
                        {
                            queryObj = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerResponseQueryResultDTO>(json, serializationSettings);
                            if (queryObj.Errors != null && queryObj.Errors.Any())
                            {
                                foreach (var error in queryObj.Errors)
                                {
                                    sql.Add($"### Error building SQL: ###{ Environment.NewLine }{ error.Description}");
                                }
                            }
                            foreach (IEnumerable<Dictionary<string, object>> results in queryObj.Results)
                            {
                                object val = null;
                                if (results.First().TryGetValue("SQL", out val))
                                {
                                    sql.Add((string)val);
                                }
                            }
                        }
                        
                        
                    }
                    contentStream.Close();
                }
                ViewSQL dlgViewSQL = new ViewSQL();
                dlgViewSQL.Sql = sql;
                dlgViewSQL.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnClearCache_Click(object sender, EventArgs e)
        {
            if (Cache.CanClearRequestSpecificCache)
            {
                Cache.ClearCache();
                log.Info($"Cache cleared for Request: { Request.Source.MSRequestID }, Path: { Cache.BaseCachePath }");

                cbResponseFileList.Checked = false;
                vpResponse.SetResponseDocuments(Array.Empty<Document>());

                if (Request.RoutingStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload)
                {
                    Request.RoutingStatus = _initialStatusOnLoad;
                    queryDataMartBindingSource.ResetBindings(false);
                }

                EnableDisableButtons();

                MessageBox.Show(this, $"Cache has been successfully cleared for Request: { Request.Source.MSRequestID }.", "Cache Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void chkGeneratePATIDList_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }
    }
}