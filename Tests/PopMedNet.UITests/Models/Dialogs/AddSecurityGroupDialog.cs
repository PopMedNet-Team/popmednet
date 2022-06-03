using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models.Dialogs
{
    public class AddSecurityGroupDialog
    {
        private readonly IPage _page;
        private readonly IFrame _frame;
        private readonly string frameUrl = ConfigurationManager.AppSettings["baseUrl"] + "security/SecurityGroupWindow";

        public AddSecurityGroupDialog(IPage page)
        {
            _page = page;
            _frame = _page.FrameByUrl(frameUrl);
        }

        #region Private Helpers

        private async Task ClickControl(AddSecurityGroupClickables controlName)
        {
            string locator;
            // Declare locators for each control
            switch (controlName)
            {
                case AddSecurityGroupClickables.OrgColumnOptionsExpand:
                    locator = "#Organizations >> th[data-title=Organization] >> span.k-i-more-vertical";
                    break;
                case AddSecurityGroupClickables.OrgSecurityGroupsOptionsExpand:
                    locator = "#Organizations >> th[data-title='Security Groups'] >> span.k-i-more-vertical";
                    break;
                case AddSecurityGroupClickables.ClearFilters:
                    locator = ".k-i-filter-clear:visible";
                    break;
                case AddSecurityGroupClickables.FilterMenuExpand:
                    locator = ".k-i-filter:visible";
                    break;
                case AddSecurityGroupClickables.FilterButton:
                    locator = "button[title=Filter]:visible";
                    break;
                default:
                    locator = "";// do stuff
                    break;
            }
            // Log and attempt click
            Console.WriteLine($"Attempting to click '{controlName}'...");
            try
            {
                var control = _frame.Locator(locator);
                await control.ClickAsync();
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not click control '{controlName}'. Stopping test.");
                throw;
            }

        }
       
        private async Task EnterFilterText(string filterText)
        {
            Console.WriteLine("Atttempting to enter filter text...");

            try
            {
                //var filterBox = _frame.Locator("div.k-filter-menu-container");
                //var textBox = filterBox.Locator("[title=Value]:visible");
                var textBox = _frame.Locator("[title=Value]:visible");
                await textBox.FillAsync(filterText);
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find the filter text box. Stopping test.");
                throw;
            }

        }

        #endregion
        #region Public Helpers
        public async Task ClearOrganizationFilters()
        {
            Console.WriteLine("Clearing filters...");
            await ClickControl(AddSecurityGroupClickables.OrgColumnOptionsExpand);
            await ClickControl(AddSecurityGroupClickables.FilterMenuExpand);
            await ClickControl(AddSecurityGroupClickables.ClearFilters);
        }
        public async Task ClearSecurityGroupsFilters()
        {
            await ClickControl(AddSecurityGroupClickables.OrgSecurityGroupsOptionsExpand);
            await ClickControl(AddSecurityGroupClickables.FilterMenuExpand);
            await ClickControl(AddSecurityGroupClickables.ClearFilters);
        }

        public async Task FilterOrganizationsByName(string orgName)
        {
            await ClearOrganizationFilters();// Clear filters so we're starting with all orgs
            await ClickControl(AddSecurityGroupClickables.OrgColumnOptionsExpand);
            await ClickControl(AddSecurityGroupClickables.FilterMenuExpand);
            await EnterFilterText(orgName);
            await ClickControl(AddSecurityGroupClickables.FilterButton);
        }
        public async Task FilterSecurityGroupsByName(string groupName)
        {
            await ClearSecurityGroupsFilters();// Clear filters so we're starting with all orgs
            await ClickControl(AddSecurityGroupClickables.OrgSecurityGroupsOptionsExpand);
            await ClickControl(AddSecurityGroupClickables.FilterMenuExpand);
            await EnterFilterText(groupName);
            await ClickControl(AddSecurityGroupClickables.FilterButton);
        }

        public async Task SelectOrganization(string orgName)
        {
            Console.WriteLine($"Attempting to select organization '{orgName}'...");
            try
            {
                var org = _frame.Locator($"td:text-is('{orgName}')");
                await org.ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not select organization '{orgName}'. Stopping test.");
                throw;
            }
        }
        public async Task SelectSecurityGroup(string groupName)
        {
            Console.WriteLine($"Attempting to select organization security group '{groupName}'...");
            try
            {
                var group = _frame.Locator($"td:text-is('{groupName}')");
                await group.ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Unable to find group '{groupName}'. Stopping test.");
                throw;
            }
        }
        #endregion

        public async Task SetUserSecurityGroup(string orgName, string groupName)
        {
            await FilterOrganizationsByName(orgName);
            await SelectOrganization(orgName);
            await FilterSecurityGroupsByName(groupName);
            await SelectSecurityGroup(groupName);
        }
    }
}
