using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PopMedNet.Dns.Portal.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult CreateDialog()
        {
            return View();
        }
    }
}
