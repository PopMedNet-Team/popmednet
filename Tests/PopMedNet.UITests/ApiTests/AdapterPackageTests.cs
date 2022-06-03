using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.ApiTests
{
    public class AdapterPackageTests
    {
        [Test]
        [Category("Smoke Test")]
        public void GetCurrentVersionNumber_ReturnsLatest()
        {
            // Given
            var currentVersion = ConfigurationManager.AppSettings["currentVersion"];// TODO: Move into web.config or get from Adapters location?

            // When

            var userName = ConfigurationManager.AppSettings["adminUser"];
            var pwd = ConfigurationManager.AppSettings["adminPassword"];
            var client = new RestClient(ConfigurationManager.AppSettings["apiUrl"])
            {
                Authenticator = new HttpBasicAuthenticator(userName, pwd)
            };
            var request = new RestRequest(resource: "Adapters/GetCurrentVersion?identifier=Lpp.Dns.DataMart.Model.QueryComposer", method: Method.Get);
            var  response = client.GetAsync(request).Result;
            
            // Then

            Console.WriteLine($"Received response with content of {response.Content}");

            JObject joResponse = JObject.Parse(response.Content);
            JArray actual = (JArray)joResponse["results"];

            Assert.AreEqual(currentVersion, actual[0].ToString());
        }

        [Test]
        [Category("Smoke Test")]
        public void GetAdapterPackage_ReturnsPackage()
        {
            var currentVersion = ConfigurationManager.AppSettings["currentVersion"];
            var expected = currentVersion + ".zip";
            var userName = ConfigurationManager.AppSettings["adminUser"];
            var pwd = ConfigurationManager.AppSettings["adminPassword"];
            var client = new RestClient(ConfigurationManager.AppSettings["apiUrl"])
            {
                Authenticator = new HttpBasicAuthenticator(userName, pwd)
            };

            var resourceUrl = "Adapters/GetPackage?identifier=Lpp.Dns.DataMart.Model.QueryComposer&version=" + currentVersion;
            var request = new RestRequest(resource: resourceUrl, method: Method.Get);
            var response = client.GetAsync(request).Result;
            var headers = response.Headers.ToString();

            Console.WriteLine("\n*****ContentHeaders:");
            var cheaders = response.ContentHeaders.ToList();
            foreach (var cheader in cheaders)
                { Console.WriteLine(cheader.ToString()); }
            var contentDisposition = cheaders.Find(c => c.Name == "Content-Disposition");
            var actual = contentDisposition.Value.ToString();

            Assert.That(actual, Does.Contain(expected).IgnoreCase);
        }

        [Test]
        [TestCaseSource(nameof(GetOldVersionsAndRequests))]
        [Category("Smoke Test")]
        public void GetAdapterPackageForRequest_ReturnsCorrectVersion(RequestVersion data)
        {
            // n.b. - targetVersion MUST be an adapter version that is on the API server.
            // requestId MUST be a request whose AdapterPackageVersion value equals targetVersion.
            // It may be necessary to poke around in the system under test to get valid values
            // for these variables. Specifically, check Lpp.Dns.Api/App_Data for the adapters 
            // (just pull the version number), then query the Requests table to find a request with
            // a matching AdapterPackageVersion

            // TODO: Consider setting up an array that will contain each version plus a request with that version. 
            // Then set it up as a data-driven test (See UserRegistrationTests.CheckRequiredFieldValidators)

            //var requestId = ConfigurationManager.AppSettings["oldVersionRequestId"];
            //var targetVersion = ConfigurationManager.AppSettings["oldVersionNumber"];
            var requestId = data.Request;
            var targetVersion = data.Version;
            var expectedFile = targetVersion + ".zip";
            var userName = ConfigurationManager.AppSettings["adminUser"];
            var pwd = ConfigurationManager.AppSettings["adminPassword"];
            var client = new RestClient(ConfigurationManager.AppSettings["apiUrl"])
            {
                Authenticator = new HttpBasicAuthenticator(userName, pwd)
            };

            var resourceUrl = "Adapters/GetPackageForRequest?requestID=" + requestId;
            var request = new RestRequest(resource: resourceUrl, method: Method.Get);
            var response = client.GetAsync(request).Result;
            var headers = response.Headers.ToString();

            Console.WriteLine("\n*****ContentHeaders:");
            var cheaders = response.ContentHeaders.ToList();
            foreach (var cheader in cheaders)
            { Console.WriteLine(cheader.ToString()); }
            var contentDisposition = cheaders.Find(c => c.Name == "Content-Disposition");
            var actual = contentDisposition.Value.ToString();

            Assert.That(actual, Does.Contain(expectedFile).IgnoreCase);
        }

        static RequestVersion[] GetOldVersionsAndRequests()
        {
            var versions = ConfigurationManager.AppSettings["oldVersionNumber"].Split(',');
            var requests = ConfigurationManager.AppSettings["oldVersionRequestId"].Split(',');

            if (versions.Length != requests.Length)
            {
                var msg = $"There must be an equal number of requests and versions. " +
                    $"The values passed have {requests.Length} requests and {versions.Length} versions.";
                throw new ArgumentException(msg);
            }

            var data = new RequestVersion[requests.Length];
            for (var i = 0; i < requests.Length; i++)
            {
                data[i] = new RequestVersion()
                {
                    Version = versions[i].ToString().Trim(),
                    Request = requests[i].ToString().Trim()
                };
            }
            return data;
                
        }

    }
    public class RequestVersion
    {
        public string Version { get; set; }
        public string Request { get; set; }
    }
        

}
