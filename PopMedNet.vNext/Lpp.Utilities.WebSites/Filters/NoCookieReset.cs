using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Lpp.Utilities.WebSites.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NoCookieReset : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Cookies.Remove(FormsAuthentication.FormsCookieName);
        }
    }
}
