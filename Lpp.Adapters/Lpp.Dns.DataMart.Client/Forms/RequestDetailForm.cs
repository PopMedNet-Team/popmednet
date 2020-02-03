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

namespace Lpp.Dns.DataMart.Client
{
    public partial class RequestDetailForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Lpp.Dns.DataMart.Client.DomainManger.DomainManager _domainManager = new DomainManger.DomainManager(Configuration.PackagesFolderPath);
        readonly IModelProcessor Processor;        
        readonly HubRequest Request;
        readonly ModelDescription ModelDesc;
        private Boolean wasRejected;
        public RequestDetailForm()
        {
            InitializeComponent();
            descriptionBrowser.Navigate("about:blank");
        }

        public RequestDetailForm(HubRequest request)
        {
            Request = request;

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
            }
            else
            {
                Processor = null;
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

            queryStatusTypeIdTextBox.DataBindings[0].Format += (o, e) => {
                if (e.DesiredType != typeof(string))
                    return;
                //format the routing status enum description based on the value's description attribute.
                e.Value = HubRequestStatus.GetDescription((DTO.DataMartClient.Enums.DMCRoutingStatus)e.Value);
            };
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

                if(Processor is IEarlyInitializeModelProcessor)
                {
                    ((IEarlyInitializeModelProcessor)Processor).Initialize(ModelDesc.ModelId, Request.Documents.Select(d => new DocumentWithStream(d.ID, new Document(d.ID, d.Document.MimeType, d.Document.Name, d.Document.IsViewable, Convert.ToInt32(d.Document.Size), d.Document.Kind), new DocumentChunkStream(d.ID, netWorkSetting))).ToArray());
                }

                vpRequest.SetRequestDocuments(requestDocuments, netWorkSetting, Processor);
                
                //set the initial value of the view request file list checkbox and then hook up the checked change event
                cbRequestFileList.Checked = vpRequest.ShowView == Lpp.Dns.DataMart.Client.Controls.ViewPanel.DisplayType.FILELIST;
                cbRequestFileList.CheckedChanged += new System.EventHandler(this.cbRequestFileList_CheckedChanged);

                EnableDisableButtons();

                RequestStatus.StatusCode statusCode;

                // Status is complete or complete with message.
                if (((statusCode = Processor.Status(RequestId).Code) & RequestStatus.StatusCode.Complete) == RequestStatus.StatusCode.Complete)
                {
                    DisplayResponse(Processor.Status(RequestId));
                }

                if (statusCode == RequestStatus.StatusCode.Error)
                {
                    MessageBox.Show("There was an error when this request was last run.\nThe error was:\n\n" + Processor.Status(RequestId).Message, Application.ProductName);
                }

                if (Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedAfterUpload || Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected || Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedBeforeUpload)
                {
                    wasRejected = true;
                }   

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
            // Warn that result may be different from completed run.
            if (Request.RoutingStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed)
            {
                DialogResult d = MessageBox.Show("This request has been previously run and uploaded.\nRe-running it may produce different results due to changed data\nand cannot be re-uploaded.\n\nPress [OK} to run, [Cancel] to return.", 
                    Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (d == DialogResult.Cancel)
                    return;
            }

            btnRun.Enabled = false;
            btnRun.Text = "Running...";
            vpResponse.ShowProcessingMessage();
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
            if ( rejectReasonForm.ShowDialog() != DialogResult.OK ) return;
            var rejectReason = rejectReasonForm.RejectReason;

            this.Cursor = Cursors.WaitCursor;
            var responseDocuments = Processor.Response( RequestId );

            if (responseDocuments.NullOrEmpty())
            {
                responseDocuments=new Document[1]{new Document(new Guid().ToString(),"Text","")};
            }
            var progress = new ProgressForm( "Uploading the result", "Uploading the request results..." ) { Indeteminate = false };
            var postDocuments = 
                responseDocuments.NullOrEmpty()
                ? Observable.Return( 100 )
                : Observable.Start<Guid[]>(
                    () => DnsServiceManager.PostResponseDocuments( RequestId, Request.DataMartId, responseDocuments, netWorkSetting ),
                    Scheduler.Default )
                .SelectMany( docIds =>
                    docIds.Zip( responseDocuments, (srvId,localDoc) => new { srvId, localDoc } )
                    .Select( ( doc, index ) => Observable.Using(
                        () =>
                        {
                            Stream contentStream;
                            Processor.ResponseDocument( RequestId, doc.localDoc.DocumentID.ToString(), out contentStream, 0x100000 );
                            return contentStream;
                        },
                        inStream => inStream == null ? Observable.Return( 100*(index+1) / docIds.Length ) :
                            DnsServiceManager.PostResponseDocumentContent( doc.srvId, inStream, netWorkSetting )
                            .Select( bytes => (100*index + bytes*100/Math.Max( 1, doc.localDoc.Size ))/ docIds.Length )
                    ) )
                    .Concat()
                );

            postDocuments
                .ObserveOn( this )
                .Do( percent => progress.Progress = percent )
                .Select( _ => Unit.Default )
                .Concat( Observable.Defer( () => Observable.Start( () =>
                    {
                        DnsServiceManager.SetRequestStatus( Request, new HubRequestStatus( DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval, rejectReason ), Request.Properties, netWorkSetting );
                        RequestCache.ForNetwork( netWorkSetting ).Release( Request );
                    }, Scheduler.Default )
                    .ObserveOn( this )
                    .Do( _ => this.Close() )
                ) )
                .TakeUntil( progress.ShowAndWaitForCancel( this ) )
                .Finally( progress.Dispose )
                .LogExceptions( log.Error )
                .Catch( (ModelProcessorError ex) => ShowPossibleTransientError( ex ) )
                .Catch( (Exception ex) => ShowUnexpectedError( ex ) )
                .Do( _ => {}, () =>
                {
                    EnableDisableButtons();
                    RefreshRequestHeader();
                    this.Cursor = Cursors.Default;
                } )
                .Catch()
                .Subscribe();
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
            NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

            // Prompt user to enter the reason
            RejectReasonForm f = new RejectReasonForm();
            f.Text = "DataMart Client - Hold Reason";
            f.ButtonText = "Hold";
            f.FormText = "Enter a reason for holding the query below. Query results can still be executed after holding the results. The query status will be marked as 'Awaiting Approval on the Portal and the reason entered below will be returned to the originator of the query.";
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
                    netWorkSetting.Profile.Username, Request.Source.ID, Request.Source.Identifier ) );

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

