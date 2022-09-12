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

        public async Task<string> GetUrl()
        {
            return _page.Url;
        }
        /// <summary>
        /// Navigates directly to a page via URL, rather than using menus/buttons/etc.
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
                case PageModels.Home:
                    await Navigate(gotoUrl);
                    return new HomePage(_page);
                default:
                    Console.WriteLine("Page not specified for navigation. Navigating to Home Page.");
                    await Navigate(gotoUrl);
                    return new HomePage(_page);
            }
        }

        public virtual async Task Navigate(string gotoUrl)
        {
            if (Uri.IsWellFormedUriString(gotoUrl, UriKind.RelativeOrAbsolute))
            {
                Console.WriteLine($"Attempting to navigate to {gotoUrl}...");
                await _page.GotoAsync(gotoUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
                Console.WriteLine("Success!");
            }
        }

        /// <summary>
        /// Navigates directly to a Request via URL, rather than using menus/buttons/etc.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<RequestDetailsPage> GoToRequest(string requestId)
        {
            var url = ConfigurationManager.AppSettings["baseUrl"];
            url += $"requests/details?ID={requestId}";
            await Navigate(url);
            return new RequestDetailsPage(_page);
        }
        public virtual async Task<RequestDetailsPage> GoToRequest(Guid requestId)
        {
            return await GoToRequest(requestId.ToString());
        }

        public async Task<LoginPage> LogOff()
        {
            await _page.ClickAsync("text=Log Off");
            return new LoginPage(_page);
        }

        public async Task SortGrid(string columnName, bool descending = false, string gridLocator = "")
        {
            await _page.Locator($"[title='{columnName}'] .k-i-more-vertical").ClickAsync();
            var menu = _page.Locator("[data-role='menu'] :visible");

            if (descending)
            {
                await menu.Locator("span:text-is('Sort Descending')").ClickAsync();
                return;
            }
            else
            {
                await menu.Locator("span:text-is('Sort Ascending')").ClickAsync();
                return;
            }
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

        public async Task WaitForPageLoad()
        {
            Console.WriteLine("Waiting for page to finish loading...");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}
