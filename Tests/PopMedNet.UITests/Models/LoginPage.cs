using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task Goto()
        {
            var loginUrl = ConfigurationManager.AppSettings["baseUrl"];
            if (bool.Parse(ConfigurationManager.AppSettings["useSso"]) != true)
                loginUrl += "login/"; // May not be strictly necessary if running on local (i.e., not SSO)
            await _page.GotoAsync(loginUrl);
        }
        public async Task<HomePage> LoginAs(string userName, string password, bool validLogin=true)
        {
            var ssoUrl = ConfigurationManager.AppSettings["ssoUrl"];
            var loginButonSelector = "#btnLogin";
            if (ConfigurationManager.AppSettings["useSso"] == "true")
                loginButonSelector = "button:has-text('Sign On')";
            await _page.FillAsync("#txtUserName", userName);
            await _page.FillAsync("#txtPassword", password);
            await _page.Locator(loginButonSelector).ClickAsync();
            var currentUrl = _page.Url;
            if (validLogin)
            {
                if (Regex.IsMatch(currentUrl, @".*" + ssoUrl + "*"))
                {
                    ILocator loc = null;
                    try
                    {
                        loc = _page.Locator("h4:has-text('PopMedNet Edge')");
                        await loc.ClickAsync();
                    }
                    catch (TimeoutException e)
                    {
                        loc = _page.Locator("h4:has-text('PopMedNet')");
                        await loc.ClickAsync();
                    }
                }
            }
            return new HomePage(_page);
        }

        public async Task<NewUserRegistrationDialog> RegisterNewAccount()
        {
            await _page.ClickAsync("text = Register for a New Account");
            return new NewUserRegistrationDialog(_page);
        }

        public async Task VerifyInvalidLogin()
        {
            Console.WriteLine("Checking for invalid credentials message...");
            try
            {
                var msg = _page.Locator("span:has-text('Invalid user name or password.')");
                if (ConfigurationManager.AppSettings["useSso"] == "true")
                    msg = _page.Locator("label:has-text('Please check your username and password and try again')");
                await msg.IsVisibleAsync(new LocatorIsVisibleOptions() { Timeout = 5000 });
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Message not found. Stopping test.");
                throw;
            }
        }
    }

}
