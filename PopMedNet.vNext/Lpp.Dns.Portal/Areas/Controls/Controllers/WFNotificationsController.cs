using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Areas.Controls.Controllers
{
    public class WFNotificationsController : Controller
    {
        // GET: Controls/WFNotifications
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUsers()
        {
            return View();
        }
    }
}