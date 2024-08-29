using System.Web.Mvc;

namespace Lpp.Dns.Api.Areas.Diagnostics
{
    public class DiagnosticsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Diagnostics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Diagnostics_default",
                "Diagnostics/{action}/{id}",
                new { controller = "Diagnostics", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}