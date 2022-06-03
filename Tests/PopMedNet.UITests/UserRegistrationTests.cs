using Lpp.Dns.DTO;
using Microsoft.Playwright;
using NUnit.Framework;
using PopMedNet.UITests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;

namespace PopMedNet.UITests
{
    class UserRegistrationTests
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
                //Headless = bool.Parse(ConfigurationManager.AppSettings["globalHeadless"]),
                Headless = false,
                SlowMo = 500,
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

        [TearDown]
        public void Teardown()
        {
            browser.CloseAsync();
        }

        [Test] // May need to move this to a different test class if this class is running all its tests in parallel
        public async Task SubmitValidRegistration_DisplaysSuccessDialog()
        {
            // Given
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var timeStamp = DateTime.Now.ToString("yyyyMMdd'-'HHmmss'.'fff");
            var email = $"bob.{timeStamp}@bob.org";
            var userName = $"bob.{timeStamp}";
            var userInfo = new UserRegistrationDTO()
            {
                FirstName = "Bob",
                MiddleName = "Bobovic",
                LastName = "Bobson",
                Title = "The Honorable",
                Email = email,
                Phone = "555-555-5555",
                Fax = "555-555-5556",
                OrganizationRequested = "Bobs International",
                RoleRequested = "Director-in-chief",
                UserName = userName,
                Password = "FakePassword123!@#",
            };

            // When

            var registrationPage = loginPage.RegisterNewAccount().Result;
            await registrationPage.FillInForm(userInfo);
            await registrationPage.Submit();

            // Then
            await registrationPage.SubmissionSuccessful();
        }

        [Test]
        public async Task SubmitRegistration_PasswordsDoNotMatch_DisplaysPasswordMatchMessage()
        {
            // Given
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var timeStamp = DateTime.Now.ToString("yyyyMMdd'-'HHmmss");
            var email = $"bob.{timeStamp}@bob.org";
            var userName = $"bob.{timeStamp}";
            var userInfo = new UserRegistrationDTO()
            {
                FirstName = "Bob",
                MiddleName = "Bobovitch",
                LastName = "Bobson",
                Title = "The Honorable",
                Email = email,
                Phone = "555-555-5555",
                Fax = "555-555-5556",
                OrganizationRequested = "Bobs International",
                RoleRequested = "Director-in-chief",
                UserName = userName,
                Password = "Abc123!@#",
            };
            var passwordConfirm = "Abc123%^";

            // When

            var registrationPage = loginPage.RegisterNewAccount().Result;
            await registrationPage.FillInForm(userInfo, passwordConfirm);
            await registrationPage.Submit();

            // Then
            Assert.That(await registrationPage.PasswordMatchErrorDisplays());
        }

        [Test]
        public async Task SubmitRegistration_UserNameNotUnique_DisplaysDuplicateUserNameMessage()
        {
            // Given
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var timeStamp = DateTime.Now.ToString("yyyyMMdd'-'HHmmss");
            var email = $"bob.{timeStamp}@bob.org";
            var userName = $"bob-dupllicate.{timeStamp}";
            var userInfo = new UserRegistrationDTO()
            {
                FirstName = "Bob",
                MiddleName = "B",
                LastName = "Bobson",
                Title = "The Honorable",
                Email = email,
                Phone = "555-555-5555",
                Fax = "555-555-5556",
                OrganizationRequested = "Bobs International",
                RoleRequested = "Director-in-chief",
                UserName = userName,
                Password = "FakePassword123!@#",
            };

            // Register accountto be duplicated
            Console.WriteLine($"Entering user account to be duplicated: {userName}...");
            var registrationPage = await loginPage.RegisterNewAccount();
            await registrationPage.FillInForm(userInfo);
            await registrationPage.Submit();
            var newLoginPage = await registrationPage.SubmissionSuccessful();


            // When - duplicate registrattion is entered
            Console.WriteLine("Entering duplicate account...");
            var dupReg = await newLoginPage.RegisterNewAccount();
            await dupReg.FillInForm(userInfo);
            await dupReg.Submit();

            // Then

            Assert.That(await dupReg.DuplicateUserNameErrorDisplays());
        }
        
        [TestCaseSource(nameof(GetRequiredFieldTestData))]
        public async Task CheckRequiredFieldValidators(
            string field,
            string firstName,
            string middleName,
            string lastName,
            string title,
            string email,
            string phone,
            string fax,
            string requestedOrg,
            string requestedRole,
            string userName,
            string password,
            string confirmPassword)
        {
            // Given
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var userInfo = new UserRegistrationDTO()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Title = title,
                Email = email,
                Phone = phone,
                Fax = fax,
                OrganizationRequested = requestedOrg,
                RoleRequested = requestedRole,
                UserName = userName,
                Password = password,
            };

            // When
            var regDialog = await loginPage.RegisterNewAccount();
            await regDialog.FillInForm(userInfo, confirmPassword);
            await regDialog.Submit();

            // Then
            var warning = await regDialog.ValidationMessageDisplays(field);
            Assert.That(warning, Is.True);
        }

        static object[] GetRequiredFieldTestData =
        {
            new object[] {
                "firstname",
                "",
                "Lenny",
                "Lennison",
                "Dr.",
                "lenny@LennyCorp.net",
                "555-555-1212",
                "555-555-1222",
                "LennyCorp",
                "Director",
                "lllennison",
                "LennyCorp#1",
                "LennyCorp#1"}, // missing firstName
            new object[] { 
                "lastname",
                "Leonard",
                "Lenny",
                "",
                "Dr.",
                "lenny@LennyCorp.net",
                "555-555-1212",
                "555-555-1222",
                "LennyCorp",
                "Director",
                "lllennison",
                "LennyCorp#1",
                "LennyCorp#1"}, // missing lastName
            new object[] {
                "email",
                "Leonard",
                "Lenny",
                "Lennison",
                "Dr.",
                "",
                "555-555-1212",
                "555-555-1222",
                "LennyCorp",
                "Director",
                "lllennison",
                "LennyCorp#1",
                "LennyCorp#1"}, // missing email
            new object[] { 
                "organization",
                "Leonard",
                "Lenny",
                "Lennison",
                "Dr.",
                "lenny@LennyCorp.net",
                "555-555-1212",
                "555-555-1222",
                "",
                "Director",
                "lllennison",
                "LennyCorp#1",
                "LennyCorp#1"}, // missing organization
            new object[] {
                "username",
                "Leonard",
                "Lenny",
                "Lennison",
                "Dr.",
                "lenny@LennyCorp.net",
                "555-555-1212",
                "555-555-1222",
                "LennyCorp",
                "Director",
                "",
                "LennyCorp#1",
                "LennyCorp#1"}, // missing username
            new object[] {
                "password",
                "Leonard",
                "Lenny",
                "Lennison",
                "Dr.",
                "lenny@LennyCorp.net",
                "555-555-1212",
                "555-555-1222",
                "LennyCorp",
                "Director",
                "lllennison",
                "",
                "LennyCorp#1"}, // missing password            
            new object[] {
                "confirmPassword",
                "Leonard",
                "Lenny",
                "Lennison",
                "Dr.",
                "lenny@LennyCorp.net",
                "555-555-1212",
                "555-555-1222",
                "LennyCorp",
                "Director",
                "lllennison",
                "LennyCorp#1",
                ""}, // missing confirmPassword
        };
    }
}
