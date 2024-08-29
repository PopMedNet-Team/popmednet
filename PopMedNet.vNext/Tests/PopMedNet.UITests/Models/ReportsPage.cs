using Microsoft.Playwright;

namespace PopMedNet.UITests.Models
{
    public class ReportsPage
    {
        private IPage page;

        public ReportsPage(IPage page)
        {
            this.page = page;
        }
    }
}