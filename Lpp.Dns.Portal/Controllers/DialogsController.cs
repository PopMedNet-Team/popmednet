using Lpp.Utilities.WebSites.Filters;
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

        [NoCookieReset, AllowAnonymous]
        public ActionResult SessionExpiring()
        {
            return View();
        }
    }
}