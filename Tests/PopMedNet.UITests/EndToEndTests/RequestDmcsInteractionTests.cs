using Lpp.Dns.DTO.DataMartClient.Criteria;
using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework;
using PopMedNet.UITests.Enums;
using PopMedNet.UITests.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.Enums;

namespace PopMedNet.UITests.EndToEndTests
{
    class RequestDmcsInteractionTests
    {
        IPlaywright playwright;
        IBrowser browser;
        IBrowserContext context;
        IPage singlePage;

        string testUrl;

        public async Task<IPage> GetPage()
        {
            playwright = await Playwright.CreateAsync();
            
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = bool.Parse(ConfigurationManager.AppSettings["globalHeadless"]),
                SlowMo = int.Parse(ConfigurationManager.AppSettings["globalSloMo"])
            });
            context = await browser.NewContextAsync();
            return await context.NewPageAsync();
        }

        [SetUp]
        public void Setup()
        {

            testUrl = ConfigurationManager.AppSettings["baseUrl"];

            singlePage = GetPage().Result;

        }

        [TearDown]
        public void TearDown()
        {
            browser.CloseAsync();
        }

        private async Task CloseDialog(IPage page)
        {
            var closeButton = await page.WaitForSelectorAsync("[aria-label=\"close\"]");
            await closeButton.ClickAsync();
        }

        [Test]
        [Category("PipelineTest")]
        public async Task PutRequestOnHold_UsingMockDmc()
        {
            // Set up request for test, get RequestId (from href text)
            // Log in
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);

            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Request - Hold via Mock DMC - {DateTime.Now.ToString("s")}";

            Console.WriteLine($"*** Creating new request {requestName}");
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            
            var requestUrl = await requestPage.GenerateGenericRequest(requestName);
            var id = requestUrl.Split('=')[1];
            Console.WriteLine($"New request generated request Id {id}");
            
            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);

            // Use API to place request on hold
            var response = await UpdateRequestStatus(id, RoutingStatus.Hold, "", "Put request on hold");
            Console.WriteLine(response);

            // Open request details
            var requestDetails = await requestPage.OpenRequest(requestName);

            // Verify most recent entry includes "Submitted to Hold"
            await requestDetails.VerifyEventLogUpdate("Submitted to Hold");
            await requestDetails.LogOff();
        }

        [Test]
        [Category("PipelineTest")]
        [Retry(3)]
        public async Task RemoveHoldFromRequest_UsingMockDmc()
        {

            // set up request
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);

            await loginPage.Goto();
            var portalUserName = ConfigurationManager.AppSettings["enhancedUser"];
            var portalPassword = ConfigurationManager.AppSettings["enhancedUserPwd"];
            var homePage = await loginPage.LoginAs(portalUserName, portalPassword);

            var requestName = $"Remove Hold Test Request {DateTime.Now.ToString("s")}";
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            var requestUrl = await requestPage.GenerateGenericRequest(requestName);
            var id = requestUrl.Split('=')[1];
            
            await E2EUtils.WaitForRequestToProcess_HttpRequest(id);


            // put hold on request
            await UpdateRequestStatus(id, RoutingStatus.Hold, "", "Put request on hold");
            var requestDetails = await requestPage.GoToRequest(id);
            // Verify most recent entry includes "Submitted to Hold"
            await requestDetails.VerifyEventLogUpdate("Submitted to Hold");

            // remove hold from request
            await UpdateRequestStatus(id, RoutingStatus.Submitted, "", "Remove hold from request");
            // Verify most recent entry includes "Submitted to Hold"
            await requestDetails.GoToRequest(id);
            await requestDetails.VerifyEventLogUpdate("Hold to Submitted");
            await requestDetails.LogOff();
        }

        private async Task<RestResponse> PostDocumentChunk(DocumentMetadata metaData, byte[] data)
        {
            var request = new RestRequest(resource: $"/DMC/PostDocumentChunk", method: Method.Post);
            //var userName = ConfigurationManager.AppSettings["adminUser"];
            //var pwd = ConfigurationManager.AppSettings["adminPassword"];
            var userName = ConfigurationManager.AppSettings["apiUser"];
            var pwd = ConfigurationManager.AppSettings["apiPwd"];
            var client = new RestClient(ConfigurationManager.AppSettings["apiUrl"])
            {
                Authenticator = new HttpBasicAuthenticator(userName, pwd)
            };

            var content = new MultipartFormDataContent();

            var docContent = JsonConvert.SerializeObject(metaData);
            var sContent = new StringContent(docContent, Encoding.UTF8, "application/json");

            content.Add(sContent, "metadata");
            

            var docByteContent = new ByteArrayContent(data);
            docByteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            content.Add(docByteContent, "files", metaData.Name);
            

            request.AddJsonBody(content, "multipart/form-data");
            request.AddHeader("Content-Type", "multipart/form-data; boundary=XXX");

            var response = await client.PostAsync(request);
            if (response.IsSuccessful)
                Console.WriteLine("Success!");
            else Console.WriteLine($"Response code: {response.StatusCode}");
            return response;
        }

        private async Task<ResponseStatus> UpdateRequestStatus(string requestId, RoutingStatus status, string dataMartId, string message)
        {
            if (string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentNullException($"Null or empty string passed as 'requestId'. Stopping test.");
            if (string.IsNullOrWhiteSpace(dataMartId))
                dataMartId = ConfigurationManager.AppSettings["dataMartId"];
            if (string.IsNullOrEmpty(message))
                message = "Test message.";


            //var userName = ConfigurationManager.AppSettings["adminUser"];
            //var pwd = ConfigurationManager.AppSettings["adminPassword"];
            var userName = ConfigurationManager.AppSettings["apiUser"];
            var pwd = ConfigurationManager.AppSettings["apiPwd"];
            var client = new RestClient(ConfigurationManager.AppSettings["apiUrl"])
            {
                Authenticator = new HttpBasicAuthenticator(userName, pwd)
            };

            var request = new RestRequest(resource: "DMC/SetRequestStatus", method: Method.Put);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("RequestID", $"{requestId}");
            request.AddParameter("Status", $"{(int)status}");
            request.AddParameter("DataMartID", $"{dataMartId}");
            request.AddParameter("Message", $"{message}");


            Console.WriteLine("Attempting to place hold on request...");

            var response = await client.PutAsync(request);
            if(response.IsSuccessful)
                Console.WriteLine("Success!");
            else Console.WriteLine($"Response code: {response.StatusCode}");
            return response.ResponseStatus;
        }

        
    }    
}
