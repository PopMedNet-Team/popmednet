using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Controllers
{
    public class QlikTestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Requests()
        {
            return View("~/Views/QlikTest/Requests.cshtml");
        }

        public ActionResult DivIntegration()
        {
            return View();
        }
    }
}