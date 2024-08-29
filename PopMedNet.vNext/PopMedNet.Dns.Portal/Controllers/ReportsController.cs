using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PopMedNet.Dns.Portal.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        public IActionResult NetworkActivityReport()
        {
            return View();
        }

        public IActionResult DataMartAuditReport()
        {
            return View();
        }
    }
}
