using Microsoft.Playwright;

namespace PopMedNet.UITests.Models
{
    public class ProfilePage
    {
        private IPage page;

        public ProfilePage(IPage page)
        {
            this.page = page;
        }
    }
}