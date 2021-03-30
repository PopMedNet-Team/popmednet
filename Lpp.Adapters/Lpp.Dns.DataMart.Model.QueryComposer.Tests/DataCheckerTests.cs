using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class DataCheckerTests
    {
        const string ResourceFolder = "../Resources/DataChecker";
        static readonly string ConnectionString;
        static readonly log4net.ILog Logger;

        static DataCheckerTests()
        {
             ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataChecker"].ConnectionString;
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(SummaryQueryTests));
        }

        /// <summary>
        /// DataChecker Race Test
        /// </summary>
        [TestMethod]
        public void DataCheckerRace()
        {

            var response = RunDataCheckerRequest("DataChecker_Race.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// DataChecker Diagnosis Codes Test
        /// </summary>
        [TestMethod]
        public void DataCheckerDiagCodes()
        {

            var response = RunDataCheckerRequest("DataChecker_DiagCodes.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// DataChecker Ethnicity Test
        /// </summary>
        [TestMethod]
        public void DataCheckerEthnicity()
        {

            var response = RunDataCheckerRequest("DataChecker_Ethnicity.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        /// <summary>
        /// MFU_HCPCS Procedures Test
        /// </summary>
        [TestMethod]
        public void DataCheckerMetadata()
        {

            var response = RunDataCheckerRequest("DataChecker_MetaData.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
         /// <summary>
        /// DataChecker NDC Test
        /// </summary>
        [TestMethod]
        public void DataCheckerNDC()
        {

            var response = RunDataCheckerRequest("DataChecker_NDC.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
         /// <summary>
        /// DataChecker PDX Test
        /// </summary>
        [TestMethod]
        public void DataCheckerPDX()
        {
            //using (var db = Helper.CreateDataCheckerDataContext(ConnectionString))
            //{
            //    var datapartners = new []{ "OPS" };
            //    var pdxCriteria = new []{"P","S"};
            //    var encounterCriteria = new []{"AV", "ED", "IP", "IS", "OA"};

            //    var query = db.PDXs.Select(p => new PDXResult { DataPartner = p.DataPartner, PDX = string.IsNullOrEmpty(p.PDXs) ? "MISSING" : pdxCriteria.Contains(p.PDXs) ? p.PDXs : "OTHER", EncType = encounterCriteria.Contains(p.EncType) ? p.EncType : "OTHER", n = p.n });
            //    query = query.Where(p => !string.IsNullOrEmpty(p.DataPartner) && datapartners.Contains(p.DataPartner));
            //    query = query.GroupBy(p => new { p.DataPartner, p.PDX, p.EncType }).Select(p => new PDXResult { DataPartner = p.Key.DataPartner, PDX = p.Key.PDX, EncType = p.Key.EncType, n = p.Sum(k => k.n) });


            //    var allEncQuery = db.PDXs.Select(p => new PDXResult { DataPartner = p.DataPartner, PDX = string.IsNullOrEmpty(p.PDXs) ? "MISSING" : pdxCriteria.Contains(p.PDXs) ? p.PDXs : "OTHER", EncType = "ALL", n = p.n });
            //    allEncQuery = allEncQuery.Where(p => pdxCriteria.Contains(p.PDX) && !string.IsNullOrEmpty(p.DataPartner) && encounterCriteria.Contains(p.EncType) && datapartners.Contains(p.DataPartner));
            //    allEncQuery = query.GroupBy(p => new { p.DataPartner, p.PDX, p.EncType }).Select(p => new PDXResult { DataPartner = p.Key.DataPartner, PDX = p.Key.PDX, EncType = p.Key.EncType, n = p.Sum(k => k.n) });

            //    var combined = query.Union(allEncQuery);
            //    Console.WriteLine(combined.ToString());
            //}



            var response = RunDataCheckerRequest("DiagnosisPDX.json");
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();

            
        }

        public class PDXResult
        {
            public string DataPartner { get; set; }
            public string PDX { get; set; }
            public string EncType { get; set; }
            public double n { get; set; }
        }


        [TestMethod]
        public void DataCheckerRxAmt()
        {

            var response = RunDataCheckerRequest("DataChecker_RxAmt.json");
            Logger.Debug(SerializeJsonToString(response));
            
            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();            
            var row = table.First();

            
        }
        [TestMethod]
        public void DataCheckerRxSup()
        {

            var response = RunDataCheckerRequest("DataChecker_RxSup.json");
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();


        }
        [TestMethod]
        public void DataCheckerAge()
        {

            var response = RunDataCheckerRequest("DataChecker_Age.json");
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();


        }
        [TestMethod]
        public void DataCheckerHeight()
        {

            var response = RunDataCheckerRequest("DataChecker_Height.json");
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();


        }

        [TestMethod]
        public void DataCheckersSex()
        {

            var response = RunDataCheckerRequest("DataChecker_Sex.json");
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();


        }

        [TestMethod]
        public void DataCheckersWeight()
        {

            var response = RunDataCheckerRequest("DataChecker_Weight.json");
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();


        }
        [TestMethod]
        public void DataCheckersSqlDistribution()
        {

            var response = RunDataCheckerRequest("DataChecker_SqlDistribution.json");
            Logger.Debug(SerializeJsonToString(response));

            Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");

            var table = response.Results.First();
            var row = table.First();


        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO RunDataCheckerRequest(string requestJsonFilepath)
        {
            var request = LoadDataCheckerRequest(requestJsonFilepath);
            using (var adapter = Helper.CreateQueryComposerModelProcessorForDataChecker(ConnectionString))
            {
                return adapter.Execute(request, false);
            }
        }
        Lpp.Dns.DTO.QueryComposer.QueryComposerQueryDTO LoadDataCheckerRequest(string filename, string folder = ResourceFolder)
        {
            string filepath = System.IO.Path.Combine(folder, filename);
            var json = System.IO.File.ReadAllText(filepath);

            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;

            var request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerQueryDTO>(json, jsonSettings);

            return request;
        }

        static string SerializeJsonToString(object response)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            return Newtonsoft.Json.JsonConvert.SerializeObject(response, settings);
        }
    }
}
