using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class RequestDetailsPage : BasePage
    {
        //private IPage _page;
        private readonly string commentFrameUrl = ConfigurationManager.AppSettings["baseUrl"] 
            + "controls/wfcomments/simplecomment-dialog";


        public RequestDetailsPage(IPage page)
        {
            this._page = page;
        }

        /// <summary>
        /// Wrapper for clicking objects in the RequestDetails page. Meant to streamline coding clicks
        /// and to log what is being clicked for the test record
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task ClickItem(RequestDetailClickables item)
        {
            // generate locator
            var locator = "";

            switch (item)
            {
                case RequestDetailClickables.OverviewTab:
                    locator = "a:text-is('Overview')";
                    break;
                case RequestDetailClickables.TaskTab:
                    locator = "#aTask";
                    break;
                case RequestDetailClickables.DocumentsTab:
                    locator = "#aDocuments";
                    break;
                case RequestDetailClickables.EnhancedEventLogTab:
                    locator = "#aEnhancedEventLog";
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Clickable item '{item}' has not been implemented in the page object.");
            }
            // Log
            Console.WriteLine($"Attempting to click {item} in RequestDetail page...");
            try
            {
                var control = _page.Locator(locator);
                await control.ScrollIntoViewIfNeededAsync();
                await control.ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Unable to click {item} in RequestDetail page. Stopping test.");
                throw;
            }
        }

        // TODO: Upload from FTP

        // TODO: Additional Instructions

        // TODO: Attachments: Local Upload

        // TODO: Attachments: FTP Upload

        // TODO: Cancel



        /// <summary>
        /// Clicks the 'No Comment' button, since no comment was passed.
        /// </summary>
        /// <returns></returns>
        public async Task EnterComment()
        {
            var _frame = _page.FrameByUrl(commentFrameUrl);
            try
            {
                Console.WriteLine("Attempting to click 'No Comment' button...");
                await _frame.Locator("button:has-text('No Comment')").ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find 'No Comment' button in frame. Stopping test.");
                throw;
            }
        }

        /// <summary>
        /// Enters the passed comment into the 'Comments' modal.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task EnterComment(string comment)
        {
            var _frame = _page.FrameByUrl(commentFrameUrl);
            try
            {
                Console.WriteLine("Attempting to enter comment...");
                await _frame.Locator("[name='txtComments'").FillAsync(comment);
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find comment field. Stopping test.");
                throw;
            }
            try
            {
                Console.WriteLine("Attempting to save comment...");
                await _frame.Locator("button:has-text('Save Comment')").ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find 'Save Comment' buton. Stopping test.");
                throw;
            }

        }
        public async Task SaveRequest()
        {
            try
            {
                Console.WriteLine("Attempting to click 'Save' button...");
                await _page.Locator("button:has-text('Save')").ClickAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find save button in Request Details. Stopping test.");
                throw;
            }
        }
        public async Task SelectDatamarts(string dataMartName)
        {
            try
            {
                Console.WriteLine($"Selecting DataMart {dataMartName}...");
                var row = await _page.WaitForSelectorAsync($"#dmSelectGrid tr:has(:text('{dataMartName}'))");
                var checkBox = await row.WaitForSelectorAsync("input[type='checkbox']");
                await checkBox.SetCheckedAsync(true);
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find a checkbox in a row containing {dataMartName}. Stopping test.");
                throw;
            }
        }

        /// <summary>
        /// Overload allowing a list of DataMart names to be selected
        /// </summary>
        /// <param name="dataMartNames"></param>
        /// <returns></returns>
        public async Task SelectDatamarts(List<string> dataMartNames)
        {
            foreach (var dataMartName in dataMartNames)
            {
                await SelectDatamarts(dataMartName);
            }
        }
        /// <summary>
        /// Used for File Distribution requests.
        /// Verifies the document was added to the request.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task SubmitRequest()
        {
            try
            {
                Console.WriteLine("Attempting to click 'Submit' button...");
                await _page.Locator("button:has-text('Submit')").ClickAsync();
                Console.WriteLine("Success!");
                
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find Submit button in Request Details. Stopping test.");
                throw;
            }
        }
        /// <summary>
        /// Uploads file at path *as an attachment* to the request
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task UploadAttachmentForRequest(string path)
        {
            try
            {
                Console.WriteLine($"Attempting to attach document from {path} to request...");
                await _page.SetInputFilesAsync("#attachments_upload  #files", path);
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find attachment uploader using selector '#attachments_upload #files'. Stopping test.");
                throw;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine($"Could not find the file at {path}. Stopping test.");
                throw ex;
            }
        }
        /// <summary>
        /// Used for File Distribution requests.
        /// Uploads file at path passed to the request directly, NOT as an attachment.
        /// To attach a file to the request, use UploadAttachmentForRequest().
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task UploadFilesForRequest(string resourceFileName)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory 
                + $"ResourceFiles\\{resourceFileName}";
            //var file = $"Resources\\TestDoc01.txt";
            try
            {
                System.Console.WriteLine($"Attempting to upload document '{resourceFileName}' to request...");
                await _page.SetInputFilesAsync("#FileUploadControl #files", file);
                Console.WriteLine("Success!");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Could not find uploader using selector '#FileUploadControl #files'. Stopping test.");
                throw ex;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine($"Could not find the file {file}. Stopping test.");
                throw ex;
            }
        }
        public async Task VerifyDocumentUpload(string fileName)
        {
            try
            {
                Console.WriteLine($"Verifying file '{fileName}' was added to the request...");
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await _page.Locator($"#FileUploadControl [title='{fileName}']").WaitForAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find file '{fileName}' in request. Stopping test.");
                throw;
            }
        }
        /// <summary>
        /// Verifies the attachment was uploaded to the 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task VerifyAttachmentUpload(string fileName)
        {
            try
            {
                Console.WriteLine($"Verifying file '{fileName}' was added to the request...");
                await _page.Locator($"#attachments_upload [title='{fileName}']").WaitForAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find file '{fileName}' in request. Stopping test.");
                throw;
            }
        }
        public async Task VerifyDataMartInRoutingsTable(string dataMartName)
        {
            // Navigate to Task tab
            await ClickItem(RequestDetailClickables.TaskTab);

            Console.WriteLine($"Checking for dataMart '{dataMartName}' in incomplete routings table...");
            try
            {
                var table = _page.Locator("[data-bind='foreach: IncompleteRoutings']");
                await table.Locator($":text-is('{dataMartName}')")
                    .ScrollIntoViewIfNeededAsync();
                await table.WaitForAsync(new LocatorWaitForOptions() {  Timeout=5000});
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find dataMart '{dataMartName}'. Stopping test.");
                throw;
            }
        }
        public async Task VerifyEventLogUpdate()
        {
            // Navigate to Enhanced Event Log
            await ClickItem(RequestDetailClickables.EnhancedEventLogTab);

            // TODO: Sort by Time descending

            // Check that first row description includes 'was changed from DraftReview to Submitted'
            var description = "DraftReview to Submitted";
            Console.WriteLine($"Checking for description '{description}' in log...");
            try
            {
                var table = _page.Locator("#WFEnhancedEventLog");
                await table.Locator($"td:has-text('{description}')").WaitForAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find description with '{description}' in log table. Stopping test. ");
                throw;
            }
        }
        public async Task VerifyFileUploadInDocumentsTab(string fileName)
        {
            await ClickItem(RequestDetailClickables.DocumentsTab);
            Console.WriteLine($"Checking Documents tab for document {fileName}");
            try
            {
                var grid = _page.Locator("#TaskDocuments >> section:has(div:text-is('Documents'))");
                await grid.ScrollIntoViewIfNeededAsync();
                await grid.Locator($"td:text-is('{fileName}')").IsVisibleAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find {fileName}. Stopping test.");
                throw;
            }
        }
        public async Task VerifyFileUploadInOverviewTab(string fileName)
        {
            await ClickItem(RequestDetailClickables.OverviewTab);
            Console.WriteLine($"Checking Overview Tab File Upload table for document {fileName}...");
            try
            {
                var table = _page.Locator("tbody[data-bind='DocumentsByRevision:Values.Documents']");
                await table.ScrollIntoViewIfNeededAsync();
                await table.Locator($"td:text-is('{fileName}')").WaitForAsync();
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find {fileName}. Stopping test.");
                throw;
            }

        }
        public async Task VerifyTaskUpdate()
        {
            // Navigate to Task tab
            await ClickItem(RequestDetailClickables.TaskTab);

            // Check Incomplete Routings to show "Status" is 'Submitted'
            Console.WriteLine("Searching for status 'Submitted' in Incomplete Routings table...");
            try
            {
                var table = _page.Locator("[data-bind='foreach: IncompleteRoutings']");
                await table.Locator("td:text-is('Submitted')")
                    .ScrollIntoViewIfNeededAsync();
                await table.WaitForAsync(new LocatorWaitForOptions() { Timeout = 5000 });
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find status 'Submitted' in table. Stopping test.");
                throw;
            }
        }
    }
}