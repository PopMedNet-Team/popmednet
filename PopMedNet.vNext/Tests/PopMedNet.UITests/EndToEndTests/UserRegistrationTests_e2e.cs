using Lpp.Dns.DTO;
using Microsoft.Playwright;
using NUnit.Framework;
using PopMedNet.UITests.Enums;
using PopMedNet.UITests.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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

        [Test]
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

            // Submit new user registration
            var registrationPage = loginPage.RegisterNewAccount().Result;
            await registrationPage.FillInForm(userInfo);
            await registrationPage.Submit();


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

            // Assign organization
            await userDetails.SetUserOrganization(userInfo.OrganizationRequested);

            // Assign user to security group
            await userDetails.AddUserToSecurtyGroup(userInfo.OrganizationRequested, "Investigators");

            // Assign security group to control user
            await userDetails.AddPermissionsGroupForUser(userInfo.OrganizationRequested, "Administrators");

            // Activate account
            await userDetails.ActivateUser();
            await userDetails.SaveChanges();

            // Verify user status
            await userDetails.VerifyUserStatus(UserStatuses.Active);
            usersPage = await userDetails.GoToPage(PageModels.Users) as UsersPage;
            await usersPage.VerifyUserStatus(userName, UserStatuses.Active);

            // Log in as user, ensure they can log in
            loginPage = await usersPage.LogOff();
            homePage = await loginPage.LoginAs(userName, userInfo.Password);
            
            await homePage.VerifyLogin();
            
            // TODO: Check contents of following pannels for accuracy/count/whatever
            await homePage.VerifyControl(HomePagePanels.Notifications);
            await homePage.VerifyControl(HomePagePanels.Requests);

        }

        [Test]
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
            await registrationPage.Submit();


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

            // Submit new user registration
            var registrationPage = loginPage.RegisterNewAccount().Result;
            await registrationPage.FillInForm(userInfo);
            await registrationPage.Submit();

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

            var userDetailsUrl = singlePage.Url;
            Console.WriteLine($"userDetailsUrl = {userDetailsUrl}");

            // Delete account request, verify it does not display in Users list (?)
            usersPage = await userDetails.DeleteUser();
            await usersPage.ClearUserFilters();
            await usersPage.VerifyUserNotInGrid(userName);


            // Navigate directly to the account request via URL, verify it exists but is marked as deleted
            await userDetails.Navigate(userDetailsUrl);
            await userDetails.VerifyUserStatus(UserStatuses.Deleted);

            // Log in as user, ensure they CANNOT log in
            loginPage = await userDetails.LogOff();
            await loginPage.LoginAs(userName, userInfo.Password);
            await loginPage.VerifyInvalidLogin();
            
        }
    }
}
