using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Utilities;

namespace Lpp.Dns.Api.Tests.DataMartClient
{
    [TestClass]
    public class AdapterPackagesTests
    {
        [TestMethod]
        public void DownloadPackage()
        {
            const string identifier = "Lpp.Dns.DataMart.Model.DataChecker";
            const string version = "4.0.3.0";

            using (var controller = new TestAdaptersController())
            {
                var response = AsyncHelpers.RunSync<System.Net.Http.HttpResponseMessage>(() => controller.GetPackage(identifier, version));
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Assert.Fail(response.ReasonPhrase);
                    return;
                }

                string filename = string.Format("{0}.{1}.zip", identifier, version);
                using (var stream = AsyncHelpers.RunSync<System.IO.Stream>(() => response.Content.ReadAsStreamAsync()))
                {
                    using (var filestream = new System.IO.FileStream(filename, System.IO.FileMode.Create))
                    {
                        stream.CopyTo(filestream);
                    }
                }
            }
        }

        [TestMethod]
        public void GetCurrentPackageVersion()
        {
            const string identifier = "Lpp.Dns.DataMart.Model.DataChecker";
            using (var controller = new TestAdaptersController())
            {
                string version = AsyncHelpers.RunSync<string>(() => controller.GetCurrentVersion(identifier));
                Console.WriteLine(version);
            }
        }

        public class TestAdaptersController : Lpp.Dns.Api.DataMartClient.AdaptersController
        {
            public TestAdaptersController() : base(@"..\..\..\Lpp.Dns.Api\App_Data")
            {
            }
        }
    }
}
