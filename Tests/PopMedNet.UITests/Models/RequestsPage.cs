using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;

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
    public class RequestsPage : BasePage
    {       
        private readonly string editMetaDataUrl = "/workflow/workflowrequests/editwfrequestmetadatadialog";
        public RequestsPage(IPage page)
        {
            _page = page;
        }
        public async Task ClickItem(RequestsPageClickables item, string extraText="")
        {
            Console.WriteLine($"Attempting to click {item}...");
            var locator = "";
            switch (item)
            {
                case RequestsPageClickables.NewRequestButton:
                    locator = "#ddNewRequest";
                    break;
                case RequestsPageClickables.ProjectFromNewRequestButton:
                    locator = $"css=[aria-labelledby='ddNewRequest'] >> text={extraText}";
                    break;
                case RequestsPageClickables.ClearAllFilters:
                    locator = "li[role=menuitem] >> .k-i-filter-clear:visible";
                    break;
                // TODO: Break Grid controls into separate class
                case RequestsPageClickables.GridFilterButton:
                    locator = "button[title='Filter']:visible";
                    break;
                case RequestsPageClickables.GridFilterMenu:
                    locator = "ul[data-role=menu]:visible >> span:text-is('Filter')";
                    break;
                case RequestsPageClickables.GridMoreIcon:
                    locator = "[data-field=Name] >> span.k-i-more-vertical";
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Clickable item '{item}' has not been implemented in Requests Page test object.");

            }
            try
            {
                await _page.Locator(locator).ClickAsync();
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"\tCould not click item {item}. Stopping test.");
                throw;
            }
        }
        public async Task<ChooseRequestTypeDialog> CreateNewRequestForProject(string projectName)
        {
            await ClickItem(RequestsPageClickables.NewRequestButton);
            await ClickItem(RequestsPageClickables.ProjectFromNewRequestButton, projectName);
            return new ChooseRequestTypeDialog(_page);
        }
        public async Task<EditRequestMetadataDialog> ChooseRequestType(string requestType)
        {
            try
            {
                var requestTypeDialog = new ChooseRequestTypeDialog(_page);
                
                await requestTypeDialog.SelectRequestType(requestType);
                await _page.Locator($"[data-url='{editMetaDataUrl}']").WaitForAsync();
                Console.WriteLine("Success!");
                return new EditRequestMetadataDialog(_page);
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Could not find request type '{requestType}'. Canceling test.");
                throw ex;
            }
        }
        /// <summary>
        /// Creates a generic FD request, via the UI. Uses default settings from App.config for
        /// the project name, request type, a test zip file, and dataMart name.
        /// </summary>
        /// <param name="requestName"></param>
        /// <returns></returns>
        public async Task<string> GenerateGenericRequest(string requestName)
        {
            // Request Data
            var projectName = ConfigurationManager.AppSettings["projectName"];
            var requestType = ConfigurationManager.AppSettings["requestType"];
            var testZip = $"{ConfigurationManager.AppSettings["testZipFile"]}";
            var dataMartName = ConfigurationManager.AppSettings["dataMart"];

            var metaData = new RequestMetadataDTO()
            {
                Name = requestName
            };

            await CreateNewRequestForProject(projectName);
            // New Request dialog
            var dialog = await ChooseRequestType(requestType);
            await dialog.FillRequestMetadata(metaData);
            var details = await dialog.Save();

            // Request Details page
            await details.UploadFilesForRequest(testZip);

            await details.SelectDatamarts(dataMartName);
            await details.SubmitRequest();
            await details.EnterComment();
            var requestUrl = _page.Url;
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            // Request details page closes and returns to Requests Page (grid)
            await base.GoToPage(PageModels.Requests);
            return requestUrl;
        }
        public async Task FilterByRequestName(string requestName)
        {   // TODO: Create new GridFilter class to handle all grid interactions         
            await ClickItem(RequestsPageClickables.GridMoreIcon);
            await ClickItem(RequestsPageClickables.GridFilterMenu);            
            var input = "div.k-filterable:visible >> input[title=Value]";
            await _page.Locator(input).FillAsync(requestName);// TODO: wrap it
            await ClickItem(RequestsPageClickables.GridFilterButton);
        }
        public async Task<RequestDetailsPage> FindRequest(string requestName)
        {
            try
            {
                Console.WriteLine($"Verifying request {requestName} is visible in table...");
                await _page.Locator($".k-master-row:has-text('{requestName}')").WaitForAsync();
                Console.WriteLine("Success!");
                return new RequestDetailsPage(_page);
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find request named '{requestName}' in Requests table.");
                throw;
            }
        }
        public async Task<string> GetRequestId(string requestName)
        {
            await FilterByRequestName(requestName);
            Console.WriteLine($"Attempting to get RequestID of '{requestName}'...");
            try
            {
                var requestLink = _page.Locator($"a:text-is('{requestName}')");
                var url = await requestLink.EvaluateAsync("link => link.href");
                var id = url.ToString().Split('=');
                Console.WriteLine($"The ID of '{requestName}' is {id[1]}");
                return id[1];
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not get the ID. Stopping test.");
                throw;
            }
        }

        /// <summary>
        /// Navigates to the Requests page directly via URL, rather than menu buttons
        /// </summary>
        /// <returns></returns>
        public async Task Goto()
        {
            var requestsUrl = ConfigurationManager.AppSettings["baseUrl"] + "requests/";
            await _page.GotoAsync(requestsUrl);
        }

        public async Task<RequestDetailsPage> GoToRequest(Guid requestId)
        {
            Console.WriteLine($"Navigating to request id {requestId}...");
            try
            {
                var requestsUrl = ConfigurationManager.AppSettings["baseUrl"] + 
                    $"requests/details?ID={requestId}";
                await _page.GotoAsync(requestsUrl);
                Console.WriteLine("\tSuccess!");
                return new RequestDetailsPage(_page);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Navigation failes.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                throw;
            }
        }
        /// <summary>
        /// Opens a request by locating it in the Requests table and clicking it.
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public async Task<RequestDetailsPage> OpenRequest(string requestName, string projectName="")
        {
            if (!string.IsNullOrEmpty(projectName))
                await SelectProjectTab(projectName);
            await FilterByRequestName(requestName);
            try
            {
                Console.WriteLine($"Verifying request {requestName} is visible in table...");
                await _page.Locator($"a:text-is('{requestName}')").ClickAsync();
                Console.WriteLine("Success!");
                return new RequestDetailsPage(_page);
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find request named '{requestName}' in Requests table.");
                throw;
            }
        }
        public async Task SelectProjectTab(string tabName)
        {
            try
            {
                Console.WriteLine($"Attempting to click tab '{tabName}'...");
                await _page.Locator($".nav-tabs li:has-text('{tabName}')").ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find tab '{tabName}'. Stopping test.");
                throw;
            }
        }
        /// <summary>
        /// Gets the given request's status as displayed in the Requests grid.
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task VerifyRequestStatus(string requestName, RequestStatuses status)
        {
            // Filter the grid for the request name
            await FilterByRequestName(requestName);
            // Verify the status matches expected
            Console.WriteLine($"Verifying {requestName} has a status of {status}...");
            try
            {
                var row = _page.Locator($"tr:has(td:text-is('{requestName}'))");
                //await row.Locator($"td:text-is('{status}')").IsVisibleAsync();
                await row.Locator($"[data-field='StatusText']:text-is('{status}')").IsVisibleAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not verify the status. Stopping test.");
                throw;
            }
            
        }

    }
}
