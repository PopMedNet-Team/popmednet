using Lpp.Utilities.WebSites;
using Lpp.Utilities.WebSites.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Sso.Controllers
{
    public class HomeController : Controller
    {
        [LppDnsAuthorize, RemoteRequireHttps, NoCache]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult TermsAndConditions()
        {
            return View("_TermsAndConditions");
        }

        public ActionResult Info()
        {
            return View("_Info");
        }
    }
}