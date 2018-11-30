using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.QueryComposer;
using log4net;
using System.IO;
using Lpp.Dns.DataMart.Model.Settings;
using System.Threading;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DistributedRegression
{
    public class DistributedRegressionModelAdapter : ModelAdapter
    {
        static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string JobStartTriggerFileKind = "DistributedRegression.Initialized";
        const string JobStopTriggerFileKind = "DistributedRegression.Stop";
        const string JobFailTriggerFileKind = "DistributedRegression.Failed";
        const string OutputManifestFileKind = "DistributedRegression.FileList";
        const string TrackingTableFileKind = "DistributedRegression.TrackingTable";
        const string EnhancedEventLogFileKind = "DistributedRegression.AdapterEventLog";

        public DistributedRegressionModelAdapter(RequestMetadata requestMetadata) : base(new Guid("4C8A25DC-6816-4202-88F4-6D17E72A43BC"), requestMetadata)
        {
            IsAnalysisCenter = false;
        }        

        public override bool CanRunAndUpload { get { return true; } }


        public override bool CanAddResponseFiles { get { return true; } }


        public override bool CanUploadWithoutRun { get { return false; } }


        public override bool CanViewSQL { get { return false; } }

        public string RootMonitorFolder { get; private set; }
        public string JobStartFilename { get; private set; }
        public string ExecutionCompleteFilename { get; private set; }
        public string JobFailFilename { get; private set; }
        public string JobStopFilename { get; private set; }
        public string OutputManifestFilename { get; private set; }
        public string RequestIdentifier { get; set; }

        public decimal Frequency { get; set; }

        public bool IsAnalysisCenter { get; private set; }

        string[] TriggerFileNames = new string[0];

        readonly List<EventLogItem> EventLog = new List<EventLogItem>(20);

        public override void Initialize(IDictionary<string, object> settings)
        {
            base.Initialize(settings);

            RootMonitorFolder = settings.GetAsString("MonitorFolder", "");
            JobStartFilename = settings.GetAsString("SuccessfulInitializationFilename", "job_started.ok");
            ExecutionCompleteFilename = settings.GetAsString("ExecutionCompleteFilename", "files_done.ok");
            JobFailFilename = settings.GetAsString("ExecutionFailFilename", "job_fail.ok");
            JobStopFilename = settings.GetAsString("ExecutionStopFilename", "job_done.ok");
            OutputManifestFilename = settings.GetAsString("ManifestFilename", "file_list.csv");
            RequestIdentifier = ReplaceInvalidFilePathCharacters(settings.GetAsString("MSRequestID", ""), string.Empty);
            Frequency = Convert.ToDecimal(settings.GetAsString("MonitoringFrequency", "5"));

            if (string.IsNullOrEmpty(RootMonitorFolder))
            {
                throw new Exception("The root monitoring folder path has not been set. Pleae confirm configuration settings.");
            }

            //The triggers have been modified to remove the monitoring for Job Stop file. (ASPE-510)
            TriggerFileNames = new[] { JobStartFilename, ExecutionCompleteFilename, JobFailFilename };

            if (EventLog.Count > 0)
                EventLog.Clear();
        }

        public override DTO.QueryComposer.QueryComposerResponseDTO Execute(DTO.QueryComposer.QueryComposerRequestDTO request, bool viewSQL)
        {
            throw new NotSupportedException("This model adapter does not support processing a QueryComposerRequestDTO, execute the adapter by calling StartRequest.");
        }

        public override bool CanPostProcess(QueryComposerResponseDTO response, out string message)
        {
            message = string.Empty;
            return false;
        }

        public override void PostProcess(QueryComposerResponseDTO response)
        {
            //Stupid autoprocessing does not check to see if can postprocess, it just goes ahead and does it.
            //throw new NotSupportedException("This model adapter does not support post processing.");
        }


        public IEnumerable<QueryComposerModelProcessor.DocumentEx> StartRequest(Model.DocumentWithStream[] requestDocs)
        {

            if(requestDocs.Any(d => string.Equals(d.Document.Filename, "manifest.json", StringComparison.OrdinalIgnoreCase)))
            {
                IsAnalysisCenter = true;

                return ProcessForAnalysisCenter(requestDocs);

            }

            return ProcessForDataPartner(requestDocs);
        }

        void LogDebug(string message)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug($"{ RequestMetadata.DataMartName}, { RequestMetadata.MSRequestID }: { message }");
            }
        }

        void LogError(string message, Exception ex)
        {
            if (logger.IsErrorEnabled)
            {
                logger.Error($"{ RequestMetadata.DataMartName}, { RequestMetadata.MSRequestID }: { message }", ex);
            }
        }

        IEnumerable<QueryComposerModelProcessor.DocumentEx> ProcessForDataPartner(DocumentWithStream[] requestDocs)
        {
            
            string outputfilesFolderPath = Path.Combine(RootMonitorFolder, RequestIdentifier, "msoc");
            string inputfilesFolderPath = Path.Combine(RootMonitorFolder, RequestIdentifier, "inputfiles");

            LogDebug($"Checking to see if input directory exists: { inputfilesFolderPath }");
            if (!Directory.Exists(inputfilesFolderPath))
            {
                LogDebug($"Creating input directory: { inputfilesFolderPath }");
                Directory.CreateDirectory(inputfilesFolderPath);
            }

            LogDebug($"Checking to see if output directory exists: { outputfilesFolderPath }");
            if (!Directory.Exists(outputfilesFolderPath))
            {
                LogDebug($"Creating output directory: { outputfilesFolderPath }");
                Directory.CreateDirectory(outputfilesFolderPath);
            }

            if (requestDocs.Any(d => string.Equals(Path.GetExtension(d.Document.Filename), ".zip", StringComparison.OrdinalIgnoreCase)))
            {
                //initial iteration with the setup package, extract if the "inputfiles" folder is empty
                if (!Directory.EnumerateFileSystemEntries(inputfilesFolderPath).Any())
                {
                    var initialSetupPackage = requestDocs.First(d => string.Equals(Path.GetExtension(d.Document.Filename), ".zip", StringComparison.OrdinalIgnoreCase));

                    string zipPath = Path.Combine(RootMonitorFolder, RequestIdentifier, initialSetupPackage.Document.Filename);

                    LogDebug("Downloading initial payload zip file to: " + zipPath);

                    EventLog.Add(new EventLogItem(EventLogItemTypes.DownloadPayload, "Beginning download of initial payload zip file."));

                    using(FileStream outStream = File.Create(zipPath))
                    {
                        //do 4mb chunks
                        byte[] buffer = new byte[4194304];
                        int bytesReceived = 0;
                        int bytesRead = 0;
                        do
                        {
                            bytesReceived = initialSetupPackage.Stream.Read(buffer, 0, buffer.Length);
                            bytesRead += bytesReceived;

                            outStream.Write(buffer, 0, buffer.Length);

                        } while (bytesReceived > 0);
                        
                        outStream.Flush();
                        outStream.Close();
                    }

                    LogDebug("Extracting initial payload zip file " + initialSetupPackage.Document.Filename);
                    EventLog.Add(new EventLogItem(EventLogItemTypes.DownloadPayload, "Beginning of extracting initial payload from zip file."));

                    using (FileStream inStream = File.OpenRead(zipPath))
                    using (ZipArchive archive = new ZipArchive(inStream))
                    {   
                        archive.ExtractToDirectory(Path.Combine(RootMonitorFolder, RequestIdentifier));
                        
                        LogDebug("The following files were extracted from the initial payload zip file: " + string.Join(", ", archive.Entries.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToArray()));
                    }

                    EventLog.Add(new EventLogItem(EventLogItemTypes.DownloadPayload, "Zip file containing initial payload extracted."));

                    LogDebug("Deleting initial payload zip file: " + zipPath);
                    File.Delete(zipPath);
                    
                }
            }
            else
            {
                EventLog.Add(new EventLogItem(EventLogItemTypes.DownloadPayload, "Beginning download of input files."));

                LogDebug($"Beginning download of { requestDocs.Length } files to input folder: { inputfilesFolderPath }");
                foreach (var f in requestDocs)
                {
                    try
                    {
                        LogDebug("Downloading document: " + f.Document.Filename);
                        using (FileStream destination = File.Create(Path.Combine(inputfilesFolderPath, f.Document.Filename)))
                        {
                            f.Stream.CopyTo(destination);
                            destination.Flush();
                            destination.Close();
                        }
                        LogDebug("Successfully downloaded document: " + f.Document.Filename);
                    }catch(Exception ex)
                    {
                        LogError($"Error downloading file { f.Document.Filename } to the input files folder.", ex);
                        throw;
                    }
                }

                LogDebug($"Finished download of { requestDocs.Length } files to input folder: { inputfilesFolderPath }");
                EventLog.Add(new EventLogItem(EventLogItemTypes.DownloadPayload, "Input files successfully downloaded."));

                //Create the Execution Complete File only if files were transferred. (ASPE-497)
                //Updated to create the file only if new/updated files are downloaded. (ASPE-535)
                //Furthermore, the ExecutionComplete file is only created for non-zip files.
                if (requestDocs.Any())
                {
                    try
                    {
                        LogDebug("Creating Execution Complete trigger file: " + ExecutionCompleteFilename);
                        using (var fs = File.CreateText(Path.Combine(inputfilesFolderPath, ExecutionCompleteFilename)))
                        {
                            fs.Close();
                        }
                        LogDebug("Successfully created Execution Complete trigger file: " + ExecutionCompleteFilename);
                        EventLog.Add(new EventLogItem(EventLogItemTypes.TriggerFileCreated, "Execution Complete trigger file created by adapter in the input folder."));
                    }catch(Exception ex)
                    {
                        LogError($"Error creating Execution Complete trigger file: { ExecutionCompleteFilename } in the input files folder.", ex);
                        throw;
                    }
                }
            }

            LogDebug("Checking to see if the Start Job or Execution Complete trigger files exist.");
            //The triggers have been modified to remove the monitoring for Job Stop file. (ASPE-510)
            if (!TriggerFileExists(outputfilesFolderPath, new[] { JobStartFilename, ExecutionCompleteFilename }))
            {
                DirectoryWatcher(outputfilesFolderPath);
            }

            List<QueryComposerModelProcessor.DocumentEx> outputDocuments = new List<QueryComposerModelProcessor.DocumentEx>();

            //get all the files to upload that are listed in the filelist document
            outputDocuments = FilesToUpload(outputfilesFolderPath);

            if (outputDocuments.Any())
            {
                LogDebug("Manifest File list the following files for upload: " + String.Join(" ,", outputDocuments.Select(x => String.Format("'{0}'", x.FileInfo.Name))).Remove(0, 1));
            }

            //get the actual file bytes for the trigger files
            foreach (var document in outputDocuments)
            {
                if (IsTriggerFile(document.FileInfo.Name))
                {
                    DateTime maxFileEndTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_settings.GetAsString("MaxReadWaitTime", "5")));
                    while (IsFileLocked(document.FileInfo))
                    {
                        if (DateTime.UtcNow > maxFileEndTime)
                        {
                            throw new Exception("The maximum time to wait to read the trigger file has been exceeded and it is still locked.");
                        }
                    }

                    //copy the bytes of the trigger file now (likely always zero) since the physical file will be deleted prior to upload.
                    byte[] buffer = new byte[document.FileInfo.Length];
                    using (var fs = document.FileInfo.OpenRead())
                    {
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Close();
                    }

                    document.Content = buffer;
                    document.FileInfo = null;
                }
            }

            LogOutputFilesCreated(outputDocuments);

            var trackingTableDocuments = outputDocuments.Where(d => d.Document.Kind == TrackingTableFileKind).Select(d => d.Document.Filename).ToArray();
            if (trackingTableDocuments.Length > 1)
            {
                throw new Exception("More than one tracking table document is included in the output files: " + string.Join(", ", trackingTableDocuments));
            }

            LogDebug("Deleting trigger files from: " + outputfilesFolderPath);
            DeleteTriggerFiles(outputfilesFolderPath);

            return outputDocuments;
        }

        private void LogOutputFilesCreated(List<QueryComposerModelProcessor.DocumentEx> outputDocuments)
        {
            EventLog.Add(new EventLogItem(EventLogItemTypes.OutputFilesCreated, "Output files created; ready for upload."));

            var content = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(EventLog, Newtonsoft.Json.Formatting.None));

            Guid documentID = QueryComposerModelProcessor.NewGuid();
            outputDocuments.Add(new QueryComposerModelProcessor.DocumentEx {
                ID = documentID,
                Document = new Document(documentID, "application/json", "AdapterEvents.log", false, content.Length, EnhancedEventLogFileKind),
                Content = content
            });
        }

        IEnumerable<QueryComposerModelProcessor.DocumentEx> ProcessForAnalysisCenter(DocumentWithStream[] requestDocs)
        {
            string outputFolderPath = Path.Combine(RootMonitorFolder, RequestIdentifier, "inputfiles");

            LogDebug($"Checking to see if output directory exists: { outputFolderPath }");
            if (!Directory.Exists(outputFolderPath))
            {
                LogDebug($"Creating output directory: { outputFolderPath }");
                Directory.CreateDirectory(outputFolderPath);
            }
            
            //look for the manifest.json file
            var manifestDocument = requestDocs.FirstOrDefault(d => string.Equals(d.Document.Filename, "manifest.json", StringComparison.OrdinalIgnoreCase) && d.Document.Kind == Lpp.Dns.DTO.Enums.DocumentKind.SystemGeneratedNoLog);
            if(manifestDocument == null)
            {
                throw new Exception("Missing Analysis Center manifest file, unable to determine location to extract input files.");
            }

            DistributedRegressionAnalysisCenterManifestItem[] manifestDocuments = null;

            Dictionary<Guid, string> datapartnerFolders = new Dictionary<Guid, string>();

            LogDebug("Determining documents to download for DataPartners from API.");

            using (var sr = new StreamReader(manifestDocument.Stream))
            using(var jr = new Newtonsoft.Json.JsonTextReader(sr))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                manifestDocuments = serializer.Deserialize<DistributedRegressionAnalysisCenterManifestItem[]>(jr);
            }

            LogDebug("Starting download of files to DataPartner directories.");

            EventLog.Add(new EventLogItem(EventLogItemTypes.DownloadPayload, "Initiating DataPartner input files download to Analysis Center."));

            //save the files to the specified folders based on the manifest file
            foreach (var document in manifestDocuments)
            {
                if (string.IsNullOrEmpty(document.DataPartnerIdentifier))
                {
                    //TODO:cannot extract file without knowing the folder, find out if this should fail everything
                    LogDebug($"DataPartner Identifier is null or empty for document ID: { document.DocumentID }, skipping download.");
                    continue;
                }

                var doc = requestDocs.FirstOrDefault(d => d.ID == document.DocumentID);
                if(doc == null)
                {
                    LogDebug($"Document not found in request documents for document ID: { document.DocumentID }, skipping download.");
                    continue;
                }

                string partnerDirectory = Path.Combine(RootMonitorFolder, RequestIdentifier, document.DataPartnerIdentifier);
                if (!Directory.Exists(partnerDirectory))
                {
                    LogDebug($"Creating directory for DataPartner: {document.DataPartnerIdentifier}");
                    Directory.CreateDirectory(partnerDirectory);
                }

                try
                {
                    using (var writer = File.OpenWrite(Path.Combine(partnerDirectory, doc.Document.Filename)))
                    {
                        doc.Stream.CopyTo(writer);
                        writer.Flush();
                        LogDebug($"Successfully downloaded file \"{ doc.Document.Filename }\" for DataPartner: { document.DataPartnerIdentifier }");
                    }
                }catch(Exception ex)
                {
                    LogError($"Error downloading file \"{ doc.Document.Filename }\" to DataPartner folder: { partnerDirectory }.", ex);
                    throw;
                }

                if(datapartnerFolders.ContainsKey(document.DataMartID) == false)
                {
                    datapartnerFolders.Add(document.DataMartID, partnerDirectory);
                }

            }

            LogDebug("All DataPartner input files successfully downloaded to Analysis Center.");
            EventLog.Add(new EventLogItem(EventLogItemTypes.DownloadPayload, "DataPartner input files successfully downloaded to Analysis Center."));

            //write the trigger files for each datapartner input folder
            foreach (var dpf in datapartnerFolders)
            {
                try
                {
                    LogDebug("Creating Execution Complete trigger file \"" + ExecutionCompleteFilename + "\" in DataPartner folder: " + dpf.Value);

                    using (var fs = File.CreateText(Path.Combine(dpf.Value, ExecutionCompleteFilename)))
                    {
                        fs.Close();
                    }

                    LogDebug("Successfully created Execution Complete trigger file \"" + ExecutionCompleteFilename + "\" in DataPartner folder: " + dpf.Value);
                }catch(Exception ex)
                {
                    LogError($"Error creating Execution Complete trigger file \"{ ExecutionCompleteFilename }\" to DataPartner folder: { dpf.Value }.", ex);
                    throw;
                }
            }

            EventLog.Add(new EventLogItem(EventLogItemTypes.TriggerFileCreated, "Execution Complete trigger file created by adapter for each Data Partner in their input folder at the Analysis Center."));


            LogDebug("Checking to see if the Start Job or Execution Complete Trigger files are there.");

            if (!TriggerFileExists(outputFolderPath, TriggerFileNames))
            {
                DirectoryWatcher(outputFolderPath);
            }

            //get all the files to upload that are listed in the filelist document
            List<QueryComposerModelProcessor.DocumentEx> outputDocuments = FilesToUpload(outputFolderPath);
            LogDebug("Manifest File list the following Files for upload: " + string.Join(", ", outputDocuments.Select(x => string.Format("\"{0}\"", x.FileInfo.Name)).ToArray()));
            //get the actual file bytes for the trigger files
            foreach (var document in outputDocuments)
            {
                if (IsTriggerFile(document.FileInfo.Name))
                {
                    DateTime maxFileEndTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_settings.GetAsString("MaxReadWaitTime", "5")));
                    while (IsFileLocked(document.FileInfo))
                    {
                        if (DateTime.UtcNow > maxFileEndTime)
                        {
                            throw new Exception("The maximum time to wait to read the trigger file has been exceeded and it is still locked.");
                        }
                    }

                    //copy the bytes of the trigger file now (likely always zero) since the physical file will be deleted prior to upload.
                    byte[] buffer = new byte[document.FileInfo.Length];
                    using (var fs = document.FileInfo.OpenRead())
                    {
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Close();
                    }

                    document.Content = buffer;
                    document.FileInfo = null;
                }
            }

            LogOutputFilesCreated(outputDocuments);

            var trackingTableDocuments = outputDocuments.Where(d => d.Document.Kind == TrackingTableFileKind).Select(d => d.Document.Filename).ToArray();
            if (trackingTableDocuments.Length > 1)
            {
                throw new Exception("More than one tracking table document is included in the output files: " + string.Join(", ", trackingTableDocuments));
            }

            LogDebug("Deleting trigger files from: " + outputFolderPath);
            DeleteTriggerFiles(outputFolderPath);

            return outputDocuments;
        }

        private void DirectoryWatcher(string outputFolderPath)
        {
            DateTime maxEndTime = DateTime.UtcNow.AddHours(Convert.ToDouble(_settings.GetAsString("MaxMonitorTime", "12")));
            logger.Info(string.Format("Beginning monitoring of directory {0} for trigger files.", outputFolderPath));
            while (true)
            {
                var files = Directory.GetFiles(outputFolderPath);

                foreach (var file in files)
                {
                    if (string.Equals(Path.GetFileName(file), JobStartFilename, StringComparison.OrdinalIgnoreCase))
                    {
                        LogDebug(JobStartFilename + " Job Start trigger file found in folder: " + outputFolderPath);
                        EventLog.Add(new EventLogItem(EventLogItemTypes.TriggerFileCreated, "Job Start trigger file created."));
                        return;
                    }
                    else if (string.Equals(Path.GetFileName(file), ExecutionCompleteFilename, StringComparison.OrdinalIgnoreCase))
                    {
                        LogDebug(ExecutionCompleteFilename + " Execution Complete trigger file found in folder: " + outputFolderPath);
                        EventLog.Add(new EventLogItem(EventLogItemTypes.TriggerFileCreated, "Execution Complete trigger file created."));
                        return;
                    }
                    else if (DateTime.UtcNow > maxEndTime)
                    {
                        throw new Exception("The maximum time to wait for results has been exceeded. Please confirm the SAS application has been started.");
                    }
                }
                Thread.Sleep(Convert.ToInt32(Frequency * 1000));
            }
        }

        public override void Dispose()
        {

        }

        bool TriggerFileExists(string folderPath, string[] triggerFiles)
        {
            string[] files = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly) ?? Array.Empty<string>();
            return triggerFiles.Any(t => files.Any(f => string.Equals(t, Path.GetFileName(f), StringComparison.OrdinalIgnoreCase)));
        }

        bool IsTriggerFile(string file)
        {
            return TriggerFileNames.Any(t => string.Equals(Path.GetFileName(file), t, StringComparison.OrdinalIgnoreCase));
        }

        void DeleteTriggerFiles(string directory)
        {
            foreach (var filename in TriggerFileNames.Where(x => x != JobFailFilename))
            {
                string triggerFilepath = Path.Combine(directory, filename);
                if (File.Exists(triggerFilepath))
                {
                    File.Delete(triggerFilepath);
                    LogDebug("Deleted trigger file: " + triggerFilepath);
                }
            }
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        List<QueryComposerModelProcessor.DocumentEx> FilesToUpload(string directory)
        {
            List<QueryComposerModelProcessor.DocumentEx> documents = new List<QueryComposerModelProcessor.DocumentEx>();
            FileInfo fi;
            Guid documentID;

            string manifestFilepath = Path.Combine(directory, OutputManifestFilename);
            LogDebug("Starting read of manifest file: " + manifestFilepath);
            if (!File.Exists(manifestFilepath))
            {
                LogDebug($"Manifest file cannot be read or file doesn't exists: { manifestFilepath }");
                return documents;
            }


            //add the output manifest
            fi = new FileInfo(manifestFilepath);
            documentID = QueryComposerModelProcessor.NewGuid();
            documents.Add(new QueryComposerModelProcessor.DocumentEx {
                ID = documentID,
                Document =  new Document(documentID, QueryComposerModelProcessor.GetMimeType(fi.Name), fi.Name, false, Convert.ToInt32(fi.Length), TranslateTriggerFilenameToDocumentKind(fi.Name)),
                FileInfo = fi
            });

            DateTime maxEndTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_settings.GetAsString("MaxReadWaitTime", "5")));
            while (IsFileLocked(fi))
            {
                if (DateTime.UtcNow > maxEndTime)
                {
                    throw new Exception("The maximum time to wait to read the manifest file has been exceeded and it is still locked.");
                }
            }
            using (var stream = File.OpenText(manifestFilepath))
            {
                //read the header line
                stream.ReadLine();

                string line, filename;
                while (!stream.EndOfStream)
                {
                    line = stream.ReadLine();
                    string[] split = line.Split(',');
                    if (split.Length > 0)
                    {
                        //first value will be the filename relative to the folder
                        filename = split[0].Trim();
                        if (!string.IsNullOrEmpty(filename))
                        {
                            if(documents.Any(d => d.Document.Filename == filename))
                            {
                                continue;
                            }

                            fi = new FileInfo(Path.Combine(directory, filename));

                            if (!fi.Exists)
                            {
                                throw new FileNotFoundException("File not found in " + directory, filename);
                            }

                            documentID = QueryComposerModelProcessor.NewGuid();
                            var documentEx = new QueryComposerModelProcessor.DocumentEx {
                                ID = documentID,
                                Document = new Document(documentID, QueryComposerModelProcessor.GetMimeType(fi.Name), fi.Name, false, Convert.ToInt32(fi.Length), TranslateTriggerFilenameToDocumentKind(fi.Name)),
                                FileInfo = fi
                            };

                            if(split[1].Trim() == "0")
                            {
                                documentEx.Document.Kind = TrackingTableFileKind;
                            }

                            documents.Add(documentEx);
                        }
                    }
                }
            }

            return documents;
        }
        
        string TranslateTriggerFilenameToDocumentKind(string filename)
        {
            if (string.Equals(JobStartFilename, filename, StringComparison.OrdinalIgnoreCase))
            {
                return JobStartTriggerFileKind;
            }

            if (string.Equals(JobStopFilename, filename, StringComparison.OrdinalIgnoreCase))
            {
                return JobStopTriggerFileKind;
            }

            if (string.Equals(JobFailFilename, filename, StringComparison.OrdinalIgnoreCase))
            {
                return JobFailTriggerFileKind;
            }

            if (string.Equals(OutputManifestFilename, filename, StringComparison.OrdinalIgnoreCase))
            {
                return OutputManifestFileKind;
            }

            return null;
        }

        static string ReplaceInvalidFilePathCharacters(string path, string replaceWith)
        {
            var invalidCharacters = Path.GetInvalidPathChars();
            StringBuilder sb = new StringBuilder();

            foreach(char c in path)
            {
                if (invalidCharacters.Contains(c))
                    sb.Append(replaceWith);
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }

    }

    internal struct EventLogItem
    {
        public readonly DateTime Timestamp;
        public readonly string Type;
        public readonly string Description;

        public EventLogItem(string type, string description) : this(DateTime.UtcNow, type, description)
        {
        }

        [Newtonsoft.Json.JsonConstructor]
        public EventLogItem(DateTime timestamp, string type, string description)
        {
            Timestamp = timestamp;
            Type = type;
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }

    internal static class EventLogItemTypes
    {
        public const string DownloadPayload = "Download Input Files";
        public const string TriggerFileCreated = "Trigger File Created";
        public const string OutputFilesCreated = "Output Files Created";
    }
}
