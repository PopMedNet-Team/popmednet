using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using Lpp.Mvc;

namespace $safeprojectname$.Controllers
{
    [CompiledViews(typeof($safeprojectname$.Views.$PartName$.Index))]
	public class $PartName$Controller : Controller
	{
        public ActionResult Index()
        {
            return View( new Models.$PartName$Model { Message = "Hello from $PartName$!" } );
        }
	}
}
