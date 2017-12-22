using System.Web.Mvc;

namespace Lpp.Dns.Portal.Root.Areas.Tests
{
    public class TestsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Tests";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Tests_default",
                "Tests/{controller}/{action}",
                new { controller="Test", action = "Index" }
            );
        }
    }
}
