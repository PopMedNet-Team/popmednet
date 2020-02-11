using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lpp.Utilities.WebSites.Attributes
{
    public class RemoteRequireHttpsAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.Request.IsLocal || filterContext.HttpContext.Request.IsSecureConnection || (filterContext.HttpContext.Request.UserHostAddress != null && 
                (filterContext.HttpContext.Request.UserHostAddress.StartsWith("192.168") ||
                filterContext.HttpContext.Request.UserHostAddress.StartsWith("10.") ||
                filterContext.HttpContext.Request.UserHostAddress.StartsWith("127.0.0"))
                ))
                return;

            base.OnAuthorization(filterContext);
        }
    }
}
