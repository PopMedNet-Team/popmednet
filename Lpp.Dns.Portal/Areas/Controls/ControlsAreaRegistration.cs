using System.Web.Mvc;

namespace Lpp.Dns.Portal.Areas.Controls
{
    public class ControlsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Controls";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Controls_default",
                "Controls/{controller}/{action}/{id}",
                new { controller = "Controls", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
