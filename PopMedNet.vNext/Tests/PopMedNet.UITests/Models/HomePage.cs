using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class HomePage:BasePage
    {
        //private readonly IPage _page;

        public HomePage(IPage page)
        {
            base._page = page;
        }

        
        public async Task<bool> VerifyLogin()
        {
            try
            {
                Console.WriteLine("Verifying logon, searching for welcome text...");
                var welcomeText = _page.Locator("span:has-text('Welcome,')");
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await welcomeText.WaitForAsync();
                Console.WriteLine("Success!");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Could not locate welcome text in home page. Canceling test.");
                throw;
            }
        }

        public async Task VerifyControl(HomePagePanels panelToVerify)
        {
            var locator = "";
            var waitForNetwork = false;
            switch(panelToVerify)
            {
                case HomePagePanels.Notifications:
                    locator = "#pNotifications";
                    waitForNetwork = true;
                    break;
                case HomePagePanels.Messages:
                    locator = "#pMessages";
                    break;
                case HomePagePanels.Tasks:
                    locator = "#pTasks";
                    break;
                case HomePagePanels.Requests:
                    locator = "#pRequests";
                    break;
                case HomePagePanels.DataMarts:
                    locator = "#pDataMarts";
                    break;
                default:
                    throw new NotImplementedException($"'{panelToVerify}' is not set up in HomePage.VerifyControl(). Stopping test.");
            }
            
            await base.VerifyControl(locator, panelToVerify.ToString(), waitForNetwork);
        }
    }
}
