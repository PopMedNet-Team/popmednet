using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class UsersPage:BasePage
    {
        //private readonly IPage _page;
        public UsersPage(IPage page)
        {
            _page = page;
        }

        public async Task<UsersPage> GoToPage()
        {
            return await GoToPage(PageModels.Users) as UsersPage;
        }

        private async Task ClickItem(UsersPageClickables controlName)
        {
            var locator = "";
            var waitForNetworkIdle = false;
            switch(controlName)
            {
                case UsersPageClickables.UserNameColumnMoreIcon:
                    locator = "[title='User Name edit column settings']";
                    break;
                default:
                    throw new NotImplementedException($"Control '{controlName}' not implemented in UsersPage.cs");
            }
            await base.ClickItem(locator, controlName.ToString(), waitForNetworkIdle);
        }

        private async Task EnterFilterText(string filterText)
        {
            Console.WriteLine("Atttempting to enter filter text...");

            try
            {
                var filterBox = _page.Locator("div.k-filter-menu-container");
                var textBox = filterBox.Locator("[title=Value]");
                await textBox.FillAsync(filterText);
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find the filter text box. Stopping test.");
                throw;
            }
            
        }
        public async Task ClearUserFilters()
        {
            Console.WriteLine("Clearing filters...");
            await ClickItem(UsersPageClickables.UserNameColumnMoreIcon);
            await ClickItem(FilterItemClickables.FilterMenu);
            await ClickItem(FilterItemClickables.ClearFilterButton);
        }
        
        public async Task<UserDetailPage> SelectUser(string userName)
        {
            
                await FilterGridByUserName(userName);

                var userLink = _page.Locator($"a:has-text('{userName}')");
                await userLink.ClickAsync();

                Console.WriteLine("Success!");
                await _page.WaitForLoadStateAsync(LoadState.Load);
                return new UserDetailPage(_page);
            
        }
        public async Task VerifyUserNotInGrid(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException($"Parameter userName cannot be null or empty.");
            Console.WriteLine($"Checking that user '{userName}' is not in users grid...");
            try
            {
                var row = _page.Locator($".k-master-row:has-text('{userName}')");
                await row.ScrollIntoViewIfNeededAsync(new LocatorScrollIntoViewIfNeededOptions() { Timeout=2500} );
                throw new PlaywrightException("Deleted user still exists in grid. Stopping test.");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("User not present in grid... Success!");
            }
        }
        public async Task VerifyUserStatus(string userName, UserStatuses status)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException($"Parameter userName cannot be null or empty.");
            
            Console.WriteLine($"Attempting to locate user '{userName}' and verify their status as '{status}'");
            try
            {
                var row = _page.Locator($".k-master-row:has-text('{userName}')");
                await row.Locator($":has-text('{status}')").ScrollIntoViewIfNeededAsync();                    
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Couldn't find user '{userName}' with status '{status}'");
                throw;
            }

        }
        public async Task FilterGridByUserName(string userName)
        {
            await ClearUserFilters();
            await ClickItem(UsersPageClickables.UserNameColumnMoreIcon);
            await ClickItem(FilterItemClickables.FilterMenu);
            await EnterFilterText(userName);
            await ClickItem(FilterItemClickables.FilterButton);
        }
    }
}
