using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Model;
using System.Data;
using System.IO;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Model
{
    [Serializable]
    public class SASModelProcessor : IModelProcessor
    {
        private const string PROCESSOR_ID = "5d630771-8619-41f7-9407-696302e48237";
        private IModelMetadata modelMetadata = new SASModelMetadata();

        [Serializable]
        internal class SASModelMetadata : IModelMetadata
        {
            private const string MODEL_ID = "82DAECA8-634D-4590-9BD3-77A2324F68D4";
            readonly IDictionary<string, bool> capabilities = new Dictionary<string, bool>() { { "IsSingleton", false }, { "AddFiles", true } };
            readonly IDictionary<string, string> properties = new Dictionary<string, string>() {{ "AutoExecLocation", "" }, { "DMOutput", "" }, {"Prompt", "false" }};

            public SASModelMetadata()
            {
                capabilities = new Dictionary<string, bool>() { { "IsSingleton", false }, { "AddFiles", true } };
                properties = new Dictionary<string, string>() {{ "AutoExecLocation", "" }, { "DMOutput", "" }, {"Prompt", "false" }};
            }

            public string ModelName
            {
                get { return "SASModelProcessor"; }
            }

            public Guid ModelId
            {
                get { return Guid.Parse(MODEL_ID); }
            }

            public string Version
            {
                get { return "0.1"; }
            }

            public IDictionary<string, bool> Capabilities
            {
                get { return capabilities; }
            }

            public IDictionary<string, string> Properties
            {
                get { return properties; }
            }

            public ICollection<Settings.ProcessorSetting> Settings
            {
                get {
                    List<Settings.ProcessorSetting> settings = new List<Settings.ProcessorSetting>();
                    settings.Add(new Settings.ProcessorSetting { Title = "Command File", Key = "AutoExecLocation", DefaultValue = "", ValueType = typeof(Lpp.Dns.DataMart.Model.Settings.FilePickerEditor), EditorSettings = new Lpp.Dns.DataMart.Model.Settings.FilePickerEditor { Title = "Open SAS Command File", Filter = "All Files|*.*", Multiselect = false } });
                    settings.Add(new Settings.ProcessorSetting { Title = "Output Location", Key = "DMOutput", DefaultValue = "", ValueType = typeof(Lpp.Dns.DataMart.Model.Settings.FolderSelectorEditor), EditorSettings = new Lpp.Dns.DataMart.Model.Settings.FolderSelectorEditor { Description = "Open SAS Output Path", ShowNewFolderButton = true } });
                    settings.Add(new Settings.ProcessorSetting { Title = "Ask before running a program", Key = "Prompt", DefaultValue = "false", ValueType = typeof(bool) });
                    return settings;
                }
            }

            public IEnumerable<Settings.SQLProvider> SQlProviders
            {
                get { return Enumerable.Empty<Settings.SQLProvider>(); }
            }
        }


        public Guid ModelProcessorId
        {
            get { return Guid.Parse(PROCESSOR_ID); }
        }

        public IModelMetadata ModelMetadata
        {
            get { return modelMetadata; }
        }

        public IDictionary<string, object> Settings
        {
            get;
            set;
        }

        #region Model Processor Life Cycle Methods

        private string requestId;
        private IList<DataSet> datasets = new List<DataSet>();
        private RequestStatus status = new RequestStatus();
        private Document[] documents;
        private Document[] responseDocuments;
        private RequestMetadata requestMetadata;
        private FileSystemWatcher FileWatcher = null;

        private string sasAutoExec = null;
        Dictionary<string, byte[]> documentContents = new Dictionary<string, byte[]>();

        public void SetRequestProperties(string requestId, IDictionary<string, string> requestProperties)
        {
        }

        public void Request( string requestId, NetworkConnectionMetadata network, RequestMetadata md,
            Document[] requestDocuments, out IDictionary<string, string> requestProperties, out Document[] desiredDocuments )
        {
            this.requestMetadata = md;
            this.requestId = requestId;
            this.documents = requestDocuments;
            requestProperties = null;
            desiredDocuments = requestDocuments;
            status.Code = RequestStatus.StatusCode.InProgress;
            sasAutoExec = Settings.GetAsString("AutoExecLocation", "");
        }

        public void RequestDocument(string requestId, string documentId, Stream contentStream)
        {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                contentStream.CopyTo(ms);
                buffer = ms.ToArray();

            }
            if (documentContents.ContainsKey(documentId))
            {
                documentContents[documentId] = buffer;
            }
            else
            {
                documentContents.Add(documentId, buffer);
            }

        }

        public void Start(string requestId, bool viewSQL)
        {
            System.Diagnostics.ProcessStartInfo SASStartInfo = null;
            System.Diagnostics.Process SASProcess = null;
            try
            {
                if (Settings.Count == 0)
                    throw new Exception(CommonMessages.Exception_MissingSettings);

                string strAutoExec = string.IsNullOrEmpty(sasAutoExec) ? string.Empty : "-AUTOEXEC " + sasAutoExec;

                string strProgram = Settings.GetAsString("DMOutput", "");
                SASProcess = new System.Diagnostics.Process();
                SASStartInfo = new System.Diagnostics.ProcessStartInfo();

                status.Code = RequestStatus.StatusCode.InProgress;

                string networkID = Settings.GetAsString("NetworkId", "");
                bool IsAutoCmd = false;
                string queryFolder = string.Empty;
            
                string _sasOutput = Settings.GetAsString("DMOutput", "");
                queryFolder = _sasOutput;

                if (string.IsNullOrEmpty(sasAutoExec))
                {
                    StreamWriter sw = new StreamWriter(_sasOutput + @"\RunSAS.bat");
                    for (int dCount = 0; dCount < documents.Length; dCount++)
                    {
                        Document doc = documents[dCount];

                        //Rename & save SAS program
                        string saveFileName = networkID + "_" + requestId + "_" + requestMetadata.DataMartId + "_" + documents[dCount].Filename;

                        strProgram = strProgram + @"\" + saveFileName.ToString();

                        sw.WriteLine("Start/w SAS.exe " + strAutoExec + " -sysin " + strProgram + " -nosplash " + @" -ALTLOG " + _sasOutput + @" -ALTPRINT " + _sasOutput);


                        //string dummyBatchFile = @"C:\SAS\Output\autoexe.bat";
                        //sw.WriteLine("Start " + dummyBatchFile + " -sysin " + strProgram + " -nosplash " + @" -ALTLOG " + _sasOutput + @" -ALTPRINT " + _sasOutput);

                        // create a write stream
                        System.IO.FileStream writeStream = new System.IO.FileStream(strProgram, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                        // write to the stream
                        byte[] byteContents;
                        if (!documentContents.TryGetValue(doc.DocumentID, out byteContents))
                        {
                            throw new Exception("Document not found for the specified Document ID:" + doc.DocumentID);
                        }
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteContents))
                        {
                            ReadWriteStream(ms, writeStream, byteContents.Length);
                        }
                        doc = null;
                    }
                    sw.Close();

                    SASStartInfo.FileName = _sasOutput + @"\RunSAS.bat";
                    SASStartInfo.Arguments = @"/c  echo Running SAS program. You may close this window.";
                }
                else
                {
                    IsAutoCmd = true;
                    string programFile = string.Empty;
                    
                    // Build the unqiue path to a directory to run the query
                    string queryIdentifier = networkID + "_" + requestMetadata.DataMartId.ToString() + "_" + requestId.ToString();;
                    queryFolder = Path.Combine(_sasOutput, queryIdentifier);
                    
                    // Create the execution folder
                    DirectoryInfo di = System.IO.Directory.CreateDirectory(queryFolder);

                    // Create an file to identify the inputs to the program
                    //StreamWriter swInputFiles = new StreamWriter(Path.Combine(queryFolder, "input.txt"));

                    // Copy files passed into query to output folder
                    for (int dCount = 0; dCount < documents.Length; dCount++)
                    {
                        // Copy the file to the output folder
                        System.IO.FileStream writeStream = new System.IO.FileStream(Path.Combine(queryFolder, documents[dCount].Filename), System.IO.FileMode.Create, System.IO.FileAccess.Write);
                        byte[] byteContents;
                        if (!documentContents.TryGetValue(documents[dCount].DocumentID, out byteContents))
                        {
                            throw new Exception("Document not found for the specified Document ID:" + documents[dCount].DocumentID);
                        }
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteContents))
                        {
                            ReadWriteStream(ms, writeStream, byteContents.Length);
                        }
                        if (Path.GetExtension(documents[dCount].Filename).ToLower() == ".exe" || Path.GetExtension(documents[dCount].Filename).ToLower() == ".sas")
                        {
                            // Set program file name
                            programFile = documents[dCount].Filename;
                        }
                    }
                    //swInputFiles.Close();

                    // Setup the process to run the batch file and pass in the output folder and the input file spec
                    SASStartInfo.FileName = @"""" + sasAutoExec + @"""";
                    SASStartInfo.Arguments = @"""" + Path.Combine(queryFolder, programFile) + @"""" + " " + @"""" + queryFolder + @"""";  // Two arguments: Program File, Output Path
                }

                //Dont show a command window
                SASStartInfo.CreateNoWindow = true;
                SASStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                //SASStartInfo.UseShellExecute = false;
                SASProcess.EnableRaisingEvents = true;
                SASProcess.StartInfo = SASStartInfo;

                //Create the File Watcher before starting the SAS because SAS may be running very fast and finishing before watcher comes into action.
                CreateDirectoryWatcher(queryFolder, IsAutoCmd);

                //start cmd.exe & the SAS process
                SASProcess.Start();

            }
            catch(Exception e)
            {
                status.Code = RequestStatus.StatusCode.Error;
                status.Message = e.Message;
                if (SASProcess != null)
                {
                    if (!SASProcess.HasExited)
                        SASProcess.Kill();
                    SASProcess.Dispose();
                    SASStartInfo = null;
                }
                throw e;
            }
        }

        public void Stop(string requestId, StopReason reason)
        {
        }

        public RequestStatus Status(string requestId)
        {
            return status;
        }

        public Document[] Response(string requestId)
        {
            BuildResponseDocuments();
            return responseDocuments;
        }

        public void AddResponseDocument(string requestId, string filePath)
        {
            BuildResponseDocuments();
            string mimeType = GetMimeType(filePath);
            Document document = new Document(responseDocuments.Length.ToString(), mimeType, filePath);
            IList<Document> responseDocumentList = responseDocuments.ToList<Document>();
            responseDocumentList.Add(document);
            responseDocuments = responseDocumentList.ToArray<Document>();
        }

        public void RemoveResponseDocument(string requestId, string documentId)
        {
            if (responseDocuments != null )
            {
                IList<Document> responseDocumentList = responseDocuments.ToList<Document>();
                foreach (Document doc in responseDocuments)
                {
                    if (doc.DocumentID == documentId)
                        responseDocumentList.Remove(doc);
                }
                responseDocuments = responseDocumentList.ToArray<Document>();
            }
        }

        public void ResponseDocument(string requestId, string documentId, out Stream contentStream, int maxSize)
        {
            contentStream = null;

            foreach(Document document in responseDocuments)
            {
                if (document.DocumentID == documentId)
                {
                    contentStream = new FileStream(document.Filename, FileMode.Open);
                    return;
                }
            }
        }

        public void Close(string requestId)
        {
        }

        public void PostProcess(string requestId)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Write the processed document content from IO stream
        /// </summary>
        /// <param name="readStream">read stream</param>
        /// <param name="writeStream">write stream</param>
        /// <param name="Length">length of the content</param>
        private void ReadWriteStream(System.IO.MemoryStream readStream, System.IO.FileStream writeStream, int Length)
        {
            try
            {
                Byte[] buffer = new Byte[Length];
                int bytesRead = readStream.Read(buffer, 0, Length);
                // write the required bytes
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = readStream.Read(buffer, 0, Length);
                }
                readStream.Close();
                writeStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CreateDirectoryWatcher(string path,bool isAutCmd)
        {
            //Create a new FileSystemWatcher.
            if (FileWatcher == null)
                FileWatcher = new FileSystemWatcher();
            else
                FileWatcher.EnableRaisingEvents = false;
            try
            {
                //Subscribe to the Created event.
                if (isAutCmd)
                {
                    FileWatcher.Created += new FileSystemEventHandler(DirectoryWatcher_FileCreated_OnAutoCmd);
                    //Set the filter to only catch TXT files.
                    FileWatcher.Filter = "result.txt";
                }
                else
                {
                    FileWatcher.Created += new FileSystemEventHandler(DirectoryWatcher_FileCreated_WithoutAutoCmd);
                    FileWatcher.Filter = "*.*";
                }

                FileWatcher.Path = path;

                //Enable the FileSystemWatcher events.
                FileWatcher.EnableRaisingEvents = true;
            }
            finally
            {
                if (this.status.Code != RequestStatus.StatusCode.InProgress)
                {
                    FileWatcher.EnableRaisingEvents = false;
                    FileWatcher = null;
                }
            }
        }

        private void DirectoryWatcher_FileCreated_OnAutoCmd(object sender, FileSystemEventArgs e)
        {
            bool done = false;
            try
            {
                // See if the result file is closed
                FileInfo fi = new FileInfo(e.Name);
                using (FileStream fs = fi.OpenWrite())
                {
                    done = true;
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                status.Code = RequestStatus.StatusCode.CompleteWithMessage;
                status.Message = ex.Message;
            }
            if (done)
            {
                status.Code = RequestStatus.StatusCode.Complete;
            }
        }

        private void DirectoryWatcher_FileCreated_WithoutAutoCmd(object sender, FileSystemEventArgs e)
        {
            /*
             * Extract the NetworkID, DataMartId and RequestID from the created file.
             * If it matches with the requestMetadata, then check if the file can be opened in write mode to ensure that the file has been fully created.
             * If the above two checks pass, update the status to completed.
             */
            string CurNetworkID = string.Empty;
            string CurDataMartID = string.Empty;
            string CurRequestID = string.Empty;

            string networkID = Settings.GetAsString("NetworkId", "");
            
            string strFile = e.Name;
            //extracting network Id
            CurNetworkID = strFile.Substring(0, strFile.IndexOf("_"));

            //extracting query Id
            strFile = strFile.Remove(0, strFile.IndexOf("_") + 1);
            CurRequestID = strFile.Substring(0, strFile.IndexOf("_"));

            //extracting datamart Id
            strFile = strFile.Remove(0, strFile.IndexOf("_") + 1);
            CurDataMartID = strFile.Substring(0, strFile.IndexOf("_"));

            if (CurNetworkID == networkID && CurDataMartID == this.requestMetadata.DataMartId && CurRequestID == this.requestId)
            {
                this.status.Code = RequestStatus.StatusCode.Complete;
            }
        }

        private void BuildResponseDocuments()
        {
            if (responseDocuments == null)
            {
                if (this.status.Code != RequestStatus.StatusCode.Error && this.status.Code != RequestStatus.StatusCode.InProgress)
                {
                    // Repository for discovered filenames
                    string networkID = Settings.GetAsString("NetworkId", "");
                    string outputFolder = Path.Combine(Settings.GetAsString("DMOutput", ""), networkID + "_" + requestMetadata.DataMartId) + "_" + requestId;
                    List<string> filenames = new List<string>();
                    string outputFiles = string.Empty;
                    outputFiles = "*.*";
                    filenames.AddRange(System.IO.Directory.GetFiles(outputFolder, outputFiles, System.IO.SearchOption.TopDirectoryOnly));

                    responseDocuments = new Document[filenames.Count];
                    for (int i = 0; i < filenames.Count; i++)
                    {
                        responseDocuments[i] = new Document(NewGuid(), "text/dataset", filenames[i], false,Convert.ToInt32(new FileInfo(filenames[i]).Length), "");
                    }
                }
            }
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        /// Gets a new sequential GUID that can be stored in a primary key
        /// </summary>
        /// <returns></returns>
        internal static Guid NewGuid()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }

        #endregion

    }
}
