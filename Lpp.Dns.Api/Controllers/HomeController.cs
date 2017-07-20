using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Api.Controllers
{
#pragma warning disable 1591
    /// <summary>
    /// Controller that services the API Home routing actions (the default).
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Services the Index action. Redirects to the Help controller's default action.
        /// </summary>
        /// <returns>Help default action result</returns>
        public ActionResult Index()
        {
            return new RedirectResult("/help");
        }
    }
#pragma warning restore 1591
}
