using System;
using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using PopMedNet.UITests.Models;
using System.Configuration;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using PopMedNet.UITests.Enums;

namespace PopMedNet.UITests.EndToEndTests
{
    public class CreateRequestTests
    {
        IPlaywright playwright;
        IBrowser browser;
        IBrowserContext context;
        IPage singlePage;

        string testUrl;

        public async Task<IPage> GetPage()
        {
            playwright = await Playwright.CreateAsync();
            //browser = await playwright.Chrome.LaunchAsync(new BrowserTypeLaunchOptions
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = bool.Parse(ConfigurationManager.AppSettings["globalHeadless"]),
                SlowMo = int.Parse(ConfigurationManager.AppSettings["globalSloMo"])
            });
            context = await browser.NewContextAsync();
            return await context.NewPageAsync();
        }

        [OneTimeSetUp]
        public void Setup()
        {

            testUrl = ConfigurationManager.AppSettings["baseUrl"];

            singlePage = GetPage().Result;

        }

        [OneTimeTearDown]
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
        [Retry(3)]
        [Category("PipelineTest")]
        public async Task CreateFileDistributionRequest_Submitted_StatusAndDataAreCorrect()
        {
            // Given
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);
            
            await loginPage.Goto();
            var userName = ConfigurationManager.AppSettings["enhancedUser"];
            var password = ConfigurationManager.AppSettings["adminPassword"];

            var projectname = ConfigurationManager.AppSettings["projectName"];
            var requestType = ConfigurationManager.AppSettings["requestType"];
            var testZip = $"{ConfigurationManager.AppSettings["testZipFile"]}";
            var requestName = $"File Distribution - Submitted Only {DateTime.Now.ToString("s")}";
            var dmName = ConfigurationManager.AppSettings["dataMart"];
            var dmName2 = ConfigurationManager.AppSettings["dataMart2"];

            var dataMarts = new System.Collections.Generic.List<string>
            {
                dmName,
                dmName2
            };

            var metaData = new RequestMetadataDTO()
            {
                Name = requestName
            };

            Console.WriteLine($"*** Logging in as {userName}");
            var homePage = await loginPage.LoginAs(userName, password);

            
            Console.WriteLine($"*** Creating new {requestType} request");
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            await requestPage.CreateNewRequestForProject(projectname);
            var dialog = await requestPage.ChooseRequestType(requestType);
            await dialog.FillRequestMetadata(metaData);
            var details = await dialog.Save();

            // Request Details page
            await details.UploadFilesForRequest(testZip);

            await details.SelectDatamarts(dataMarts);
            await details.SubmitRequest();
            await details.EnterComment();
            var requestId = await details.GetRequestId();

            Console.WriteLine($"*** Verify request status displays correctly in requests grid");
            await homePage.GoToPage(PageModels.Requests);
            await requestPage.SelectProjectTab(projectname);
            await requestPage.VerifyRequestStatus(requestName, RequestStatuses.Submitted);

            Console.WriteLine($"*** Open request details and verify details were saved correctly");
            await details.GoToRequest(requestId);
            await details.VerifyFileUploadInOverviewTab(testZip);
            await details.VerifyEventLogUpdate();
            await details.VerifyDataMartInRoutingsTable(dmName);
            await details.VerifyDataMartInRoutingsTable(dmName2);
            await details.VerifyTaskUpdateInRoutingsTable(dmName);
            await details.VerifyTaskUpdateInRoutingsTable(dmName2);
            await details.VerifyFileUploadInDocumentsTab(testZip);

            Assert.Pass();
        }
    }
}
