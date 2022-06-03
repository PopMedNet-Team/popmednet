using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.UITests.Models
{
    public class SharedControls
    {
        private readonly IPage _page;

        public SharedControls()
        {

        }

        public SharedControls(IPage page)
        {
            _page = page;
        }

        public async Task<HomePage> GoToHome()
        {
            await _page.ClickAsync("text=Home");
            return new HomePage(_page);
        }

        public async Task<RequestsPage> GoToRequests()
        {
            await _page.ClickAsync("text=Requests"); //TODO: refine the query so it just clicks on the menu item, not some random other "Requests" text
            return new RequestsPage(_page);
        }

        public async Task<RequestsPage> GoToRequests(string projectName)
        {
            // TODO: Set it up so it goes to the correct menu item inside "Requests"
            throw new NotImplementedException("This isn't set up yet.");
            
            //return new RequestsPage(_page);
        }

        public async Task<ProfilePage> GoToProfile()
        {
            await _page.ClickAsync("text=Profile");
            return new ProfilePage(_page);
        }

        public async Task<ResourcesPage> GoToResources()
        {
            await _page.ClickAsync("text=Resources");
            return new ResourcesPage(_page);
        }

        // TODO: GoToDataMartAuditReport, GoToNetworkActivityReport

        // TODO: All Network menu items

        public async Task GoToContactUs()
        {
            await _page.ClickAsync("text=Contact Us");

        }

        public async Task<LoginPage> LogOff()
        {
            await _page.ClickAsync("text=Log Off");
            return new LoginPage(_page);
        }
        
    }
}
