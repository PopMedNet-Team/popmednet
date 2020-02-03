using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Controllers
{
    public class DialogsController : BaseController
    {
        public ActionResult CodeSelector()
        {
            return View();
        }

        public ActionResult EditRoutingStatus()
        {
            return View();
        }

        public ActionResult PredefinedLocationSelector()
        {
            return View();
        }

        public ActionResult MetadataBulkEditPropertiesEditor()
        {
            return View();
        }

        public ActionResult RoutingHistory()
        {
            return View();
        }
   
    }
}