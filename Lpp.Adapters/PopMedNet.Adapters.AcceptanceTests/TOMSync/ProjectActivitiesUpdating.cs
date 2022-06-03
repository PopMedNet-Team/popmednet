using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using System.Net.Http;

namespace PopMedNet.Adapters.AcceptanceTests.TOMSync
{
    [TestClass]
    public class ProjectActivitiesUpdating
    {
        readonly string serviceUrl = null;
        readonly string serviceUsername = null;
        readonly string servicePassword = null;

        public ProjectActivitiesUpdating()
        {
            serviceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Url"];
            serviceUsername = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.User"] ?? string.Empty).DecryptString();
            servicePassword = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.Password"] ?? string.Empty).DecryptString();
        }

        HttpClient CreateHttpClient()
        {
            var web = new HttpClient();
            web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));
            return web;
        }

        [TestMethod]
        public async Task GetActivities()
        {
            //wrong url will return a 404 message from the service
            //wrong credentials will return a 401 from the service

            Console.WriteLine("Getting task activities from: " + serviceUrl);
            using (var http = CreateHttpClient())
            {
                string json;
                using (var stream = await http.GetStreamAsync(serviceUrl))
                using (var reader = new System.IO.StreamReader(stream))
                {
                    json = await reader.ReadToEndAsync();
                    //Console.WriteLine(json);
                }

                Assert.IsNotNull(json);

                using (var jsonReader = new Newtonsoft.Json.JsonTextReader(new System.IO.StringReader(json))) 
                {
                    var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
                    serializer.Error += (sender, args) => {
                        Console.WriteLine(args.ErrorContext.Error.ToString());
                    };
                    var projectActivities = serializer.Deserialize<List<TaskOrderImportDTO>>(jsonReader);

                    Assert.IsNotNull(projectActivities, "Unable to convert json to TaskOrderImportDTO");

                    Console.WriteLine(projectActivities.Count + " project activities received from the service");
                }
            }
        }
    }
}
