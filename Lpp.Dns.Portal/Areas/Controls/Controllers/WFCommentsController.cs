using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Areas.Controls.Controllers
{
    public class WFCommentsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("addcomment-dialog")]
        public ActionResult AddCommentDialog()
        {
            return View();
        }

        [HttpGet]
        [ActionName("simplecomment-dialog")]
        public ActionResult SimpleCommentDialog()
        {
            return View();
        }
    }
}