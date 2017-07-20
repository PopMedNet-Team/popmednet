using Lpp.Dns.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Controllers
{
    public class SecurityController : Controller
    {
        [ChildActionOnly]
        public ActionResult EditAcl(EditAclModel model)
        {
            return View(model);
        }

        public ActionResult SelectSecurityGroup()
        {
            return View();
        }
        public ActionResult SecurityGroupWindow()
        {
            return View();
        }
    }
}