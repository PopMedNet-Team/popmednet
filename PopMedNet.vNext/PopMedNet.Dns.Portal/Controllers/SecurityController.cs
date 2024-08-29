using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PopMedNet.Dns.Portal.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        public IActionResult SelectSecurityGroup()
        {
            return View();
        }
        public IActionResult SecurityGroupWindow()
        {
            return View();
        }
    }
}
