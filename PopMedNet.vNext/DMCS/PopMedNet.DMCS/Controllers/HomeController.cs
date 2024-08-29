using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code;
using PopMedNet.DMCS.Data.Model;
using PopMedNet.DMCS.Models;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Controllers
{
    
    public class HomeController : Controller
    {
        readonly UserManager<Data.Identity.IdentityUser> _userManager;
        readonly SignInManager<Data.Identity.IdentityUser> _signInManager;
        readonly ModelContext _modelDb;
        readonly ILogger _logger;
        readonly IOptions<DMCSConfiguration> _config;
        readonly IThemeManager _themeManager;
        readonly Services.BackgroundWorkerQueue _backgroundWorkerQueue;
        readonly IHubContext<SessionHub> _sessionHub;
        readonly string[] NoNotifyActions = new[] { "Logout", "Login", "Touch" };

        public HomeController(UserManager<Data.Identity.IdentityUser> userManager, 
                              SignInManager<Data.Identity.IdentityUser> signInManager, 
                              ModelContext modelDb, 
                              ILogger logger, 
                              IOptions<DMCSConfiguration> config,
                              ThemeManager themeManager,
                              Services.BackgroundWorkerQueue backgroundWorkerQueue,
                              IHubContext<SessionHub> sessionHub
                              )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger.ForContext("SourceContext", typeof(HomeController).FullName);
            this._config = config;
            this._modelDb = modelDb;
            this._themeManager = themeManager;
            this._backgroundWorkerQueue = backgroundWorkerQueue;
            this._sessionHub = sessionHub;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            controller.ViewBag.NetworkName = _themeManager.Title;
            controller.ViewBag.Terms = _themeManager.Terms;
            controller.ViewBag.ContactUsHref = _themeManager.ContactUsHref;
            controller.ViewBag.Info = _themeManager.Info;
            controller.ViewBag.SessionDurationMinutes = _config.Value.Application.SessionDurationMinutes;
            controller.ViewBag.SessionWarningMinutes = _config.Value.Application.SessionWarningMinutes;

            if (context.HttpContext.User.Identity.IsAuthenticated && !NoNotifyActions.Contains(((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName, StringComparer.OrdinalIgnoreCase))
            {
                //fire a notification that a new page has been entered, entering a page will refresh the session timeout. Let all the other pages know.
                string username = context.HttpContext.User.Identity.Name;
                _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
                {
                    await _sessionHub.Clients.Groups(username).SendAsync("onSessionRefresh", DateTime.UtcNow);
                });
            }

            base.OnActionExecuting(context);
        }

        [Route("~/login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                string url =  System.Net.WebUtility.UrlDecode(Request.Query.Where(q => q.Key == "ReturnUrl").Select(q => q.Value).FirstOrDefault());

                return Redirect("~" + url);
            }                   

            return View();
        }

        [Authorize]
        [Route("~/configuration")]
        public IActionResult Configuration()
        {
            var model = new Configuration
            {
                NetworkName = _config.Value.PopMedNet.NetworkName,
                DMCSInstanceIdentifier = _config.Value.Settings.DMCSIdentifier.ToString("D"),
                ServiceURL = _config.Value.PopMedNet.ApiServiceURL,
                CacheFolder = _config.Value.Settings.CacheFolder,
                CacheTimer = _config.Value.Settings.CacheFolderTimer
            };

            return View(model);
        }

        [Authorize]
        [Route("~/application-logs")]
        public IActionResult ApplicationLogs()
        {
            return View();
        }

        [Authorize]
        [Route("~/datamart/{id}")]
        public IActionResult DataMart(Guid id)
        {

            ViewBag.ID = id;         

            try
            {
                using(var api = PMNApi.PMNApiClient.CreateForUser(_config.Value, Request.Cookies))
                {
                    ViewBag.CanConfigure = api.CanConfigureDataMart(id).Result;
                }
            }
            catch {
                ViewBag.CanConfigure = false;
            }
            
            return View();
        }

        /// <summary>
        /// Displays the details for a route.
        /// </summary>
        /// <param name="id">The ID of the route.</param>
        /// <returns></returns>
        [Authorize]
        [Route("~/route/{id}")]
        public IActionResult RouteDetail(Guid id)
        {
            var details = _modelDb.RequestDataMarts.Where(rdm => rdm.ID == id).Select(rdm => new RoutingDTO { ID = rdm.ID, RequestName = rdm.Request.Name, DataMartName = rdm.DataMart.Name }).FirstOrDefault();
            
            return View(details);
        }

        [Authorize,
         Route("~/logout")]
        public async Task<IActionResult> Logout()
        {
            string username = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                username = User.Identity.Name;
                this._logger.Information("Signing out for user: {0}", User.Identity.Name);
            }

            await _signInManager.SignOutAsync();

            Response.Cookies.Delete("DMCS-User");
            

            if (!string.IsNullOrEmpty(username))
            {
                _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await _sessionHub.Clients.Groups(username).SendAsync("onLogout");
                });
            }

            return RedirectToAction("Login");
        }

        [Authorize, Route("~/touch")]
        public async Task<IActionResult> Touch()
        {
            string username = User.Identity.Name;
            var identityUser = await _signInManager.UserManager.FindByNameAsync(username);
            await _signInManager.RefreshSignInAsync(identityUser);
            
            _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
            {
                await _sessionHub.Clients.Groups(username).SendAsync("onSessionRefresh", DateTime.UtcNow);
            });

            return Ok();
        }

        [HttpGet,
         Route("~/download-document")]
        public async Task<IActionResult> DownloadDocument(Guid id, Guid requestDMID, string filename)
        {
            var req = await (from rdm in this._modelDb.RequestDataMarts
                              join resp in this._modelDb.Responses on rdm.ID equals resp.RequestDataMartID
                              join respDoc in this._modelDb.RequestDocuments on resp.ID equals respDoc.ResponseID
                              join doc in this._modelDb.Documents on respDoc.RevisionSetID equals doc.RevisionSetID
                              where rdm.ID == requestDMID && doc.ID == id && resp.Count == rdm.Responses.Max(rr => rr.Count)
                              select new { 
                                DataMart = rdm.DataMart,
                                RequestID = rdm.RequestID,
                                ResponseID = resp.ID,
                              }).FirstOrDefaultAsync();

            var cacheManager = new CacheManager(_config, req.DataMart, req.RequestID, req.ResponseID, this._logger);

            return await cacheManager.ReturnDownloadActionResultAsync(Request, id, this._modelDb);
        }

        [Authorize,
         Route("~/"),
         Route("/Home"),
         Route("~/HomeController/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true),
         Route("~/error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        
    }

    public class Configuration
    {
        public string NetworkName { get; set; }
        public string DMCSInstanceIdentifier { get; set; }
        public string ServiceURL { get; set; }
        public string CacheFolder { get; set; }
        public int CacheTimer { get; set; }
    }
}
