using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class ChooseRequestTypeDialog
    {
        private readonly IPage _page; 
        private readonly IFrame _frame;
        private readonly string frameUrl = ConfigurationManager.AppSettings["baseUrl"] + "requests/createdialog";

        public ChooseRequestTypeDialog(IPage page)
        {
            _page = page;
            _frame = _page.FrameByUrl(frameUrl);
        }

        public async Task SelectRequestType(string requestType)
        {
            Console.WriteLine($"Attempting to select request type {requestType}...");
            var retries = 5;
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    await _frame.Locator($"text={requestType}").ClickAsync();
                    Console.WriteLine("Success!");
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Could not select request type. {retries-i-1} retries remaining.");
                }
            }
            
        }

        public async Task WaitForFrame()
        {
            
            await _frame.Locator("section #Content").IsVisibleAsync(new LocatorIsVisibleOptions { Timeout = 500 });
        }
    }
}
