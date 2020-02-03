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
        static readonly ILog log = LogManager.GetLogger( "DataMartViewPanel" );
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;

        // For viewing request documents
        private Document[] documents;
        private NetWorkSetting networkSetting;

        // For viewing response documents
        private IModelProcessor processor;
        private string requestId;
        private bool isRequestView = false;

        Lib.Caching.DocumentCacheManager _cache = null;

        public DataMartViewPanel()
            : base()
        {
            InitializeComponent();

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|XML file (*.xml)|*.xml|All files (*.*)|*.*";

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open File";
            openFileDialog.Filter = "All Files|*.*";
            openFileDialog.FileName = string.Empty;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            try
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.AddFileInitialFolder.NullOrEmpty() ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : Properties.Settings.Default.AddFileInitialFolder;
            }
            catch { };
        }

        void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DataMartViewPanel
            // 
            this.Name = "DataMartViewPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

            FILELIST.CellContentClick += FILELIST_CellContentClick;
        }

        public void Initialize(NetWorkSetting networkSetting, IModelProcessor processor, string requestID, bool isRequestView)
        {
            this.networkSetting = networkSetting;
            this.processor = processor;
            this.requestId = requestID;
            this.isRequestView = isRequestView;

            if (isRequestView)
            {
                RemoveSelectColumn();
            }

            this.lblNoResults.Visible = false;
        }

        /// <summary>
        /// Pass the request's documents to the View Panel
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="networkSetting"></param>
        public void SetRequestDocuments(Document[] documents)
        {
            this.documents = documents;

            FileListDataSource = documents;

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
        public void SetResponseDocuments(Document[] documents)
        {
            this.documents = documents;

            var processorStatus = processor.Status(requestId);
            if (processorStatus.Code == RequestStatus.StatusCode.Error)
            {
                ShowErrorMessage(processorStatus.Message);
                return;
            }

            FileListDataSource = documents ?? new Document[0];

            // If "documents" is null, then no document is available, show a message.
            if (documents == null || documents.Length == 0)
            {
                ShowView = DisplayType.HTML;
                DataSource = "<html><body><p style='text-align:center;color:gray;font-family:arial'>No Result Documents Available.</p></body></html>";
                return;
            }

            Document d = documents.Where(f => f.IsViewable).FirstOrDefault();
            if (d != null)
            {
                Document s = documents.Where(f => f.Filename == "ViewableDocumentStyle.xml").FirstOrDefault();
                Document j = documents.Where(f => f.Filename == "response.json").FirstOrDefault();

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

        public void InitializeResponseDocumentsFromCache(Lib.Caching.DocumentCacheManager cache)
        {
            _cache = cache;
            this.documents = (cache.GetResponseDocuments() ?? Array.Empty<Document>()).ToArray();

            FileListDataSource = documents;

            // If "documents" is null, then no document is available, show a message.
            if (documents == null || documents.Length == 0)
            {
                //initial view does not show a message
                //ShowView = DisplayType.HTML;
                //DataSource = "<html><body><p style='text-align:center;color:gray;font-family:arial'>No Result Documents Available.</p></body></html>";
                return;
            }

            Document d = documents.Where(f => f.IsViewable).FirstOrDefault();
            if (d != null)
            {
                Document s = documents.Where(f => f.Filename == "ViewableDocumentStyle.xml").FirstOrDefault();
                Document j = documents.Where(f => f.Filename == "response.json").FirstOrDefault();

                using (Stream contentStream = (j != null) ? cache.GetDocumentStream(Guid.Parse(j.DocumentID)) : cache.GetDocumentStream(Guid.Parse(d.DocumentID)))
                {
                    ShowMimeType = d.MimeType;
                    if (s != null)
                    {
                        using (Stream styleStream = cache.GetDocumentStream(Guid.Parse(s.DocumentID)))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ViewableDocumentStyle));

                            SetDataSourceStream(contentStream, (ViewableDocumentStyle)serializer.Deserialize(new StreamReader(styleStream)));
                        }
                    }
                    else if (j != null)
                    {
                        ShowView = DisplayType.JSON;
                        SetDataSourceStream(contentStream, null);
                    }
                    else
                    {
                        SetDataSourceStream(contentStream);
                    }
                }
            }
            else
            {
                ShowView = DisplayType.FILELIST;
                FILELIST.BringToFront();
            }
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
                if(FILELIST.Columns[e.ColumnIndex].Name != "colDocumentSelected")
                {
                    object documentID = FILELIST.Rows[e.RowIndex].Cells["colDocumentID"].Value;
                    if(documentID != null)
                    {
                        Document document = documents.FirstOrDefault(d => d.DocumentID == documentID.ToString());
                        if (document != null)
                        {
                            Savefile(document);
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public IEnumerable<Document> DeleteSelectedFiles()
        {
            var selectedDocuments = GetSelectedFiles();
            if (selectedDocuments.Count > 0)
            {
                if(MessageBox.Show(this, "Are you sure you want to delete the selected file(s)?", "Delete File", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    foreach(var document in selectedDocuments)
                    {
                        processor.RemoveResponseDocument(requestId, document.DocumentID);
                    }

                    documents = processor.Response(requestId);
                    FileListDataSource = documents;
                    ShowView = DisplayType.FILELIST;

                    return selectedDocuments;
                }

            }else
            {
                MessageBox.Show(this.ParentForm, "Please select at least one file first.", "Delete File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return Array.Empty<Document>();
        }

        List<Document> GetSelectedFiles()
        {
            List<Document> selectedFiles = new List<Document>();

            if (FILELIST.Rows.Count > 0)
            {
                object cellValue = null;
                foreach (DataGridViewRow row in FILELIST.Rows)
                {
                    cellValue = row.Cells["colDocumentSelected"].Value;
                    if (cellValue != null && (bool)cellValue)
                    {
                        object documentID = row.Cells["colDocumentID"].Value;
                        if (documentID != null)
                        {
                            Document document = documents.FirstOrDefault(d => d.DocumentID == documentID.ToString());
                            if (document != null)
                            {
                                selectedFiles.Add(document);
                            }
                        }
                    }
                }
            }

            return selectedFiles;
        }

        /// <summary>
        /// Save Files attached to query
        /// </summary>
        /// <param name="doc">Document object </param>
        private void Savefile(Document doc)
        {
            saveFileDialog.FileName = Path.GetFileName(doc.Filename);

            if (doc.Filename.EndsWith("xml", StringComparison.OrdinalIgnoreCase))
            {
                saveFileDialog.FilterIndex = 2;
            }
            else if(!doc.Filename.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
            {
                string fileExtension = Path.GetExtension(doc.Filename);
                if(fileExtension.NullOrEmpty())
                {
                    if (!doc.MimeType.NullOrEmpty() && doc.MimeType.IndexOf("/") >= 0)
                    {
                        fileExtension = "." + doc.MimeType.Substring(doc.MimeType.IndexOf('/')+1);
                        saveFileDialog.Filter = string.Format("{1} files (*{0})|*{0}|{2}", fileExtension, fileExtension.Substring(1, 1).ToUpper() + fileExtension.Substring(2), saveFileDialog.Filter);
                    }
                }
                else
                    saveFileDialog.Filter = string.Format("{1} files (*{0})|*{0}|{2}", fileExtension, fileExtension.Substring(1, 1).ToUpper() + fileExtension.Substring(2), saveFileDialog.Filter);
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            var sSaveLocation = saveFileDialog.FileName;
            
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
                        {
                            if(_cache != null)
                            {
                                dcStream = _cache.GetDocumentStream(Guid.Parse(doc.DocumentID));
                            }
                            else
                            {
                                processor.ResponseDocument(requestId, doc.DocumentID, out dcStream, 60000);
                            }
                        }
                        return dcStream;
                    },
                    inStream => 
                        Observable.Return(0, Scheduler.Default).Repeat()
                        .Select(_ => inStream.Read(buffer, 0, buffer.Length))
                        .TakeWhile(bytesRead => bytesRead > 0)
                        .Do(bytesRead => outStream.Write(buffer, 0, bytesRead))
                        .Scan(0, (totalTransferred, bytesRead) => totalTransferred + bytesRead)
                        .ObserveOn(this)
                        //.Do(totalTransferred => progress.Progress = totalTransferred * 100 / Math.Max(doc.Size, 1))
                        .Do(totalTransferred => progress.Progress = totalTransferred * 100 / Math.Max((inStream.CanSeek ? (int)inStream.Length : doc.Size), 1))
                   ) 
            )
            .SubscribeOn(Scheduler.Default)
            .TakeUntil( progress.ShowAndWaitForCancel( this ) )
            .ObserveOn(this)
            .Finally( progress.Dispose )
            .LogExceptions( log.Error )
            .Do( _ => {},
                ex => { MessageBox.Show(this, "There was an error saving the file: \r\n\r\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); },
                () => { MessageBox.Show(this, "File downloaded successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); })
            .Catch()
            .Subscribe();
        }

        public void AddResponseDocument()
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = openFileDialog.FileName;
                if (!filepath.NullOrEmpty() && File.Exists(filepath))
                {
                    FileInfo fi = new FileInfo(filepath);
                    if(fi.Length > int.MaxValue)
                    {
                        MessageBox.Show("The file is too large. Please select a file smaller than " + Int32.MaxValue.ToString("0,000,000,000") + " bytes.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Properties.Settings.Default.AddFileInitialFolder = fi.DirectoryName;
                        Properties.Settings.Default.Save();
                        
                        processor.AddResponseDocument(requestId, filepath);
                        documents = processor.Response(requestId);
                        FileListDataSource = documents;
                        ShowView = DisplayType.FILELIST;
                    }
                }
            }
        }


    }
}