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

        public DistributedRegressionModelAdapter() : base(new Guid("4C8A25DC-6816-4202-88F4-6D17E72A43BC"))
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

        public bool IsAnalysisCenter { get; private set; }

        string[] TriggerFileNames = new string[0];

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

            if (string.IsNullOrEmpty(RootMonitorFolder))
            {
                throw new Exception("The root monitoring folder path has not been set. Pleae confirm configuration settings.");
            }

            //The triggers have been modified to remove the monitoring for Job Stop file. (ASPE-510)
            TriggerFileNames = new[] { JobStartFilename, ExecutionCompleteFilename, JobFailFilename };
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

        IEnumerable<QueryComposerModelProcessor.DocumentEx> ProcessForDataPartner(DocumentWithStream[] requestDocs)
        {
            string outputfilesFolderPath = Path.Combine(RootMonitorFolder, RequestIdentifier, "msoc");
            string inputfilesFolderPath = Path.Combine(RootMonitorFolder, RequestIdentifier, "inputfiles");

            if (!Directory.Exists(inputfilesFolderPath))
            {
                Directory.CreateDirectory(inputfilesFolderPath);
            }

            if (!Directory.Exists(outputfilesFolderPath))
            {
                Directory.CreateDirectory(outputfilesFolderPath);
            }

            if (requestDocs.Any(d => string.Equals(Path.GetExtension(d.Document.Filename), ".zip", StringComparison.OrdinalIgnoreCase)))
            {
                //initial iteration with the setup package, extract if the "inputfiles" folder is empty
                if(!Directory.EnumerateFileSystemEntries(inputfilesFolderPath).Any())
                {
                    var initialSetupPackage = requestDocs.First(d => string.Equals(Path.GetExtension(d.Document.Filename), ".zip", StringComparison.OrdinalIgnoreCase));

                    using (ZipArchive archive = new ZipArchive(initialSetupPackage.Stream))
                    {
                        archive.ExtractToDirectory(Path.Combine(RootMonitorFolder, RequestIdentifier));
                    }
                }
            }
            else
            {
                foreach (var f in requestDocs)
                {
                    using(FileStream destination = File.Create(Path.Combine(inputfilesFolderPath, f.Document.Filename)))
                    {
                        f.Stream.CopyTo(destination);
                        destination.Flush();
                        destination.Close();
                    }
                }

                //Create the Execution Complete File only if files were transferred. (ASPE-497)
                //Updated to create the file only if new/updated files are downloaded. (ASPE-535)
                //Furthermore, the ExecutionComplete file is only created for non-zip files.
                if (requestDocs.Any())
                {
                    using (var fs = File.CreateText(Path.Combine(inputfilesFolderPath, ExecutionCompleteFilename)))
                    {
                        fs.Close();
                    }
                }
            }

            //The triggers have been modified to remove the monitoring for Job Stop file. (ASPE-510)
            if (!TriggerFileExists(outputfilesFolderPath, new[] { JobStartFilename, ExecutionCompleteFilename }))
            {
                FileSystemWatcher directoryWatcher = new FileSystemWatcher(outputfilesFolderPath, "*.*");
                directoryWatcher.Created += (object sender, FileSystemEventArgs e) =>
                {

                    //monitor for either the job started, execution successful, or job end trigger file
                    if (e.ChangeType == WatcherChangeTypes.Created && (string.Equals(e.Name, JobStartFilename, StringComparison.OrdinalIgnoreCase) || string.Equals(e.Name, ExecutionCompleteFilename, StringComparison.OrdinalIgnoreCase)))
                    {
                        directoryWatcher.EnableRaisingEvents = false;
                    }

                };

                directoryWatcher.EnableRaisingEvents = true;

                DateTime maxEndTime = DateTime.UtcNow.AddHours(Convert.ToDouble(_settings.GetAsString("MaxMonitorTime", "12")));
                while (directoryWatcher.EnableRaisingEvents)
                {
                    //wait for the watcher to stop => trigger file exists or have run out of time

                    if (DateTime.UtcNow > maxEndTime)
                    {
                        throw new Exception("The maximum time to wait for results has been exceeded. Please confirm the SAS application has been started.");
                    }

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            List<QueryComposerModelProcessor.DocumentEx> outputDocuments = new List<QueryComposerModelProcessor.DocumentEx>();

            //get all the files to upload that are listed in the filelist document
            outputDocuments = FilesToUpload(outputfilesFolderPath);

            //get the actual file bytes for the trigger files
            foreach(var document in outputDocuments)
            {
                if (IsTriggerFile(document.FileInfo.Name))
                {
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

            var trackingTableDocuments = outputDocuments.Where(d => d.Document.Kind == TrackingTableFileKind).Select(d => d.Document.Filename).ToArray();
            if (trackingTableDocuments.Length > 1)
            {
                throw new Exception("More than one tracking table document is included in the output files: " + string.Join(", ", trackingTableDocuments));
            }

            //delete the trigger file
            DeleteTriggerFiles(outputfilesFolderPath);

            return outputDocuments;
        }

        IEnumerable<QueryComposerModelProcessor.DocumentEx> ProcessForAnalysisCenter(DocumentWithStream[] requestDocs)
        {
            string outputFolderPath = Path.Combine(RootMonitorFolder, RequestIdentifier, "inputfiles");
            //if(Directory.Exists(outputFolderPath) == false)
            //{
            //    throw new Exception("The file path: \"" + outputFolderPath + "\" does not exist. Please make sure the output folder exists.");
            //}

            if (!Directory.Exists(outputFolderPath))
            {
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

            using(var sr = new StreamReader(manifestDocument.Stream))
            using(var jr = new Newtonsoft.Json.JsonTextReader(sr))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                manifestDocuments = serializer.Deserialize<DistributedRegressionAnalysisCenterManifestItem[]>(jr);
            } 

            //save the files to the specified folders based on the manifest file
            foreach(var document in manifestDocuments)
            {
                if (string.IsNullOrEmpty(document.DataPartnerIdentifier))
                {
                    //TODO:cannot extract file without knowing the folder, find out if this should fail everything

                    continue;
                }

                var doc = requestDocs.FirstOrDefault(d => d.ID == document.DocumentID);
                if(doc == null)
                {
                    continue;
                }

                string partnerDirectory = Path.Combine(RootMonitorFolder, RequestIdentifier, document.DataPartnerIdentifier);
                if (!Directory.Exists(partnerDirectory))
                {
                    Directory.CreateDirectory(partnerDirectory);
                }

                using(var writer = File.OpenWrite(Path.Combine(partnerDirectory, doc.Document.Filename)))
                {
                    doc.Stream.CopyTo(writer);
                    writer.Flush();
                }

                if(datapartnerFolders.ContainsKey(document.DataMartID) == false)
                {
                    datapartnerFolders.Add(document.DataMartID, partnerDirectory);
                }

            }

            //write the trigger files for each datapartner input folder
            foreach(var dpf in datapartnerFolders)
            {
                using (var fs = File.CreateText(Path.Combine(dpf.Value, ExecutionCompleteFilename)))
                {
                    fs.Close();
                }
            }            

            if (!TriggerFileExists(outputFolderPath, TriggerFileNames))
            {
                //wait for the trigger file in the "inputfiles" folder
                FileSystemWatcher directoryWatcher = new FileSystemWatcher(outputFolderPath, "*.*");
                directoryWatcher.Created += (object sender, FileSystemEventArgs e) =>
                {
                    //monitor for either the job started, execution successful, or job end trigger file
                    //The triggers have been modified to remove the monitoring for Job Stop file. (ASPE-510)
                    if (e.ChangeType == WatcherChangeTypes.Created && (string.Equals(e.Name, JobStartFilename, StringComparison.OrdinalIgnoreCase) || string.Equals(e.Name, ExecutionCompleteFilename, StringComparison.OrdinalIgnoreCase)))
                    {
                        directoryWatcher.EnableRaisingEvents = false;
                    }
                };

                directoryWatcher.EnableRaisingEvents = true;

                DateTime maxEndTime = DateTime.UtcNow.AddHours(Convert.ToDouble(_settings.GetAsString("MaxMonitorTime", "12")));
                while (directoryWatcher.EnableRaisingEvents)
                {
                    if (DateTime.UtcNow > maxEndTime)
                    {
                        throw new Exception("The maximum time to wait for results has been exceeded. Please confirm the SAS application has been started.");
                    }

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            //get all the files to upload that are listed in the filelist document
            List<QueryComposerModelProcessor.DocumentEx> outputDocuments = FilesToUpload(outputFolderPath);

            //get the actual file bytes for the trigger files
            foreach (var document in outputDocuments)
            {
                if (IsTriggerFile(document.FileInfo.Name))
                {
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

            var trackingTableDocuments = outputDocuments.Where(d => d.Document.Kind == TrackingTableFileKind).Select(d => d.Document.Filename).ToArray();
            if (trackingTableDocuments.Length > 1)
            {
                throw new Exception("More than one tracking table document is included in the output files: " + string.Join(", ", trackingTableDocuments));
            }

            DeleteTriggerFiles(outputFolderPath);

            return outputDocuments;
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
                }
            }
        }

        List<QueryComposerModelProcessor.DocumentEx> FilesToUpload(string directory)
        {
            List<QueryComposerModelProcessor.DocumentEx> documents = new List<QueryComposerModelProcessor.DocumentEx>();
            FileInfo fi;
            Guid documentID;

            string manifestFilepath = Path.Combine(directory, OutputManifestFilename);
            if (!File.Exists(manifestFilepath))
            {
                //If the manifest file is not present, then we're not going to return any files. (ASPE-497)

                //foreach(string filename in Directory.GetFiles(directory))
                //{
                //    fi = new FileInfo(filename);
                //    documentID = QueryComposerModelProcessor.NewGuid();
                //    documents.Add(new QueryComposerModelProcessor.DocumentEx {
                //        ID = documentID,
                //        Document = new Document(documentID, QueryComposerModelProcessor.GetMimeType(fi.Name), fi.Name, false, Convert.ToInt32(fi.Length), TranslateTriggerFilenameToDocumentKind(fi.Name)),
                //        FileInfo = fi                        
                //    });
                //}

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
                            fi = new FileInfo(Path.Combine(directory, filename));
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
}
