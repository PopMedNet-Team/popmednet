using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Client.Utils;
using System.IO;
using System.Windows.Forms;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using log4net;
using System.Xml.Serialization;
using System.Drawing;

namespace Lpp.Dns.DataMart.Client.Controls
{
    public class DataMartViewPanel : ViewPanel
    {
        static readonly ILog _log = LogManager.GetLogger( "DataMartViewPanel" );
        private SaveFileDialog SaveFile;

        // For viewing request documents
        private Document[] documents;
        private NetWorkSetting networkSetting;

        // For viewing response documents
        private IModelProcessor processor;
        private string requestId;
        private bool isRequestView = false;

        public DataMartViewPanel()
            : base()
        {
            this.SaveFile = new System.Windows.Forms.SaveFileDialog();
            FILELIST.CellContentClick += FILELIST_CellContentClick;
        }

        /// <summary>
        /// Pass the request's documents to the View Panel
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="networkSetting"></param>
        public void SetRequestDocuments(Document[] documents, NetWorkSetting networkSetting, IModelProcessor processor)
        {
            this.processor = processor;
            this.documents = documents;
            this.networkSetting = networkSetting;
            FileListDataSource = documents;
            isRequestView = true;
            foreach (Lpp.Dns.DataMart.Model.Document document in documents)
            {
                Guid documentID = Guid.Parse(document.DocumentID);

                if (!document.IsViewable && string.Equals("application/json", document.MimeType, StringComparison.OrdinalIgnoreCase))
                {
                    //the request is using the QueryComposer model processor, attempt to deserialize the document to determine the viewing url
                    try
                    {
                        using (var stream = new DocumentChunkStream(documentID, networkSetting))
                        using(var reader = new StreamReader(stream))
                        {
                            string query = reader.ReadToEnd();

                            var jobj = Newtonsoft.Json.Linq.JObject.Parse(query);
                            string viewUrl = (string)jobj["Header"]["ViewUrl"];
                            
                            if (!string.IsNullOrEmpty(viewUrl))
                            {
                                ShowMimeType = "application/URL";
                               
                                string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Crypto.EncryptString(string.Format("{0}:{1}:{2}", networkSetting.Username, networkSetting.DecryptedPassword, DateTime.UtcNow.ToString("s")))));
                                string url = viewUrl + "&token=" + token;
                                DataSource = url;
                                return;
                            }
                        }
                    }
                    catch {
                        //deserialization failed, not a document that we want
                    }
                }

                if (document.IsViewable)
                {
                    
                    using (var documentStream = new DocumentChunkStream(documentID, networkSetting))
                    {
                        ShowMimeType = document.MimeType;
                        SetDataSourceStream(documentStream);
                    }
                    return;
                }
            }

