using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PopMedNet.Dns.Portal.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        public IActionResult Details()
        {
            return View();
        }
    }
}
