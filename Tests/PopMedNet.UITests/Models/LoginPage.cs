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
        public async Task LoginAs(string userName, string password)
        {
            await _page.TypeAsync("#txtUserName", userName);
            await _page.TypeAsync("#txtPassword", password);
            await _page.ClickAsync("#btnLogin");
        }

        public async Task<NewUserRegistrationDialog> RegisterNewAccount()
        {
            await _page.ClickAsync("text = Register for a New Account");
            //await _page.WaitForSelectorAsync("[title='User Registration']");
            return new NewUserRegistrationDialog(_page);
        }
    }

}
