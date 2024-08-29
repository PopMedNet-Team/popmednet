using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PopMedNet.Dns.Portal.Hubs;
using PopMedNet.Dns.Portal.Models;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;

namespace PopMedNet.Dns.Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly Utilities.WebSites.BackgroundWorkerQueue _backgroundWorkerQueue;

        public HomeController(IConfiguration config, ILogger<HomeController> logger, Utilities.WebSites.BackgroundWorkerQueue backgroundWorkerQueue)
        {
            _configuration = config;
            _logger = logger;
            _backgroundWorkerQueue = backgroundWorkerQueue;
        }

        [Route("~/login")]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            //TODO: reset any existing authentication cookie and session values
            if (User.Identity!.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect("~" + returnUrl);
                }

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            if (Request.Cookies.ContainsKey("Authorization"))
            {
                Response.Cookies.Append("Authorization", "", new CookieOptions { Expires = DateTime.MinValue, HttpOnly = false, IsEssential = true });
            }

            //TODO: check for SSO configuration, if so redirect to SSO
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, Route("~/login")]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl)
        {
            using(var web = new HttpClient())
            {
                var result = await web.PostAsJsonAsync($"{ _configuration.GetValue<string>("ServiceUrl") }users/validatelogin", new DTO.LoginDTO { UserName = username, Password = password, Environment = "Portal vNext", IPAddress = "localhost", RememberMe = false });
                
                if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("Error", await result.Content.ReadAsStringAsync());
                    return View();
                }
                
                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("Error", "Error communicating with authentication service.");
                    return View();
                }

                var u = await result.Content.ReadFromJsonAsync<DTO.LoginResultDTO>();
                //var identity = new GenericIdentity(u!.FullName!, CookieAuthenticationDefaults.AuthenticationScheme);
                var identity = new Utilities.Security.ApiIdentity(u!.ID, u.UserName, u.FullName, u.OrganizationID);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new System.Security.Claims.ClaimsPrincipal(identity), authProperties);                

                //set a PMN informational authentication cookie that contains information about the user
                var cookieContent = System.Text.Json.JsonSerializer.Serialize(new PopMedNet.Utilities.WebSites.Models.LoginResponseModel(HttpContext, u.ID, u.UserName, password, u.OrganizationID, u.PasswordExpiration, _configuration.GetValue<int>("SessionExpireMinutes")));
                
                Response.Cookies.Append("Authorization", cookieContent, new CookieOptions { Expires = DateTime.Now.AddHours(5), HttpOnly = false, SameSite= SameSiteMode.Lax, Secure = true, IsEssential=true  });

                return Redirect(string.IsNullOrEmpty(returnUrl) ? "~/" : returnUrl);

            }
        }

        [Route("~/logout"), Authorize]
        public async Task<IActionResult> Logout(string? returnUrl, [FromServices] IHubContext<SessionHub> sessionHub)
        {
            string? username = User.Identity.Name;

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("Authorization");

            if (!string.IsNullOrEmpty(username))
            {
                _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await sessionHub.Clients.Groups(username).SendAsync("onLogout");
                });
            }
            

            return Redirect(string.IsNullOrEmpty(returnUrl) ? "~/login" : "~/login?returnUrl=" + returnUrl);
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("~/termsandconditions")]
        public IActionResult TermsAndConditions()
        {
            return View();
        }
        [Route("~/info")]
        public IActionResult Info()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize, Route("~/resources"), Route("~/Home/resources")]
        public IActionResult Resources()
        {
            return View();
        }

        [Authorize, Route("~/defaultsecuritypermissions")]
        public IActionResult DefaultSecurityPermissions()
        {
            return View();
        }

        public IActionResult UserRegistration()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult RestorePassword(string token)
        {
            //make sure there is a valid token, else redirect away
            Guid tokn;
            if (!Guid.TryParse(token, out tokn))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet("~/touch"), Authorize]
        public async Task<IActionResult> Touch([FromServices] IHubContext<Hubs.SessionHub> sessionHub)
        {
            if (User.Identity.IsAuthenticated)
            {
                string? username = User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
                    {
                        await sessionHub.Clients.Groups(username).SendAsync("onSessionRefresh", DateTime.UtcNow);
                    });
                }

                return Ok();
            }

            return BadRequest(new { Error = "User is not authenticated." });
        }
    }
}