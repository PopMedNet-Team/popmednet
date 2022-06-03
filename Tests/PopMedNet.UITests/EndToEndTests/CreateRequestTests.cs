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
        [Category("Smoke Test")]
        public async Task CreateFileDistributionRequest_EndToEnd()
        {
            // Given
            await singlePage.SetViewportSizeAsync(1920, 1080);
            var loginPage = new LoginPage(singlePage);
            
            await loginPage.Goto();
            var userName = ConfigurationManager.AppSettings["adminUser"];
            var password = ConfigurationManager.AppSettings["adminPassword"];

            var projectname = ConfigurationManager.AppSettings["projectName"];
            var requestType = ConfigurationManager.AppSettings["requestType"];
            var testZip = $"{ConfigurationManager.AppSettings["testZipFile"]}";
            var requestName = $"FD Test Request {DateTime.Now.ToString("s")}";
            var dataMartName = ConfigurationManager.AppSettings["dataMart"];

            var metaData = new RequestMetadataDTO()
            {
                Name = requestName
            };

            // Log in
            var homePage = await loginPage.LoginAs(userName, password);

            // Requests page...
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            await requestPage.CreateNewRequestForProject(projectname);
                
            // New Request dialog
            var dialog = await requestPage.ChooseRequestType(requestType);
            await dialog.FillRequestMetadata(metaData);
            var details = await dialog.Save();

            // Request Details page
            await details.UploadFilesForRequest(testZip);
            await details.VerifyDocumentUpload(testZip);

            await details.SelectDatamarts(dataMartName);
            await details.SubmitRequest();
            await details.EnterComment();
            var requestId = await details.GetRequestId();

            await details.GoToRequest(requestId);
            await details.VerifyFileUploadInOverviewTab(testZip);
            await details.VerifyEventLogUpdate();
            await details.VerifyDataMartInRoutingsTable(dataMartName);
            await details.VerifyTaskUpdateInRoutingsTable();
            await details.VerifyFileUploadInDocumentsTab(testZip);

            // Verify request was saved
            await homePage.GoToPage(PageModels.Requests);
            await requestPage.SelectProjectTab(projectname);

            // Verify request status is 'Submitted'
            await requestPage.VerifyRequestStatus(requestName, RequestStatuses.Submitted);
            Assert.Pass();
            // Cancel request
        }
    }
}