        // FIXME[ddee]: Used to color some cells yellow when cells are below threshold. Need to figure a way
        // to embed this in the XML.
        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    try
        //    {
        //        string strColumns = "dispensings members daysSupply events";
        //    DataGridViewCell dgvc = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //    if (null == dgvc.Value)
        //        return;
        //    double d;
        //    if (strColumns.ToLower().Contains(dataGridView1.Columns[e.ColumnIndex].HeaderText.ToLower()) &&  double.TryParse(dgvc.Value.ToString(), out d) && d < Convert.ToDouble(CellThreshHold))
        //        dataGridView1.Rows[dgvc.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
        //    }
        //    catch (Exception ex)
        //    {
        //        DMClient.Util.ShowException(ex);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DataTable TransformJSONToDataTable(string json)
        {
            try
            {
                var results = new DataTable();
                var response = JObject.Parse(json);
                foreach (var row in response["Results"][0])
                {
                    var datarow = results.NewRow();
                    foreach (var jToken in row)
                    {
                        var jproperty = jToken as JProperty;
                        if (jproperty == null) continue;
                        if (results.Columns[jproperty.Name] == null)
                            results.Columns.Add(jproperty.Name, typeof(string));
                        datarow[jproperty.Name] = jproperty.Value.ToString();
                    }
                    //if (!results.Columns.Contains("LowThreshold"))
                    //{
                    //    results.Columns.Add("LowThreshold", typeof(string));
                    //    datarow["LowThreshold"] = "False";
                    //}
                    //if (datarow["LowThreshold"].ToString() == "")
                    //{
                    //    datarow["LowThreshold"] = "False";
                    //}
                    if (results.Columns.Contains("LowThreshold"))
                    {
                        results.Columns.Remove("LowThreshold");
                    }

                    results.Rows.Add(datarow);
                }

                return results;
            }
            catch (Exception wx)
            {
                throw wx;
            }

        }

        private void btnExportResults_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable tableToExport = null;

