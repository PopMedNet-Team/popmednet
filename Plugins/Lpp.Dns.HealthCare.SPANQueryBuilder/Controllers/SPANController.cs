using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using Lpp.Mvc;

namespace Lpp.Dns.Healthcare.SPAN.Controllers
{
    [CompiledViews(typeof(Lpp.Dns.Healthcare.SPAN.Views.Display))]
    public class SPANController : Controller
    {
        public ActionResult Index()
        {
            return View(new Models.SPANModel { Message = "Hello from MyPlugin!" });
        }

        public ActionResult Create()
        {
            return View(new Models.SPANModel { Message = "Create View!" }); ;
        }

        public ActionResult Display()
        {
            return View(new Models.SPANModel { Message = "Display View!" });
        }
    }
}
