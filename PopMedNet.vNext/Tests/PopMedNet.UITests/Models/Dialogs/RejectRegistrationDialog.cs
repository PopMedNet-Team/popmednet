using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models.Dialogs
{
    public class RejectRegistrationDialog
    {
        private readonly IPage _page;
        private readonly IFrame _frame;
        private readonly string frameUrl = ConfigurationManager.AppSettings["baseUrl"]
            + "users/rejectregistration";
        

        public RejectRegistrationDialog(IPage page)
        {
            _page = page;
            _frame = _page.FrameByUrl(frameUrl);
        }

        public async Task EnterReason(string reason)
        {
            Console.WriteLine("Attempting to enter rejection reason...");
            try
            {
                await _frame.WaitForLoadStateAsync(LoadState.NetworkIdle);
                var reasonBox = _frame.FrameLocator("[title='Editable area. Press F10 for toolbar.']")
                    .Locator("body[contenteditable=true]");
                await reasonBox.FillAsync(reason);
                
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not enter reason for rejection. Ending test.");
                throw;
            }
        }
        public async Task ClickSave()
        {
            Console.WriteLine("Attempting to save reason...");
            try
            {
                await _frame.ClickAsync("#btnSave");
                Console.WriteLine("Success!");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Could not click 'Save' button. Ending test.");
                throw;
            }
        }
    }
}
