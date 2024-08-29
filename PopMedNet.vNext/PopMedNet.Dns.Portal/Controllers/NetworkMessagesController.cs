using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PopMedNet.Dns.Portal.Controllers
{
    [Authorize]
    public class NetworkMessagesController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
