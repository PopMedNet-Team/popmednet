using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using PopMedNet.UITests.Models;
using System.Configuration;

namespace PopMedNet.UITests
{
    public class LoginPageTests_NoLogin
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
                //SlowMo = 50,
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
        public async Task LaunchPMN()
        {
            // Given - nothing extra here

            // When
            await singlePage.GotoAsync(testUrl);

            // Then
            Assert.That(singlePage.Url, Does.StartWith("http://localhost:60338/"));
        }


        [Test]
        public async Task Login_NoUsernameEntered_UserNameValidationMessageDisplays()
        {
            // Given
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();

            // When            
            await loginPage.LoginAs("", "FakePass");

            // Then
            var userNameValText = await singlePage.WaitForSelectorAsync("#username-error");
            Assert.That(userNameValText.InnerTextAsync, Is.EqualTo("username is required"));
        }

        [Test]
        public async Task Login_NoUsernameOrPasswordEntered_UserNameAndPasswordValidationMessagesDisplay()
        {
            // Given
            await singlePage.GotoAsync(testUrl);

            // When

            await singlePage.ClickAsync("#btnLogin");

            // Then
            var userNameValText = await singlePage.WaitForSelectorAsync("#username-error");
            var passwordValText = await singlePage.WaitForSelectorAsync("#password-error");

            Assert.That(userNameValText.InnerTextAsync, Is.EqualTo("username is required"),
                "Username validation message did not display as expected.");
            Assert.That(passwordValText.InnerTextAsync, Is.EqualTo("password is required"),
                "Password validation message did not display as expected.");
        }

        [Test]
        public async Task ContactUs_NavigatesToPMNSupportDesk()
        {
            // Given
            await singlePage.GotoAsync(testUrl);

            // When
            var newPage = await context.RunAndWaitForPageAsync(async () =>
            {
                await singlePage.ClickAsync("text=Contact Us");
            });

            // Then 
            await newPage.WaitForLoadStateAsync();
            var title = await newPage.TitleAsync();
            Assert.That(title, Is.EqualTo("PopMedNet Support - Jira Service Management"));
            
            await singlePage.BringToFrontAsync();
        }

        [Test]
        public async Task TermsAndConditions_DisplaysTandCDialog()
        {
            // Given
            await singlePage.GotoAsync(testUrl);

            // When
            await singlePage.ClickAsync("#Content :text(\"Terms and Conditions\")");

            // Then 
            var dialogTitle = await singlePage.WaitForSelectorAsync(".k-window-title");
            Assert.That(dialogTitle.InnerTextAsync, Is.EqualTo("Terms And Conditions"));
            await CloseDialog(singlePage);
        }

        [Test]
        public async Task TermsAndConditionsInFooter_DisplaysTandCDialog()
        {
            // Given
            await singlePage.GotoAsync(testUrl);

            // When
            await singlePage.ClickAsync("#footer :text(\"Terms and Conditions\")");

            // Then 
            var dialogTitle = await singlePage.WaitForSelectorAsync(".k-window-title");
            Assert.That(dialogTitle.InnerTextAsync, Is.EqualTo("Terms And Conditions"));
            await CloseDialog(singlePage);
        }

        [Test]
        public async Task InfoClick_DisplaysFundingInfoDialog()
        {
            // Given
            await singlePage.GotoAsync(testUrl);

            // When
            await singlePage.ClickAsync("#footer :text(\"Info\")");

            // Then 
            var dialogTitle = await singlePage.WaitForSelectorAsync(".k-window-title");
            Assert.That(dialogTitle.InnerTextAsync, Is.EqualTo("Information"));
            await CloseDialog(singlePage);
        }

       

        [Test]
        public async Task ForgotPasswordClick_DisplaysForgotPasswordDialog()
        {
            // Given

            await singlePage.GotoAsync(testUrl);

            // When

            await singlePage.ClickAsync("#lForgotPassword");

            // Then 
            var dialogTitle = await singlePage.WaitForSelectorAsync(".k-window-title");
            Assert.That(dialogTitle.InnerTextAsync, Is.EqualTo("Forgot Password"));
        }

        [Test]
        public async Task Login_IncorrectUsername_DisplaysInvalidUserNameOrPassword()
        {
            // Given

            var badUserName = Guid.NewGuid().ToString();
            var badPassword = Guid.NewGuid().ToString();

            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();

            // When

            await loginPage.LoginAs(badUserName, badPassword);

            // Then 

            var warning = await singlePage.WaitForSelectorAsync(".validation-summary-errors");
            Assert.That(warning.InnerTextAsync, Does.Contain("Invalid user name or password"));
        }

        [Test]
        public async Task Login_IncorrectPassword_DisplaysInvalidUserNameOrPassword()
        {
            // Given

            var userName = ConfigurationManager.AppSettings["adminUser"];
            var badPassword = Guid.NewGuid().ToString();

            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            await singlePage.GotoAsync(testUrl);

            // When

            await loginPage.LoginAs(userName, badPassword);

            // Then 

            var warning = await singlePage.WaitForSelectorAsync(".validation-summary-errors");
            Assert.That(warning.InnerTextAsync, Does.Contain("The Login or Password is invalid."));
        }
    }
}