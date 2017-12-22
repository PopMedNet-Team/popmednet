using System.Web.Mvc;

namespace Lpp.Dns.Portal.Root.Areas.Api
{
    public class ApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Api";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Api_default",
                "api/{controller}/{action}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