            ShowView = DisplayType.FILELIST;
        }

        public void ShowErrorMessage(string errors)
        {
            ShowView = DisplayType.HTML;
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body style='font-family:arial,san serif;'><p style='text-align:center;color:red;'>Error Getting Results</p>");
            if (!string.IsNullOrWhiteSpace(errors))
            {
                sb.Append("<p>" + errors.Replace(Environment.NewLine, "<br/>") + "</p>");
            }
            sb.Append("</body></html>");
            DataSource = sb.ToString();
        }

        public void ShowProcessingMessage()
        {
            ShowView = DisplayType.HTML;
            DataSource = "<html><body style='font-family:arial,san-serif;'><p style='text-align:center;color:blue;'>Processing Request...</p><p style='font-size:0.8em;text-align:center;'>Started at " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss tt") + ".</p></body></html>";
        }

        /// <summary>
        /// Passes the response's documents to the View Panel
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="processor"></param>
        /// <param name="requestId"></param>
        public void SetResponseDocuments(Document[] documents, IModelProcessor processor, string requestId)
        {
            this.processor = processor;
            this.documents = documents;
            this.requestId = requestId;
            isRequestView = false;
            var processorStatus = processor.Status(requestId);
            if (processorStatus.Code == RequestStatus.StatusCode.Error)
            {
                ShowErrorMessage(processorStatus.Message);
                return;
            }

            // If "documents" is null, then no document is available, show a message.
            if (documents == null)
            {
                ShowView = DisplayType.HTML;
                DataSource = "<html><body><p style='text-align:center;color:gray;font-family:arial'>No Result Documents Available.</p></body></html>";
                return;
            }

            FileListDataSource = documents;

            Document d = documents.Where(f => f.IsViewable).FirstOrDefault();
            Document s = documents.Where(f => f.Filename == "ViewableDocumentStyle.xml").FirstOrDefault();
            Document j = documents.Where(f => f.Filename == "response.json").FirstOrDefault();
            if (d != null)
            {
                Stream contentStream = null;
                processor.ResponseDocument(requestId, d.DocumentID, out contentStream, 100);
                ShowMimeType = d.MimeType;
                if (s != null)
                {
                    Stream styleStream = null;

                    processor.ResponseDocument(requestId, s.DocumentID, out styleStream, 100);
                    XmlSerializer serializer = new XmlSerializer(typeof(ViewableDocumentStyle));
                    SetDataSourceStream(contentStream, (ViewableDocumentStyle)serializer.Deserialize(new StreamReader(styleStream)));
                    contentStream.Close();
                    styleStream.Close();
                }
                else if (j != null)
                {
                    ShowView = DisplayType.JSON;
                    Stream styleStream = null;

                    processor.ResponseDocument(requestId, j.DocumentID, out styleStream, 100);
                   
                    SetDataSourceStream(contentStream, null);
                    contentStream.Close();
                    styleStream.Close();
                    
                }
                else
                {
                    SetDataSourceStream(contentStream);
                    contentStream.Close();
                }
            }
            else
            {
                ShowView = DisplayType.FILELIST;
            }
        }

        public void AddDocument(Document document, Stream stream)
        {
            IList<Document> documentList = documents.ToList<Document>();
            documentList.Add(document);
            FileListDataSource = documentList.ToArray<Document>();
        }


        /// <summary>
        /// Handle cellContentClick event (saving the document to disk)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FILELIST_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.RowIndex >= 0)
                {
                    bool IsCheckBoxColumn = false;
                    IsCheckBoxColumn = (FILELIST.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell != null);
                    if (IsCheckBoxColumn)
                    {
                        bool IsSelected = !(bool)(FILELIST.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell).Value;
                        (FILELIST.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell).Value = IsSelected;
                        FILELIST.Rows[e.RowIndex].Selected = IsSelected;
                        FILELIST.MultiSelect = true;
                    }
                    else
                    {
                        Document document = documents[e.RowIndex];
                        Savefile(document);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public List<Document> GetSelectedFiles()
        {
            List<Document> SelectedFiles = new List<Document>();

            if (FILELIST.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in FILELIST.Rows)
                {
                    foreach (DataGridViewColumn col in FILELIST.Columns)
                    {
                        bool IsCheckBoxColumn = false;
                        IsCheckBoxColumn = (FILELIST.Rows[row.Index].Cells[col.Index] as DataGridViewCheckBoxCell != null);
                        if (IsCheckBoxColumn)
                        {
                            bool IsSelected = (bool)(FILELIST.Rows[row.Index].Cells[col.Index] as DataGridViewCheckBoxCell).Value;
                            if (IsSelected)
                            {
                                Document document = documents[row.Index];
                                SelectedFiles.Add(document);
                            }
                            break;
                        }
                    }
                }
            }

            return SelectedFiles;
        }

        /// <summary>
        /// Save Files attached to query
        /// </summary>
        /// <param name="doc">Document object </param>
        private void Savefile(Document doc)
        {
            SaveFile.FileName = Path.GetFileName(doc.Filename);
            SaveFile.RestoreDirectory = true;
            SaveFile.Filter = "Text file (*.txt)|*.txt|XML file (*.xml)|*.xml|All files (*.*)|*.*";

            if (doc.Filename.EndsWith("xml", StringComparison.OrdinalIgnoreCase))
            {
                SaveFile.FilterIndex = 2;
            }
            else if(!doc.Filename.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
            {
                string fileExtension = Path.GetExtension(doc.Filename);
                if(fileExtension.NullOrEmpty())
                {
                    if (!doc.MimeType.NullOrEmpty() && doc.MimeType.IndexOf("/") >= 0)
                    {
                        fileExtension = "." + doc.MimeType.Substring(doc.MimeType.IndexOf('/')+1);
                        SaveFile.Filter = string.Format("{1} files (*{0})|*{0}|{2}", fileExtension, fileExtension.Substring(1, 1).ToUpper() + fileExtension.Substring(2), SaveFile.Filter);
                    }
                }
                else
                    SaveFile.Filter = string.Format("{1} files (*{0})|*{0}|{2}", fileExtension, fileExtension.Substring(1, 1).ToUpper() + fileExtension.Substring(2), SaveFile.Filter);
            }

            if (SaveFile.ShowDialog() != DialogResult.OK) return;

            var sSaveLocation = SaveFile.FileName;

            SFTPClient sftpClient = null;
            var progress = new ProgressForm( "Saving File", "Transferring the File from the Portal..." ) { Indeteminate = doc.Size <= 0 };
            byte[] buffer = new byte[0x100000];
            Observable.Using( () => File.Create( sSaveLocation ),
                outStream =>
                    Observable.Using( () =>
                    {
                        Stream dcStream;
                        if (isRequestView == true)
                        {
                            if (doc.MimeType != "application/vnd.pmn.lnk")
                            {
                                dcStream = new DocumentChunkStream(Guid.Parse(doc.DocumentID), networkSetting);
                            }
                            else
                            {
                                string lnk = string.Empty;
                                using (var docStream = new DocumentChunkStream(Guid.Parse(doc.DocumentID), networkSetting))
                                {
                                    lnk = new StreamReader(docStream).ReadToEnd();
                                    docStream.Close();
                                }
                                if (networkSetting.SftpClient == null)
                                    networkSetting.SftpClient = new SFTPClient(networkSetting.Host, networkSetting.Port, networkSetting.Username, networkSetting.DecryptedPassword);
                                dcStream = networkSetting.SftpClient.FileStream(lnk);
                            }
                        }
                        else
                            processor.ResponseDocument(requestId, doc.DocumentID, out dcStream, 60000);
                        return dcStream;
                    },
                    inStream => 
                        Observable.Return(0, Scheduler.ThreadPool).Repeat()
                        .Select(_ => inStream.Read(buffer, 0, buffer.Length))
                        .TakeWhile(bytesRead => bytesRead > 0)
                        .Do(bytesRead => outStream.Write(buffer, 0, bytesRead))
                        .Scan(0, (totalTransferred, bytesRead) => totalTransferred + bytesRead)
                        .ObserveOn(this)
                        //.Do(totalTransferred => progress.Progress = totalTransferred * 100 / Math.Max(doc.Size, 1))
                        .Do(totalTransferred => progress.Progress = totalTransferred * 100 / Math.Max((int)inStream.Length, 1))
                   ) 
            )
            .SubscribeOn(Scheduler.ThreadPool)
            .TakeUntil( progress.ShowAndWaitForCancel( this ) )
            .ObserveOn(this)
            .Finally( progress.Dispose )
            .LogExceptions( _log.Error )
            .Do( _ => {},
                ex => { MessageBox.Show(this, "There was an error saving the file: \r\n\r\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); },
                () => { MessageBox.Show(this, "File downloaded successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); })
            .Catch()
            .Subscribe();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DataMartViewPanel
            // 
            this.Name = "DataMartViewPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}