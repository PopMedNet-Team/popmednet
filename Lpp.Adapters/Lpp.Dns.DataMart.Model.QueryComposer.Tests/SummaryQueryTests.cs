using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class SummaryQueryTests
    {
        const string ResourceFolder = "../Resources/SummaryQuery";
        static readonly string ConnectionString;
        static readonly log4net.ILog Logger;

        static SummaryQueryTests()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SummaryQuery"].ConnectionString;
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(SummaryQueryTests));
        }

        /// <summary>
        /// Simple INC: ICD9 Diagnosis 3 Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_INC_ICD9_Diagnosis_3Digit()
        {

            var response = RunIncRequest("INC_ICD9_Diagnosis_3Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// Simple INC: ICD9 Diagnosis 3 Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_INC_Pharmacy_Dispensings_By_Drug_Class()
        {
            
            var response = RunIncRequest("INC_Pharmacy_Dispensings_By_Drug_Class.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// Inci: Pharmacy Dispensings by Generic Name Test
        /// </summary>
        [TestMethod]
        public void Simple_INC_Pharmacy_Dispensings_By_Drug_Name()
        {

            var response = RunIncRequest("INC_Pharmacy_Dispensings_By_Drug_Name.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// MFU_HCPCS Procedures Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_HCPCS_Procedures()
        {
            
            var response = RunMFURequest("MFU_HCPCS Procedures.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
         /// <summary>
        /// MFU_ICD9_Diagnosis_3Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_ICD9_Diagnosis_3Digit()
        {

            var response = RunMFURequest("MFU_ICD9_Diagnosis_3Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
         /// <summary>
        /// MFU_ICD9_Diagnosis_4Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_ICD9_Diagnosis_4Digit()
        {

            var response = RunMFURequest("MFU_ICD9_Diagnosis_4Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// MFU_ICD9_Diagnosis_5Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_ICD9_Diagnosis_5Digit()
        {

            var response = RunMFURequest("MFU_ICD9_Diagnosis_5Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// MFU_ICD9_Procedure_3Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_ICD9_Procedure_3Digit()
        {

            var response = RunMFURequest("MFU_ICD9_Procedure_3Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// MFU_ICD9_Procedure_4Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_ICD9_Procedure_4Digit()
        {

            var response = RunMFURequest("MFU_ICD9_Procedure_4Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
         /// <summary>
        /// Simple MFU: Pharmacy Dispensings by Drug Class Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_Pharmacy_Dispensings_By_Drug_Class()
        {

            var response = RunMFURequest("MFU_Pharmacy_Dispensings_By_Drug_Class.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
         /// <summary>
        /// MFU: Pharmacy Dispensings by Generic Name Test
        /// </summary>
        [TestMethod]
        public void Simple_MFU_Pharmacy_Dispensings_By_Drug_Name()
        {

            var response = RunMFURequest("MFU_Pharmacy_Dispensings_By_Drug_Name.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// Prev: Enrollment Test
        /// </summary>
        [TestMethod]
        public void Simple_Prev_Enrollment()
        {

            var response = RunPrevRequest("Prev_Enrollment.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// Prev_HCPCS_Procedures Tests
        /// </summary>
        [TestMethod]
        public void Simple_Prev_HCPCS_Procedures()
        {

            var response = RunPrevRequest("Prev_HCPCS_Procedures.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
       
        /// <summary>
        ///  Prev_ICD9_Diagnosis_3Digit Tests
        /// </summary>
        [TestMethod]
        public void Simple_Prev_ICD9_Diagnosis_3Digit()
        {

            var response = RunPrevRequest("Prev_ICD9_Diagnosis_3Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        ///  Prev_ICD9_Diagnosis_4Digit Tests
        /// </summary>
        [TestMethod]
        public void Simple_Prev_ICD9_Diagnosis_4Digit()
        {

            var response = RunPrevRequest("Prev_ICD9_Diagnosis_4Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        ///  Prev_ICD9_Diagnosis_5Digit Tests
        /// </summary>
        [TestMethod]
        public void Simple_Prev_ICD9_Diagnosis_5Digit()
        {

            var response = RunPrevRequest("Prev_ICD9_Diagnosis_5Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// Prev_ICD9_Procedure_3Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_Prev_ICD9_Procedure_3Digit()
        {

            var response = RunPrevRequest("Prev_ICD9_Procedure_3Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// Prev_ICD9_Procedure_3Digit Test
        /// </summary>
        [TestMethod]
        public void Simple_Prev_ICD9_Procedure_4Digit()
        {

            var response = RunPrevRequest("Prev_ICD9_Procedure_4Digit.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
         /// <summary>
        /// Prev_Drug_Class Test
        /// </summary>
        [TestMethod]
        public void Simple_Prev_Pharmacy_Dispensings_By_Drug_Class()
        {

            var response = RunPrevRequest("Prev_Pharmacy_Dispensings_By_Drug_Class.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }

        [TestMethod]
        public void Metadata_Refresh_Dates_Response()
        {

            var response = RunMetadataRefreshRequest("MetdataRefrshDates.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void Metadata_Refresh_Dates_Documents()
        {
            QueryComposerModelProcessor.DocumentEx[] docs;
            var request = LoadRequest("MetdataRefrshDates.json");
            using (var adapter = Helper.CreateMetadataRefreshAdapter(ConnectionString))
            {
                adapter.Execute(request, false);
                docs = adapter.OutputDocuments();

                foreach (var doc in docs)
                {
                    using (var fileStream = File.Create(doc.Document.Filename))
                    {
                        fileStream.Write(doc.Content, 0, doc.Content.Length);
                        fileStream.Flush();
                    }
                }
            }
        }

        [TestMethod]
        public void PMNDEV5164()
        {

            var response = RunIncRequest("PMNDEV-5164.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void PMNDEV5186()
        {

            var response = RunPrevRequest("PMNDEV-5186.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void PMNDEV5194()
        {

            var response = RunPrevRequest("PMNDEV-5194.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void PMNDEV7081()
        {

            var response = RunPrevRequest("PMNDEV-7081.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void PMNDEV7082()
        {

            var response = RunPrevRequest("PMNDEV-7082.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void PMNDEV7083()
        {

            var response = RunPrevRequest("PMNDEV-7083.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunIncRequest(string requestJsonFilepath)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreateINCSummaryModelAdapterAdapter(ConnectionString))
            {
                return adapter.Execute(request, false);
            }
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunMFURequest(string requestJsonFilepath)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreateMFUSummaryModelAdapterAdapter(ConnectionString))
            {
                return adapter.Execute(request, false);
            }
        }
        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunPrevRequest(string requestJsonFilepath)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreatePrevSummaryModelAdapterAdapter(ConnectionString))
            {
                return adapter.Execute(request, false);
            }
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunMetadataRefreshRequest(string requestJsonFilepath)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreateMetadataRefreshAdapter(ConnectionString))
            {
                return adapter.Execute(request, false);
            }
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO LoadRequest(string filename, string folder = ResourceFolder)
        {
            string filepath = System.IO.Path.Combine(folder, filename);
            var json = System.IO.File.ReadAllText(filepath);
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
            Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(json, jsonSettings);

            return request;
        }

        static string SerializeJsonToString(Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO response)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return Newtonsoft.Json.JsonConvert.SerializeObject(response, settings);
        }
    }
}
