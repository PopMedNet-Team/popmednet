using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        public ActionResult Details()
        {
            return View("~/Views/Tasks/Details.cshtml");
        }
    }
}