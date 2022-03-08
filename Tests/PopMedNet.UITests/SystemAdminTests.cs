using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using System.Configuration;
using System.Collections.Specialized;
using PopMedNet.UITests.Models;

namespace PopMedNet.UITests
{
    class SystemAdminTests
    {
        IPlaywright playwright;
        IBrowser browser;
        IBrowserContext context;
        IPage singlePage;

        public string testUrl;

        public async Task<IPage> GetPage()
        {
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = bool.Parse(ConfigurationManager.AppSettings["globalHeadless"]),
                //SlowMo = 50,
            });
            context = await browser.NewContextAsync();
            return await context.NewPageAsync();
        }

        [SetUp]
        public void Setup()
        {

            testUrl = Environment.GetEnvironmentVariable("baseUrl");

            singlePage = GetPage().Result;
        }




        [Test] // May need to move this to a different test class if this class is running all its tests in parallel
        public async Task LoginAsSystemAdmin()
        {
            // Given

            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var userName = ConfigurationManager.AppSettings["adminUser"];
            var password = ConfigurationManager.AppSettings["adminPassword"];

            // When

            await loginPage.LoginAs(userName, password);

            // Then
            var welcomeText = await singlePage.WaitForSelectorAsync("#header > section > span");
            Assert.That(welcomeText.InnerTextAsync, Does.Contain("Welcome, System Administrator"));

            // TODO: Check for other elements that should be visible for an admin but not for non-admins?

        }
    }
}
