using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using PopMedNet.UITests.Models.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class UserDetailPage : BasePage
    {
        //private readonly IPage _page;
        public UserDetailPage(IPage page)
        {
            _page = page;
        }

        #region Private Methods (Helpers)

        private async Task ClickControl(UserDetailPageClickables controlName)
        {
            string locator = "";
            var waitForNetworkIdle = false;
            switch (controlName)
            {
                case UserDetailPageClickables.SecurityGroupsTab:
                    locator = "#tabs-tab-1";
                    break;
                case UserDetailPageClickables.PermissionsTab:
                    locator = "#tabs-tab-2";
                    break;
                case UserDetailPageClickables.AddSecurityGroupButton:
                    locator = "#btnAddSecurityGroup:visible";
                    break;
                case UserDetailPageClickables.SaveButton:
                    locator = "#btnSave";
                    break;
                case UserDetailPageClickables.OkButton:
                    locator = "#btnOK";
                    break;
                case UserDetailPageClickables.ActivateLink:
                    locator = "a:text-is('Activate')";
                    break;
                case UserDetailPageClickables.DeleteUserButton:
                    locator = "#btnDelete";
                    waitForNetworkIdle = true;
                    break;
                case UserDetailPageClickables.RejectLink:
                    locator = "a:has-text('Reject')";
                    break;
                case UserDetailPageClickables.OrganizationDropDown:
                    locator = "[title='Please select an organization'] >> .k-select";
                    break;
                case UserDetailPageClickables.ConfirmYesDialogButton:
                    locator = "#confirmYes";
                    break;
                default:
                    break;
            }
            await base.ClickItem(locator, controlName.ToString(), waitForNetworkIdle);
        }

        private async Task EnterRejectionReason(string reason)
        {
            ;
            var dialog = new RejectRegistrationDialog(_page);
            await _page.WaitForLoadStateAsync();
            await dialog.EnterReason(reason);
            await dialog.ClickSave();
        }
        private async Task SelectOrganization(string orgName)
        {
            Console.WriteLine($"Attempting to select organization '{orgName}'...");
            try
            {
                var sub = orgName.Substring(0, 2);
                var loc = _page.Locator("#cboOrganization-list");
                var org = loc.Locator($"li:has-text('{orgName}')");
                await loc.TypeAsync(sub, new LocatorTypeOptions() { Delay = 0 });
                await org.ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not locate organization '{orgName}'. Stopping test.");
                throw;
            }
        }

        #endregion

        #region Public Methods (tasks)

        public async Task AddPermissionsGroupForUser(string organization, string securityGroup)
        {
            await ClickControl(UserDetailPageClickables.PermissionsTab);
            await ClickControl(UserDetailPageClickables.AddSecurityGroupButton);
            var dialog = new AddSecurityGroupDialog(_page);
            await dialog.SetUserSecurityGroup(organization, securityGroup);
        }
        public async Task AddUserToSecurtyGroup(string organization, string securityGroup)
        {
            await ClickControl(UserDetailPageClickables.SecurityGroupsTab);
            await ClickControl(UserDetailPageClickables.AddSecurityGroupButton);
            var dialog = new AddSecurityGroupDialog(_page);
            await dialog.SetUserSecurityGroup(organization, securityGroup);
        }
        public async Task ActivateUser()
        {
            await ClickControl(UserDetailPageClickables.ActivateLink);
        }
        /// <summary>
        /// Called from User Detail page, therefore when a specific user is open.
        /// </summary>
        /// <returns></returns>
        public async Task<UsersPage> DeleteUser()
        {
            Console.WriteLine("***Attempting to delete user...");
            await ClickControl(UserDetailPageClickables.DeleteUserButton);
            //await ClickConfirmYesButtion();
            await ClickControl(UserDetailPageClickables.ConfirmYesDialogButton);
            return new UsersPage(_page);
        }

        public async Task RejectUserRequest(string reason)
        {
            Console.WriteLine("***Attempting to reject user request...");
            await ClickControl(UserDetailPageClickables.RejectLink);
            await EnterRejectionReason(reason);
        }

        public async Task SaveChanges()
        {
            Console.WriteLine("Attempting to save changes to user...");
            //await ClickSaveButton();
            await ClickControl(UserDetailPageClickables.SaveButton);
            //await ClickOkButton();
            await ClickControl(UserDetailPageClickables.OkButton);
        }

        public async Task SetUserOrganization(string orgName)
        {
            await ClickControl(UserDetailPageClickables.OrganizationDropDown);
            await SelectOrganization(orgName);
        }

        /// <summary>
        /// In UserDetailPage, checks that the status indicated is displayed.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task VerifyUserStatus(UserStatuses status)
        {
            Console.WriteLine($"Verifying user displays as '{status}'...");
            try
            {
                var statusLabel = _page.Locator($"span:has-text('{status}')");
                await statusLabel.WaitForAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not verify status. Ending test.");
                throw;
            }

        }
        #endregion
    }
}
