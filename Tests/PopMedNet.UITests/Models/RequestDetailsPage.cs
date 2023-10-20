using Microsoft.Playwright;
using PopMedNet.UITests.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
                    locator = "a:has-text('Task:')";
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
            var retries = 5;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    var control = _page.Locator(locator);
                    await control.IsVisibleAsync();
                    await control.ClickAsync(new LocatorClickOptions { Timeout = 5000 });
                    Console.WriteLine("\tSuccess!");
                    return;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine($"\tCould not click item. Will retry {retries - 1 - i} more times.");
                }
            }
            Console.WriteLine($"Unable to click {item} in RequestDetail page. Stopping test.");
            throw new TimeoutException();

        }

        // TODO: Upload from FTP

        // TODO: Additional Instructions

        // TODO: Attachments: Local Upload

        // TODO: Attachments: FTP Upload

        // TODO: Cancel

        public async Task<string> GetRequestId()
        {
            var url = await GetUrl();
            return url.Split('=')[1];
        }

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
                Console.WriteLine("\tSuccess!");
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
                Console.WriteLine("\tSuccess!");
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
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find 'Save Comment' buton. Stopping test.");
                throw;
            }

        }

        public async Task ExpandCompletedRouting(string dataMartName = "")
        {
            Console.WriteLine("Attempting to expand completed routing...");
            try
            {
                await _page.Locator(".k-i-plus-sm").ClickAsync(new LocatorClickOptions { Timeout = 1000 });
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                if (await _page.Locator(".k-i-minus-sm").IsVisibleAsync())
                {
                    Console.WriteLine("Completed routing seems to be expanded. Continuing...");
                    return;
                }
                Console.WriteLine("\tCould not expand routing. Stopping test.");
                throw;
            }
        }

        public async Task SaveRequest()
        {
            try
            {
                Console.WriteLine("Attempting to click 'Save' button...");
                await _page.Locator("button:has-text('Save')").ClickAsync();
                Console.WriteLine("\tSuccess!");
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
                Console.WriteLine("\tSuccess!");
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
                Console.WriteLine($"Adding dataMart: {dataMartName}");
                await SelectDatamarts(dataMartName);
            }
        }
        public async Task SortGrid(RequestDetailClickables gridName, ColumnNames columnTitle, bool descending = false)
        {
            var gridLocator = "";
            var columnLocator = "";
            switch (gridName)
            {
                case RequestDetailClickables.EnhancedEventLogGrid:
                    gridLocator = "#gEnhancedEventLog";
                    break;
                default:
                    throw new NotImplementedException($"Attempted to call 'SortGrid' with grid {gridName}, " +
                        $"which is not implemented. Check that the correct control was passed.");

            }
            switch (columnTitle)
            {
                case ColumnNames.Time:
                    columnLocator = "Time edit column settings";
                    break;
                default:
                    throw new NotImplementedException($"Attempted to call 'SortGrid' with column {columnTitle}, " +
                       $"which is not implemented. Check that the correct control was passed.");
            }
            await base.SortGrid(gridLocator: gridLocator, columnName: columnLocator, descending: descending);
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
                Console.WriteLine("\tSuccess!");

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
                Console.WriteLine("\tSuccess!");
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
                var fileChooser = await _page.RunAndWaitForFileChooserAsync(async () =>
                    {
                        await _page.Locator("#Normal_FileUpload .k-upload-button").ClickAsync();
                    });
                await fileChooser.SetFilesAsync(file);
                //await _page.SetInputFilesAsync("#Normal_FileUpload #files", file);
                Console.WriteLine("\tChecking that file was fully uploaded...");
                await VerifyDocumentUpload(resourceFileName);
                //await VerifyControlIsVisibleWithRetries($".k-file-success > [title={resourceFileName}]");
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Could not find uploader using selector '#Normal_FileUpload #files'. Stopping test.");
                throw ex;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine($"Could not find the file {file}. Stopping test.");
                throw ex;
            }
        }

        /// <summary>
        /// Checks that a control corresponding to the selector passed is visible.
        /// If not, it retries the check with a delay between each attempt.
        /// </summary>
        /// <param name="selector">Selector for control</param>
        /// <param name="retries">Times to retry. Default is 5.</param>
        /// <param name="delay">Delay between tries. Default is 500ms.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="TimeoutException"></exception>
        public async Task VerifyControlIsVisibleWithRetries(string selector, int retries = 5, int delay = 500)
        {
            if (string.IsNullOrWhiteSpace(selector))
                throw new ArgumentNullException("Empty selector passed to verify control. Stopping test.");
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    System.Threading.Thread.Sleep(delay);
                    await _page.Locator(selector).IsVisibleAsync();
                    Console.WriteLine("\tSuccess!");
                    return;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine($"Could not find {selector}. {retries - i} attempts remain.");
                }
            }
            {
                throw new TimeoutException($"Could not find {selector}. Stopping test.");
            }
        }

        public async Task ViewResponse(string responseName = "Response 1")
        {
            // click response
            await _page.Locator($"a:text-is('{responseName}')").ClickAsync();

            // Click Response Detail
            await _page.Locator("[href='#responsedetail_0']").ClickAsync();
            System.Threading.Thread.Sleep(5000);
        }
        /// <summary>
        /// Use to verify a file upload was successful *immediately after upload*
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task VerifyDocumentUpload(string fileName)
        {
            Debug.WriteLine("VerifyDocumentUpload");
            try
            {
                Console.WriteLine($"Verifying file '{fileName}' was added to the request...");
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                await _page.Locator($".k-file-success:has-text('{fileName}')").WaitForAsync();
                Console.WriteLine("\tSuccess!");
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
            Debug.WriteLine($"Method: VerifyAttachmentUpload({fileName}");
            Console.WriteLine($"Verifying file '{fileName}' was added to the request...");
            try
            {
                await _page.Locator($"#attachments_upload [title='{fileName}']").WaitForAsync();
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"\tCould not find file '{fileName}' in request. Stopping test.");
                throw;
            }
        }

        /// <summary>
        /// Overload to accept a list of datamart names
        /// </summary>
        /// <param name="dataMartList"></param>
        /// <returns></returns>
        public async Task VerifyDataMartInRoutingsTable(List<string> dataMartList)
        {
            Console.WriteLine($"DataMarts to verify: {dataMartList.Count}");

            foreach (var dm in dataMartList)
            {
                Console.WriteLine(dm);
                await VerifyDataMartInRoutingsTable(dm);
            }
        }

        public async Task VerifyDataMartInRoutingsTable(string dataMartName)
        {
            if (String.IsNullOrEmpty(dataMartName))
            {
                Console.WriteLine($"'dataMartName' was null or empty. Skipping the rest of this verification for now...");
                //   throw new ArgumentNullException($"'dataMartName' was null or empty. Skipping the rest of this verification...");
                return;
            }
            Debug.WriteLine($"VerifyDataMartInRoutingsTable({dataMartName}");

            // Handle flakiness for this test...

            var retries = 5;
            for (int i = retries; i > 0; i--)
            {

                Console.WriteLine($"Checking for dataMart '{dataMartName}' in Incomplete Routings table...");
                try
                {
                    // Navigate to Task tab
                    await ClickItem(RequestDetailClickables.TaskTab);
                    var panel = _page.Locator("article:has-text('Incomplete Routings')");
                    await panel.Locator($"td:has-text('{dataMartName}')")
                        .ScrollIntoViewIfNeededAsync();
                    return;
                }
                catch (TimeoutException e)
                {
                    if (i - 1 > 0)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                        Console.WriteLine($"Could not verify {dataMartName} in request. Will attempt {i - 1} more times.");
                    }
                    else
                    {
                        Console.WriteLine($"\tCould not find dataMart '{dataMartName}'. Stopping test.");
                        throw e;
                    }
                }
            }
        }
        public async Task VerifyEventLogUpdate()
        {
            await VerifyEventLogUpdate("");
        }

        public async Task VerifySignatureFileInformation()
        {
            Console.WriteLine("Attempting to verify signature file information...");
            await ClickItem(RequestDetailClickables.TaskTab);
            var table = _page.Locator("[data-bind='foreach:ResponseTerms']");
            await table.IsVisibleAsync();
            //TODO: Determine whether we need to verify the actual data in the table, and what that should be
        }

        /// <summary>
        /// Opens the EnhancedEventLogTab, sorts the grid by Time (descending),
        /// Then verifies the first (i.e., most recent) row has the expected string.
        /// Empty string or null passed results in the string "DraftReview to Submitted" being
        /// searched for.
        /// </summary>
        /// <param name="keywords">String to search for (e.g, "DraftReview to Submitted")</param>
        /// <returns></returns>
        public async Task VerifyEventLogUpdate(string keywords)
        {
            // Navigate to Enhanced Event Log
            await ClickItem(RequestDetailClickables.EnhancedEventLogTab);

            // TODO: Sort by Time descending

            // Sort by Time desc
            await SortGrid(RequestDetailClickables.EnhancedEventLogGrid, ColumnNames.Time, descending: true);

            if (string.IsNullOrWhiteSpace(keywords))
                keywords = "DraftReview to Submitted";
            Console.WriteLine($"Checking for description '{keywords}' in log...");
            try
            {
                var table = _page.Locator("#WFEnhancedEventLog tbody");
                var firstRow = table.Locator("tr >> nth=0");
                await firstRow.Locator($"td:has-text('{keywords}')").WaitForAsync();
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find description with '{keywords}' in log table. Stopping test. ");
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
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find {fileName}. Stopping test.");
                throw;
            }
        }
        public async Task VerifyFileUploadInOverviewTab(string fileName)
        {
            Console.WriteLine($"Checking Overview Tab File Upload table for document {fileName}...");
            var retries = 5;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    await ClickItem(RequestDetailClickables.OverviewTab);
                    await _page.Locator("#Overview:visible").WaitForAsync();
                    var table = _page.Locator("tbody[data-bind='DocumentsByRevision:Values.Documents']").Last;
                    await table.ScrollIntoViewIfNeededAsync(new LocatorScrollIntoViewIfNeededOptions { Timeout=1000});
                    await table.Locator($"td:text-is('{fileName}')").WaitForAsync();
                    Console.WriteLine("\tSuccess!");
                    return;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine($"\tCould not locate item in table. Will retry {retries-1-i} more times.");
                    await ClickItem(RequestDetailClickables.TaskTab);                    
                }
            }
            var msg = $"\tCould not find {fileName}. Stopping test.";
            throw new TimeoutException(msg);
        }
        public async Task VerifyResponseDocuments(List<string> fileNames)
        {
            await ExpandCompletedRouting();
            await ViewResponse();
            foreach (var fileName in fileNames)
                await VerifyResponseDocuments(fileName);
        }
        public async Task VerifyResponseDocuments(string fileName)
        {
            Console.WriteLine($"Searching for {fileName} in Response Documents table...");
            
            try
            {
                var area = _page.FrameLocator("#responsedetailframe_1");
                await area.Locator($"a:text-is('{fileName}')").WaitForAsync();
                Console.WriteLine("\tSuccess!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"could not find {fileName}. Stopping test.");
                throw;
            }
        }

        public async Task VerifyResultsStatusInOverviewResultsTable(string expectedStatus = "Completed")
        {
            await ClickItem(RequestDetailClickables.OverviewTab);

            Console.WriteLine($"searching for status '{expectedStatus}' in Results table...");
            try
            {
                var table = _page.Locator("[data-bind='foreach: AllRoutings']");
                await table.WaitForAsync(new LocatorWaitForOptions() { Timeout = 5000 });
                await table.Locator($"td:text-is('{expectedStatus}')")
                    .ScrollIntoViewIfNeededAsync();
            }
            catch (TimeoutException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine($"Could not find status '{expectedStatus}' in table. Stoppingg test.");
                throw e;
            }
        }

        public async Task VerifyTaskStatusInRoutingsTable(string expectedStatus = "Completed")
        {
            await ClickItem(RequestDetailClickables.TaskTab);

            Console.WriteLine($"searching for status '{expectedStatus}' in Completed Routings table...");
            try
            {
                var table = _page.Locator("[data-bind='foreach: VirtualRoutings']");
                await table.Locator($"td:text-is('{expectedStatus}')")
                    .ScrollIntoViewIfNeededAsync();
                await table.WaitForAsync(new LocatorWaitForOptions() { Timeout = 5000 });
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"Could not find status '{expectedStatus}' in table. Stoppingg test.");
                throw;
            }
        }

        public async Task VerifyTaskUpdateInRoutingsTable(List<string> dataMarts)
        {
            foreach (var dm in dataMarts)
                await VerifyTaskUpdateInRoutingsTable(dm);
        }
        public async Task VerifyTaskUpdateInRoutingsTable(string dataMart = "")
        {
            // Navigate to Task tab
            await ClickItem(RequestDetailClickables.TaskTab);

            // Check Incomplete Routings to show "Status" is 'Submitted'
            Console.WriteLine("Searching for status 'Submitted' in Incomplete Routings table...");
            try
            {
                var row = _page.Locator($"[data-bind='foreach: IncompleteRoutings'] >> tr:has-text('{dataMart}')");
                await row.Locator("td:text-is('Submitted')")
                    .ScrollIntoViewIfNeededAsync();
                await row.WaitForAsync(new LocatorWaitForOptions() { Timeout = 5000 });
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not find status 'Submitted' in table. Stopping test.");
                throw;
            }
        }

    }
}