                Lpp.Dns.DataMart.Model.Document[] responseDocuments = Processor.Response(RequestId);
                DataSet _dataSet = new DataSet();
                foreach (Lpp.Dns.DataMart.Model.Document responseDocument in responseDocuments)
                {
                    Stream contentStream = null;
                    try
                    {
                        Processor.ResponseDocument( RequestId, responseDocument.DocumentID, out contentStream, 100 );
                        if ( responseDocument.IsViewable )
                        {
                            if (responseDocument.MimeType != "application/json")
                            {
                                _dataSet.ReadXml(contentStream);
                                contentStream.Close();
                            }
                            else
                            {
                                string content = new StreamReader( contentStream ).ReadToEnd();
                                tableToExport = TransformJSONToDataTable(content);
                            }
                            break;
                        }
                    }
                    finally
                    {
                        contentStream.CallDispose();
                    }
                }

                if (_dataSet != null && _dataSet.Tables.Count > 0)
                    tableToExport = _dataSet.Tables[0];


                if (tableToExport == null || tableToExport.Rows.Count == 0)
                {
                    MessageBox.Show("No result data available for export.", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Show Message To Get The File Name to Save Exported Content.
                string FileName = string.Empty;
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save file as...";
                dialog.Filter = "Excel File (*.xls)|*.xls|CSV File (*.txt)|*.txt";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = dialog.FileName;
                    ExportData.ExportFormat fileFormat = (dialog.FilterIndex == 1) ? ExportData.ExportFormat.Excel : ExportData.ExportFormat.CSV;

                    ExportData objExport = new ExportData();
                    objExport.ExportDetails(tableToExport, fileFormat, FileName);
                    MessageBox.Show(string.Format("Result successfully exported to file {0}.", FileName), "Results Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            DisplayResults();
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            try
            {
                FileDialog.Title = "Open File";
                FileDialog.Filter = "All Files|*.*";
                FileDialog.FileName = "";
                try
                {
                    FileDialog.InitialDirectory = 
                        Properties.Settings.Default.AddFileInitialFolder.NullOrEmpty() 
                        ? Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments )
                        : Properties.Settings.Default.AddFileInitialFolder;
                }
                catch { }
                FileDialog.ShowDialog();
                if (FileDialog.FileName == "") return;

                Properties.Settings.Default.AddFileInitialFolder = Path.GetDirectoryName( FileDialog.FileName );
                Properties.Settings.Default.Save();

                //Cannot handle file of size greatedr than int32.MaxValue.
                if (System.IO.File.Exists(FileDialog.FileName))
                {
                    System.IO.FileInfo finfo = new System.IO.FileInfo(FileDialog.FileName);
                    if (finfo.Length > Int32.MaxValue)
                        MessageBox.Show("The file is too large. Please select a file smaller than " + Int32.MaxValue.ToString("0,000,000,000") + " bytes.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        AddFileToResult(FileDialog.FileName);
                }
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
                if (vpResponse.FILELIST.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete the selected files from the response?", "Delete File") == DialogResult.OK)
                    {
                        foreach (Document d in vpResponse.GetSelectedFiles())
                        {
                            Processor.RemoveResponseDocument(RequestId, d.DocumentID);
                        }
                        Lpp.Dns.DataMart.Model.Document[] responseDocuments = Processor.Response(RequestId);
                        vpResponse.SetResponseDocuments(responseDocuments, Processor, RequestId);
                        vpResponse.ShowView = ViewPanel.DisplayType.FILELIST;
                    }
                }
                else
                {
                    MessageBox.Show("At least one file should selected ");
                    return;
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUnexpectedError(ex);
            }
        }

        private void processRequestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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

                MessageBox.Show(this, string.Format("Request {0} failed to run on DataMart: {1}.{2}"+
                    "There may be data in the response panel but it may be incomplete and cannot be uploaded.{2}" +
                    "This error may be transient and you may try again.{2}Please check log for details.{2}{2}{3}", Request.Source.Identifier, Request.DataMartName, Environment.NewLine, errorMessage), "Run Request Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);                
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

        private void ProcessRequest(object sender, DoWorkEventArgs e)
        {
            NetWorkSetting netWorkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);

            try
            {
                Processor.SetRequestProperties( Request.Source.ID.ToString(), Request.Properties );
                ProcessRequest(Request, Processor, netWorkSetting, false);

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
                    if(!wasRejected)
                    {
                        Request.RoutingStatus = newStatus.Code;
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

                log.Info(String.Format("User {0} executed the following query {3} (ID: {1}):\n\n{2}", netWorkSetting.Profile.Username, Request.Source.ID, Request.Source.Name, Request.Source.Identifier ) );
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
                    RequestCache.ForNetwork( netWorkSetting ).Lock( Request );
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

        private void DisplayResults()
        {
            Lpp.Dns.DataMart.Model.Document[] documents = Processor.Response(RequestId);
            cbResponseFileList.Checked = !documents.Any(s => s.IsViewable);
            vpResponse.SetResponseDocuments(documents, Processor, RequestId);
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

                if (status.Code == RequestStatus.StatusCode.CompleteWithMessage && !string.IsNullOrEmpty(status.Message) )
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

        /// <summary>
        /// Add files to response document list.
        /// </summary>
        /// <param name="FilePath"></param>
        private void AddFileToResult(string FilePath)
        {
            try
            {
                Processor.AddResponseDocument(RequestId, FilePath);
                Lpp.Dns.DataMart.Model.Document[] responseDocuments = Processor.Response(RequestId);
                vpResponse.SetResponseDocuments(responseDocuments, Processor, RequestId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        static readonly Guid FileDistributionProcessorID = new Guid("C8BC0BD9-A50D-4B9C-9A25-472827C8640A");
        static readonly Guid ModularProgramModelID = new Guid("1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154");
        static readonly Guid FileDistributionModelID = new Guid("00BF515F-6539-405B-A617-CA9F8AA12970");

        /// <summary>
        /// Button states:
        /// 
        /// REQUEST STATUS     Submitted   InProgress   AwaitingApproval   Completed   Hold   Rejected   Failed   Cancelled
        /// Run:                  Y            N              Y             Y (warn)    Y        Y         Y          Y
        /// Hold:                 Y            Y              Y                N        N        N         Y          N
        /// Reject:               Y            Y              Y                N        Y        N         Y          N
        /// Add File:             Y            Y              Y                Y        Y        Y         Y          Y
        /// Delete File:          Y            Y              Y                Y        Y        Y         Y          Y
        /// Export Results:       Y            N              Y                Y        Y        Y         N          Y
        /// Upload Results:       N            N              Y                N        Y        N         N          N
        /// Close:                Y            Y              Y                Y        Y        Y         Y          Y
        /// </summary>
        private void EnableDisableButtons()
        {
            btnRun.Enabled = true;
            btnRejectQuery.Enabled = btnHold.Enabled = btnUploadResults.Enabled = btnExportResults.Enabled = false;

            try
            {
                Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus hubStatus = Request.RoutingStatus;
                RequestStatus processorStatus = Processor.Status(Request.Source.ID.ToString());
                RequestStatus.StatusCode processorStatusCode = processorStatus.Code;
                NetWorkSetting networkSetting = Configuration.Instance.GetNetworkSetting(Request.NetworkId);
                bool CanRunAndUpload = true; 
                bool CanUploadWithoutRun = false;
                bool CanViewSQL = false;

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
                
                if (Processor != null
                    && Processor.ModelMetadata != null
                    && Processor.ModelMetadata.Capabilities != null
                    && Processor.ModelMetadata.Capabilities.ContainsKey("CanViewSQL")
                    && (bool)Processor.ModelMetadata.Capabilities["CanViewSQL"])
                {
                    CanViewSQL = true;
                }

                btnRun.Enabled = CanRunAndUpload && DnsServiceManager.CheckUserRight(Request, HubRequestRights.Run, networkSetting) && processorStatusCode != RequestStatus.StatusCode.InProgress;
                btnHold.Enabled = DnsServiceManager.CheckUserRight(Request, HubRequestRights.Hold, networkSetting) &&
                                  hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed &&
                                  hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold &&
                                  hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                                  hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled;
                btnRejectQuery.Enabled = DnsServiceManager.CheckUserRight(Request, HubRequestRights.Reject, networkSetting) &&
                                         hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed &&
                                         hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                                         hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled;
                btnExportResults.Enabled = processorStatusCode != RequestStatus.StatusCode.InProgress &&
                                           hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed &&
                                           (processorStatusCode & RequestStatus.StatusCode.Complete) == RequestStatus.StatusCode.Complete;
                
                //Old upload Results button enabler
                //btnUploadResults.Enabled = (CanRunAndUpload || CanUploadWithoutRun)
                //                        && DnsServiceManager.CheckUserRight(Request, HubRequestRights.Run, networkSetting) &&
                //                        hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed &&
                //                        hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                //                        hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed &&
                //                        hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                //                        (CanUploadWithoutRun ||
                //                             (processorStatusCode & RequestStatus.StatusCode.Complete) == RequestStatus.StatusCode.Complete
                //                         )
                //                         && !wasRejected;

                //As Per PMNDEV-4303: The upload 
                if (Processor.ModelProcessorId == FileDistributionProcessorID || Request.Source.ModelID == ModularProgramModelID || Request.Source.ModelID == FileDistributionModelID)
                {
                    //New upload Results button enabler
                    btnUploadResults.Enabled = (CanRunAndUpload || CanUploadWithoutRun)
                                            && DnsServiceManager.CheckUserRight(Request, HubRequestRights.Run, networkSetting) &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                                            !wasRejected;

                    //As Per PMNDEV-4303: The text on the Upload Results button needs to be changed to "Re-Upload Results" when the request has been completed before.
                    if (btnUploadResults.Enabled && 
                        (hubStatus == Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed || hubStatus == DTO.DataMartClient.Enums.DMCRoutingStatus.ResultsModified)
                        && (processorStatusCode & RequestStatus.StatusCode.Complete) != RequestStatus.StatusCode.Complete)
                        btnUploadResults.Text = "Re-Upload Results";
                }
                else
                {
                    btnUploadResults.Enabled = (CanRunAndUpload || CanUploadWithoutRun)
                                            && DnsServiceManager.CheckUserRight(Request, HubRequestRights.Run, networkSetting) &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed &&
                                            hubStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                                            (CanUploadWithoutRun ||
                                                 (processorStatusCode & RequestStatus.StatusCode.Complete) == RequestStatus.StatusCode.Complete
                                             )
                                             && !wasRejected;
                }

                btnDeleteFile.Enabled = btnAddFile.Enabled = Processor.ModelMetadata.Capabilities.ContainsKey("AddFiles") && Processor.ModelMetadata.Capabilities["AddFiles"] == true;

                btnPostProcess.Enabled = processorStatus.PostProcess;

                btnViewSQL.Enabled = CanViewSQL;
            }
            catch(Exception ex)
            {
                // If hub or processor is unavailable for status, then light up only "run".
                log.Error("Unable to reach hub or processor for status to determine button statuses.", ex);
            }
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
                    if ( request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold &&
                        request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected &&
                        request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled &&
                        request.RoutingStatus != Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed)
                    {
                        var newStatus = DnsServiceManager.ConvertModelRequestStatus( processor.Status( requestId ) );
                        if (request.RoutingStatus != newStatus.Code && !wasRejected)
                        {
                            request.RoutingStatus = newStatus.Code;
                            DnsServiceManager.SetRequestStatus(request, newStatus, requestProperties, networkSetting);
                        }
                    }
                }
                foreach (var requestDocument in desiredDocuments)
                {
                    log.Debug("About to post desired document ID: " + requestDocument.DocumentID);
                    DocumentChunkStream requestDocumentStream = new DocumentChunkStream(Guid.Parse(requestDocument.DocumentID), networkSetting);
                    processor.RequestDocument(requestId, requestDocument.DocumentID, requestDocumentStream);
                    log.Debug("Posted desired document ID: " + requestDocument.DocumentID);
                }

                log.Debug("Starting request with local request identifier: "+ request.Source.Identifier + " (ID: " + request.Source.ID.ToString("D") + ")");

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
                log.Debug("Start finished on request with local request identifier: " + request.Source.Identifier + " (ID: " + request.Source.ID.ToString("D") + ")");

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

                        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO>(json, serializationSettings);
                        foreach(IEnumerable<Dictionary<string, object>> results in obj.Results)
                        {
                            object val = null;
                            if (results.First().TryGetValue("SQL", out val))
                            {
                                sql.Add((string)val);
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cbRequestFileList_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void lblRequest_Click(object sender, EventArgs e)
        {

        }

        private void vpRequest_Load(object sender, EventArgs e)
        {

        }

        private void vpResponse_Load(object sender, EventArgs e)
        {

        }
    }
}