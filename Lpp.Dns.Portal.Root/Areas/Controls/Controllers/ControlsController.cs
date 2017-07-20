using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using Newtonsoft.Json;

namespace Lpp.Dns.Portal.Root.Areas.Controls.Controllers
{
    public class ControlsController : Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            this.View(actionName).ExecuteResult(ControllerContext);
        }
    }
}
