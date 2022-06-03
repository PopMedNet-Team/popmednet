using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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
            var loginUrl = ConfigurationManager.AppSettings["baseUrl"] + "login/";
            await _page.GotoAsync(loginUrl);
        }
        public async Task<HomePage> LoginAs(string userName, string password)
        {
            await _page.FillAsync("#txtUserName", userName);
            await _page.FillAsync("#txtPassword", password);
            await _page.ClickAsync("#btnLogin");
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
                await msg.IsVisibleAsync(new LocatorIsVisibleOptions() { Timeout = 5000 });
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Message not found. Stopping test.");
                throw;
            }
        }
    }

}
