using System.Web.Http;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker
{
    public class DataCheckerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataChecker";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DataChecker_default",
                "DataChecker/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
