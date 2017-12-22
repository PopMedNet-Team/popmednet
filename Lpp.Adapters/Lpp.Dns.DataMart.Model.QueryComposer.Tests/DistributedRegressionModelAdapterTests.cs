using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class DistributedRegressionModelAdapterTests
    {
        const string ResourceFolder = "../Resources/DistributedRegression";
        const string MonitorFolderDataPartner = "./DistributedRegression/DP";
        const string MonitorFolderAnalysisCenter = "./DistributedRegression/AC";
        const string DP_InitialSetupPackage = "DR_initial_setup.zip";
        const string SuccessfulInitializationFilename = "job_start.ok";
        const string ExecutionCompleteFilename = "files_done.ok";
        const string ExecutionFailFilename = "job_fail.ok";
        const string ExecutionStopFilename = "job_done.ok";
        const string ManifestFilename = "files_list.csv";

        Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DistributedRegression.DistributedRegressionModelAdapter _adapter = null;

        [ClassInitialize]
        public static void TestClassInitialize(TestContext context)
        {

            if (!System.IO.Directory.Exists(MonitorFolderDataPartner))
            {
                System.IO.Directory.CreateDirectory(MonitorFolderDataPartner);
            }

            if (!System.IO.Directory.Exists(MonitorFolderAnalysisCenter))
            {
                System.IO.Directory.CreateDirectory(MonitorFolderAnalysisCenter);
            }
        }

        [ClassCleanup]
        public static void TestClassCleanup()
        {
            CleanoutMonitorFolders();
        }

        static void CleanoutMonitorFolders()
        {
            foreach (var f in Directory.GetFiles(MonitorFolderDataPartner, "*.*", SearchOption.AllDirectories).Union(Directory.GetFiles(MonitorFolderAnalysisCenter, "*.*", SearchOption.AllDirectories)))
            {
                File.Delete(f);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _adapter = new Adapters.DistributedRegression.DistributedRegressionModelAdapter();

            CleanoutMonitorFolders();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            if(_adapter != null)
            {
                _adapter.Dispose();
            }
        }

        IDictionary<string,object> CreateSettings(string monitorFolder)
        {
            return new Dictionary<string, object> {
                { "MonitorFolder", monitorFolder },
                { "SuccessfulInitializationFilename", SuccessfulInitializationFilename},
                { "ExecutionCompleteFilename", ExecutionCompleteFilename},
                { "ExecutionFailFilename", ExecutionFailFilename},
                { "ExecutionStopFilename", ExecutionStopFilename},
                { "ManifestFilename", ManifestFilename},
                { "MaxMonitorTime", "12"},
                { "MaxReadWaitTime", "5"}
            };
        }

        [TestMethod]
        public void DataPartnerSetup()
        {
            _adapter.Initialize(CreateSettings(MonitorFolderDataPartner));

            var timer = new System.Timers.Timer(TimeSpan.FromSeconds(2).TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                            {
                                if (Directory.Exists(Path.Combine(MonitorFolderDataPartner, "msoc")))
                                {
                                    timer.Stop();

                                    using (var fs = File.Create(Path.Combine(MonitorFolderDataPartner, "msoc", SuccessfulInitializationFilename)))
                                    {
                                        fs.Close();
                                    }
                                    
                                }
                            };

            Guid initialPackageDocumentID = Guid.NewGuid();
            var document = new Document(initialPackageDocumentID.ToString("D"), "application/x-zip-compressed", DP_InitialSetupPackage);
            var documentFilepath = Path.Combine(ResourceFolder, document.Filename);
            var setupPackage = new DocumentWithStream(initialPackageDocumentID, document, File.Open(documentFilepath, FileMode.Open));

            timer.Start();
            var responseDocuments = _adapter.StartRequest(new[] { setupPackage });

            Assert.IsTrue(responseDocuments.Any(d => string.Equals(d.Document.Filename, SuccessfulInitializationFilename, StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void AnalysisCenterWithNewPayloadFromDataPartners()
        {
            _adapter.Initialize(CreateSettings(MonitorFolderAnalysisCenter));


            //remove all the directories
            var directories = Directory.GetDirectories(MonitorFolderAnalysisCenter);
            foreach (var dir in directories)
            {
                Directory.Delete(dir, true);
            }

            //create the inputfiles folder
            string inputfilesFolderPath = Path.Combine(MonitorFolderAnalysisCenter, "inputfiles");
            if (!Directory.Exists(inputfilesFolderPath))
            {
                Directory.CreateDirectory(inputfilesFolderPath);
            }

            //payload will consist of input files from each data partner, need to be extracted to partner specific folders and then monitor the inputfiles folder

            List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem> manifestItems = new List<DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem>();
            List<DocumentWithStream> datapartnerDocuments = new List<DocumentWithStream>();

            for(int i =1; i <= 5; i++)
            {
                Guid documentID = Guid.NewGuid();
                datapartnerDocuments.Add(new DocumentWithStream(documentID, new Document(documentID.ToString("D"), "text/plain", "payload_document.txt"), new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Hello!"))));

                manifestItems.Add(new DTO.QueryComposer.DistributedRegressionAnalysisCenterManifestItem {
                    DataMart = "Data Partner " + i,
                    DataMartID = Guid.NewGuid(),
                    DataPartnerIdentifier = "msoc" + i,
                    DocumentID = documentID,
                    RequestDataMartID = Guid.NewGuid(),
                    ResponseID = Guid.NewGuid(),
                    RevisionSetID = documentID
                });
            }

            //create the analysis center manifest
            MemoryStream manifestStream;
            using (var ms = new MemoryStream())
            using (var sr = new StreamWriter(ms))
            using (var jr = new Newtonsoft.Json.JsonTextWriter(sr))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(jr, manifestItems);
                jr.Flush();

                manifestStream = new MemoryStream(ms.ToArray());
            }
            

            Guid manifestID = Guid.NewGuid();
            datapartnerDocuments.Add(new DocumentWithStream(manifestID, new Document(manifestID, "application/json", "manifest.json", false, Convert.ToInt32(manifestStream.Length), Lpp.Dns.DTO.Enums.DocumentKind.SystemGeneratedNoLog), manifestStream));

            var timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                if (Directory.Exists(Path.Combine(MonitorFolderAnalysisCenter, "inputfiles")))
                {
                    timer.Stop();

                    using (var fs = File.Create(Path.Combine(MonitorFolderAnalysisCenter, "inputfiles", ExecutionCompleteFilename)))
                    {
                        fs.Close();
                    }

                }
            };
            timer.Start();

            var responseDocuments = _adapter.StartRequest(datapartnerDocuments.ToArray());

            for (int i = 1; i <= 5; i++)
            {
                Assert.IsTrue(Directory.Exists(Path.Combine(MonitorFolderAnalysisCenter, "msoc" + i)));
            }

        }

        [TestMethod]
        public void DataPartnerPayloadFromAnalysisCenter()
        {
            //the analysis center provides multiple documents that are distributed to each data partner, the files are extracted into the "inputfiles" folder of the data partner
            //the SAS app will drop an output files manifest and documents into the "msoc" folder, the files listed in the manifest file will be uploaded when the trigger file exists

            _adapter.Initialize(CreateSettings(MonitorFolderDataPartner));

            string outputFolder = Path.Combine(MonitorFolderDataPartner, "msoc");
            var timer = new System.Timers.Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                if (Directory.Exists(outputFolder))
                {
                    timer.Stop();

                    //create some output documents
                    string fname;
                    for (int i = 1; i <= 5; i++)
                    {
                        fname = Path.Combine(outputFolder, "output_file_" + i + ".txt");
                        using (var fs = File.CreateText(fname))
                        {
                            fs.WriteLine("Data partner document " + i + ".");
                            fs.Close();
                        }
                    }

                    //create a manifest csv listing the documents to upload
                    using(var fs = File.CreateText(Path.Combine(outputFolder, ManifestFilename)))
                    {
                        fs.WriteLine("file_nm,transfer_to_site_in");
                        fs.WriteLine("output_file_1.txt,1");
                        fs.WriteLine("output_file_2.txt,0");
                        fs.WriteLine("output_file_3.txt,1");
                        fs.WriteLine("output_file_4.txt,0");
                        fs.WriteLine("output_file_5.txt,1");
                        fs.Close();
                    }

                    //create the execution complete trigger file
                    using (var fs = File.Create(Path.Combine(outputFolder, ExecutionCompleteFilename)))
                    {
                        fs.Close();
                    }

                }
            };

            List<DocumentWithStream> datapartnerDocuments = new List<DocumentWithStream>();

            for (int i = 1; i <= 5; i++)
            {
                Guid documentID = Guid.NewGuid();
                datapartnerDocuments.Add(new DocumentWithStream(documentID, new Document(documentID.ToString("D"), "text/plain", "payload_document_" + i + ".txt"), new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Hello!"))));
            }

            timer.Start();
            var responseDocuments = _adapter.StartRequest(datapartnerDocuments.ToArray());

            Console.WriteLine("done");

            //assert that only the documents listed in the file manifest got uploaded

        }
        
    }
}
