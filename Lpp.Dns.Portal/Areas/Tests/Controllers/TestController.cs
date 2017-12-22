using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Root.Areas.Tests.Controllers
{
    public class TestController : Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            this.View(actionName).ExecuteResult(ControllerContext);
        }
    }
}
