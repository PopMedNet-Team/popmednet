using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

            var response = RunIncRequestForSingleResult("INC_ICD9_Diagnosis_3Digit.json");
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
            
            var response = RunIncRequestForSingleResult("INC_Pharmacy_Dispensings_By_Drug_Class.json");
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

            var response = RunIncRequestForSingleResult("INC_Pharmacy_Dispensings_By_Drug_Name.json");
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
            
            var response = RunMFURequestForSingleResult("MFU_HCPCS Procedures.json");
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

            var response = RunMFURequestForSingleResult("MFU_ICD9_Diagnosis_3Digit.json");
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

            var response = RunMFURequestForSingleResult("MFU_ICD9_Diagnosis_4Digit.json");
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

            var response = RunMFURequestForSingleResult("MFU_ICD9_Diagnosis_5Digit.json");
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

            var response = RunMFURequestForSingleResult("MFU_ICD9_Procedure_3Digit.json");
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

            var response = RunMFURequestForSingleResult("MFU_ICD9_Procedure_4Digit.json");
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

            var response = RunMFURequestForSingleResult("MFU_Pharmacy_Dispensings_By_Drug_Class.json");
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

            var response = RunMFURequestForSingleResult("MFU_Pharmacy_Dispensings_By_Drug_Name.json");
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

            var response = RunPrevRequestForSingleResult("Prev_Enrollment.json");
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

            var response = RunPrevRequestForSingleResult("Prev_HCPCS_Procedures.json");
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

            var response = RunPrevRequestForSingleResult("Prev_ICD9_Diagnosis_3Digit.json");
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

            var response = RunPrevRequestForSingleResult("Prev_ICD9_Diagnosis_4Digit.json");
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

            var response = RunPrevRequestForSingleResult("Prev_ICD9_Diagnosis_5Digit.json");
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

            var response = RunPrevRequestForSingleResult("Prev_ICD9_Procedure_3Digit.json");
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

            var response = RunPrevRequestForSingleResult("Prev_ICD9_Procedure_4Digit.json");
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

            var response = RunPrevRequestForSingleResult("Prev_Pharmacy_Dispensings_By_Drug_Class.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }

        [TestMethod]
        public void PMNDEV5164()
        {

            var response = RunIncRequestForSingleResult("PMNDEV-5164.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void PMNDEV5186()
        {

            var response = RunPrevRequestForSingleResult("PMNDEV-5186.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        [TestMethod]
        public void PMNDEV5194()
        {

            var response = RunPrevRequestForSingleResult("PMNDEV-5194.json");
            Logger.Debug(SerializeJsonToString(response));

        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO RunIncRequestForSingleResult(string queryJsonFilepath)
        {
            var query = LoadQuery(queryJsonFilepath);
            using (var adapter = Helper.CreateINCSummaryModelAdapterAdapter(ConnectionString))
            {
                return adapter.Execute(query, false).FirstOrDefault();
            }
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO RunMFURequestForSingleResult(string requestJsonFilepath)
        {
            var request = LoadQuery(requestJsonFilepath);
            using (var adapter = Helper.CreateMFUSummaryModelAdapterAdapter(ConnectionString))
            {
                return adapter.Execute(request, false).FirstOrDefault();
            }
        }
        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO RunPrevRequestForSingleResult(string requestJsonFilepath)
        {
            var request = LoadQuery(requestJsonFilepath);
            using (var adapter = Helper.CreatePrevSummaryModelAdapterAdapter(ConnectionString))
            {
                return adapter.Execute(request, false).FirstOrDefault();
            }
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerQueryDTO LoadQuery(string filename, string folder = ResourceFolder)
        {
            string filepath = System.IO.Path.Combine(folder, filename);
            var json = System.IO.File.ReadAllText(filepath);
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;

            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerQueryDTO>(json, jsonSettings);

            return query;
        }

        static string SerializeJsonToString(object response)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return Newtonsoft.Json.JsonConvert.SerializeObject(response, settings);
        }
    }
}
