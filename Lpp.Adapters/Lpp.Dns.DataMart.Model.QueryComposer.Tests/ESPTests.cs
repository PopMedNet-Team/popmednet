using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class ESPTests
    {
        const string ResourceFolder = "../Resources/QueryComposition";
        static readonly string ConnectionString;
        static readonly log4net.ILog Logger;

        static ESPTests()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ESP"].ConnectionString;
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(ESPTests));
        }

        [TestMethod]
        public void ESP_All_Terms()
        {
            var response = RunRequest("ESP_All_Terms.json");
            Logger.Debug(SerializeJsonToString(response));

            //Assert.IsTrue(response.Results.FirstOrDefault(r => r.Any()) != null, "There were no results");
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO RunRequest(string requestJsonFilepath)
        {
            var request = LoadRequest(requestJsonFilepath);
            using (var adapter = Helper.CreateESPModelAdapterAdapter(ConnectionString))
            {
                return adapter.Execute(request, false);
            }
        }

        Lpp.Dns.DTO.QueryComposer.QueryComposerQueryDTO LoadRequest(string filename, string folder = ResourceFolder)
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
