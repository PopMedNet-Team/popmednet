using System;
using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using PopMedNet.UITests.Models;
using System.Configuration;
using PopMedNet.UITests.Enums;

namespace PopMedNet.UITests.UIChecks
{
    public class RequestMetadataDialogChecks
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
                SlowMo = 1000,
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

        [Test] // May need to move this to a different test class if this class is running all its tests in parallel
        public async Task X_MultiAssertTest()
        {
            // Given

            //var loginPage = new LoginPage(singlePage);
            //await loginPage.Goto();
            //var userName = ConfigurationManager.AppSettings["adminUser"];
            //var password = ConfigurationManager.AppSettings["adminPassword"];

            var check1 = "check1";
            var check2 = "check2";
            var check3 = "check3";

            //// When

            //await loginPage.LoginAs(userName, password); // TODO: Update to allow for portal set-up (e.g., in QA)

            // Then
            Assert.That(check1, Is.EqualTo("check1"));
            Console.WriteLine("Check 1 passes");

            Assert.That(check2, Is.EqualTo("check2"));
            Console.WriteLine("Check 2 passes");

            //Assert.That(check3, Is.EqualTo("check4")); // Should fail
            //Console.WriteLine("Check 3 didn't pass this time...");

            Assert.That(check3, Is.EqualTo("check3")); // Should pass this time
            Console.WriteLine("Check 3 passed, finally...");

            // TODO: Check for other elements that should be visible for an admin but not for non-admins?

        }

        [Test] 
        public async Task X_ControlChecks()
        {
            // Given

            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var userName = ConfigurationManager.AppSettings["adminUser"];
            var password = ConfigurationManager.AppSettings["adminPassword"];

            // When

            var homePage = await loginPage.LoginAs(userName, password);
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            Assert.That(requestPage, Is.Not.Null);
            await requestPage.CreateNewRequestForProject("QA Project");
            var requestDialog = await requestPage.ChooseRequestType("[PCORNet] Default");

            // Then
            

        }

        [Test]
        public async Task CancelEditRequestMetadata_NoConfirm_DialogRemainsVisible()
        {
            // Given

            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var userName = ConfigurationManager.AppSettings["adminUser"];
            var password = ConfigurationManager.AppSettings["adminPassword"];

            // When

            var homePage = await loginPage.LoginAs(userName, password);
            var requestPage = await homePage.GoToPage(PageModels.Requests) as RequestsPage;
            Assert.That(requestPage, Is.Not.Null);
                await requestPage.CreateNewRequestForProject("QA Project");
            var requestDialog = await requestPage.ChooseRequestType("[PCORNet] Default");
            //var cancelConfirm = requestDialog.ClickAsync();
            // Then

        }
    }
}
