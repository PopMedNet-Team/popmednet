using System.Web.Mvc;

namespace Lpp.Dns.Portal.Root.Areas.Controls
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
                "{controller}/{action}",
                new { controller="Controls", action = "Index" }
            );
        }
    }
}
