using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class BasePage
    {
        protected IPage _page;

        public BasePage()
        {

        }

        public BasePage(IPage page)
        {
            _page = page;
        }

        public async Task ClickItem(string locator, string controlName, bool waitForNetworkIdle = false)
        {
            Console.WriteLine($"Attempting to click '{controlName}'...");
            try
            {
                if(waitForNetworkIdle)
                    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await _page.Locator(locator).ScrollIntoViewIfNeededAsync();
                await _page.Locator(locator).ClickAsync();
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"\tCould not click '{controlName}'. Stopping test.");
                throw;
            }
        }

        public async Task ClickItem(FilterItemClickables controlName)
        {
            var locator = "";
            var waitForNetworkIdle = false;
            switch(controlName)
            {
                case FilterItemClickables.FilterMenu:
                    locator = ".k-i-filter";
                    break;
                case FilterItemClickables.ClearFilterButton:
                    locator = "button[title=Clear]";
                    break;
                case FilterItemClickables.FilterButton:
                    locator = "button[title=Filter]";
                    break;
                default:
                    throw new NotImplementedException($"'{controlName.ToString()}' not implemented. Stopping test.");
            }
            await ClickItem(locator, controlName.ToString(), waitForNetworkIdle);
        }

        /// <summary>
        /// Navigates directly to a page via URL, rather than using 
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public virtual async Task<BasePage> GoToPage(PageModels pageName)
        {
            var gotoUrl = ConfigurationManager.AppSettings["baseUrl"];

            switch (pageName)
            {
                case PageModels.Requests:
                    gotoUrl += "requests/";
                    await Navigate(gotoUrl);
                    return new RequestsPage(_page);
                case PageModels.Users:
                    gotoUrl += "users/";
                    await Navigate(gotoUrl);
                    return new UsersPage(_page);
                default:
                    Console.WriteLine("Page not specified for navigation. Navigating to Home Page.");
                    await Navigate(gotoUrl);
                    return new HomePage(_page);
            }
        }

        public async Task Navigate(string gotoUrl)
        {
            await _page.GotoAsync(gotoUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
        }

        public async Task<LoginPage> LogOff()
        {
            await _page.ClickAsync("text=Log Off");
            return new LoginPage(_page);
        }
        public async Task VerifyControl(string locator, string controlName, bool waitForNetworkIdle = false)
        {
            Console.WriteLine($"Verifying presence of control '{controlName}'...");
            try
            {
                if (waitForNetworkIdle)
                    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await _page.Locator(locator).ScrollIntoViewIfNeededAsync();
                await _page.Locator(locator).IsVisibleAsync();
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"\tCould not find control '{controlName}'. Stopping test.");
                throw;
            }
        }
    }
}
