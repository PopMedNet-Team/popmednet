using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PopMedNet.Dns.Portal.Controllers
{
    [Authorize]
    public class SecurityGroupsController : Controller
    {
        public IActionResult Details()
        {
            return View();
        }
    }
}
