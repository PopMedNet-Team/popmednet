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
            await _frame.Locator($"text={requestType}").ClickAsync();
        }
    }
}
