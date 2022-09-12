using Lpp.Dns.DTO;
using Microsoft.Playwright;
using NUnit.Framework;
using PopMedNet.UITests.Enums;
using PopMedNet.UITests.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace PopMedNet.UITests.EndToEndTests
{
    class UserRegistrationTests_e2e
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
                SlowMo = int.Parse(ConfigurationManager.AppSettings["globalSloMo"])
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

        [Test]
        [Category("PipelineTest")]
        public async Task SubmitAndApproveUserRegistration()
        {
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var timeStamp = DateTime.Now.ToString("yyyyMMdd'-'HHmmss'.'fff");
            var email = $"ApproveUser.{timeStamp}@test.org";
            var userName = $"ApproveUser.{timeStamp}";
            var userInfo = new UserRegistrationDTO()
            {
                FirstName = "Approve",
                MiddleName = "Test",
                LastName = "User",
                Title = "",
                Email = email,
                Phone = "555-555-5555",
                Fax = "555-555-5556",
                OrganizationRequested = ConfigurationManager.AppSettings["genericOrganization"],
                RoleRequested = "Tester",
                UserName = userName,
                Password = ConfigurationManager.AppSettings["genericPassword"]
            };

            Console.WriteLine($"*** Submit new registration for {userInfo}");
            var registrationPage = loginPage.RegisterNewAccount().Result;
            await registrationPage.FillInForm(userInfo);
            await registrationPage.Submit(true);

            var adminUser = ConfigurationManager.AppSettings["adminUser"];
            var adminPwd = ConfigurationManager.AppSettings["adminPassword"];

            Console.WriteLine($"*** Log in as administrator {adminUser}");
            var homePage = await loginPage.LoginAs(adminUser, adminPwd);
            await homePage.VerifyLogin();

            Console.WriteLine($"*** Show details for user {userName}");
            var usersPage = await homePage.GoToPage(PageModels.Users) as UsersPage;
            await usersPage.ClearUserFilters();

            // Select new registration from Users list, verifying it's set to inactive initially
            var userDetails = await usersPage.SelectUser(userName);

            Console.WriteLine($"*** Assign organization");
            await userDetails.SetUserOrganization(userInfo.OrganizationRequested);

            Console.WriteLine($"*** Assign user to security group");
            await userDetails.AddUserToSecurtyGroup(userInfo.OrganizationRequested, "Investigators");

            Console.WriteLine($"*** Activate user and save changes");
            await userDetails.ActivateUser();
            await userDetails.SaveChanges();

            Console.WriteLine($"*** Verify user status displays as 'Active' in their details page");
            await userDetails.VerifyUserStatus(UserStatuses.Active);
            usersPage = await userDetails.GoToPage(PageModels.Users) as UsersPage;
            Console.WriteLine($"*** Verify user status displays as 'Active' in user grid");
            await usersPage.VerifyUserStatus(userName, UserStatuses.Active);

            Console.WriteLine($"*** Log in as {userName}");
            loginPage = await usersPage.LogOff();
            homePage = await loginPage.LoginAs(userName, userInfo.Password);

            
            await homePage.VerifyLogin();
        }

        [Test]
        [Category("PipelineTest")]
        public async Task SubmitAndRejectNewUserRegistration()
        {
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var timeStamp = DateTime.Now.ToString("yyyyMMdd'-'HHmmss'.'fff");
            var email = $"RejectUser.{timeStamp}@test.org";
            var userName = $"RejectUser.{timeStamp}";
            var userInfo = new UserRegistrationDTO()
            {
                FirstName = "Reject",
                MiddleName = "Test",
                LastName = "User",
                Title = "",
                Email = email,
                Phone = "555-555-5555",
                Fax = "555-555-5556",
                OrganizationRequested = "Test Organization",
                RoleRequested = "Tester",
                UserName = userName,
                Password = ConfigurationManager.AppSettings["genericPassword"]
            };

            // Submit new user registration
            var registrationPage = loginPage.RegisterNewAccount().Result;
            await registrationPage.FillInForm(userInfo);
            await registrationPage.Submit(handleAlert:true);


            // Log in as Admin
            var adminUser = ConfigurationManager.AppSettings["adminUser"];
            var adminPwd = ConfigurationManager.AppSettings["adminPassword"];
            var homePage = await loginPage.LoginAs(adminUser, adminPwd);
            await homePage.VerifyLogin();

            // Open Network/Users
            var usersPage = await homePage.GoToPage(PageModels.Users) as UsersPage;
            await usersPage.ClearUserFilters();

            // Select new registration from Users list, verifying it's set to inactive initially
            var userDetails = await usersPage.SelectUser(userName);

            // Reject account
            await userDetails.RejectUserRequest("Rejecting for test reasons.");
            await userDetails.SaveChanges();

            // Verify state shows as inactive in UsersGrid
            usersPage = await userDetails.GoToPage(PageModels.Users) as UsersPage;
            
            await usersPage.VerifyUserStatus(userName, UserStatuses.Inactive);

            // Verify "Rejected" label displays in User Details (after navigating away)
            userDetails = await usersPage.SelectUser(userName);
            await userDetails.VerifyUserStatus(UserStatuses.Rejected);


            // Clear filters as cleanup
            usersPage = await userDetails.GoToPage(PageModels.Users) as UsersPage;
            await usersPage.ClearUserFilters();

        }

        [Test]
        [Category("PipelineTest")]
        public async Task SubmitAndDeleteNewUserRegitration()
        {
            var loginPage = new LoginPage(singlePage);
            await loginPage.Goto();
            var timeStamp = DateTime.Now.ToString("yyyyMMdd'-'HHmmss'.'fff");
            var email = $"DeleteUser.{timeStamp}@test.org";
            var userName = $"DeleteUser.{timeStamp}";
            var userInfo = new UserRegistrationDTO()
            {
                FirstName = "Delete",
                MiddleName = "Test",
                LastName = "User",
                Title = "",
                Email = email,
                Phone = "555-555-5555",
                Fax = "555-555-5556",
                OrganizationRequested = "Test Organization",
                RoleRequested = "Tester",
                UserName = userName,
                Password = ConfigurationManager.AppSettings["genericPassword"]
            };

            
            Console.WriteLine("*** Submit new user registration");
            var registrationPage = loginPage.RegisterNewAccount().Result;
            await registrationPage.FillInForm(userInfo);
            await registrationPage.Submit(handleAlert:true);

            Console.WriteLine("*** Log in as Admin");
            var adminUser = ConfigurationManager.AppSettings["adminUser"];
            var adminPwd = ConfigurationManager.AppSettings["adminPassword"];
            var homePage = await loginPage.LoginAs(adminUser, adminPwd);
            await homePage.VerifyLogin();

            Console.WriteLine("*** Verify user is inactive");
            var usersPage = await homePage.GoToPage(PageModels.Users) as UsersPage;
            await usersPage.ClearUserFilters();
            var userDetails = await usersPage.SelectUser(userName);

            var userDetailsUrl = singlePage.Url;
            Console.WriteLine($"userDetailsUrl = {userDetailsUrl}");
            
            Console.WriteLine("*** Delete account request and verify it does not display in Users list");
            usersPage = await userDetails.DeleteUser();
            await usersPage.ClearUserFilters();
            await usersPage.VerifyUserNotInGrid(userName);

            Console.WriteLine("*** Verify account is marked as deleted");
            await userDetails.Navigate(userDetailsUrl);
            await userDetails.VerifyUserStatus(UserStatuses.Deleted);

            Console.WriteLine("*** Verify deleted user cannot log in");
            loginPage = await userDetails.LogOff();
            await loginPage.LoginAs(userName, userInfo.Password, validLogin:false);
            await loginPage.VerifyInvalidLogin();
            
        }
    }
}
