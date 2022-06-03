using Microsoft.Playwright;

namespace PopMedNet.UITests.Models
{
    public class ResourcesPage : BasePage
    {
        private readonly IPage _page;
        public ResourcesPage(IPage page)
        {
            _page = page;
        }
    }

   
}