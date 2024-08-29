using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Lpp.Dns.Sso.Models;
using Lpp.Utilities;
using Lpp.Dns.WebSites;
using Lpp.Dns.ApiClient;
using Lpp.Utilities.WebSites.Models;
using Newtonsoft.Json;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites;
using Lpp.Utilities.WebSites.Attributes;

namespace Lpp.Dns.Sso.Controllers
{
    [RemoteRequireHttps, NoCache]
    public class AccountController : Controller
    {
        [AllowAnonymous, RemoteRequireHttps, NoCache]
        public ActionResult Login(string returnUrl, string error = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Error = error;
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous, RemoteRequireHttps]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {            
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await ApiController.Logon(model.UserName, model.Password);

                    if (user == null)
                    {
                        ViewBag.Error = "Please check your login and password and try again.";
                        ViewBag.ReturnUrl = returnUrl;
                        return View(model);
                    }
                    else
                    {
                        if (returnUrl.IsNullOrWhiteSpace()) 
                            return Redirect(returnUrl ?? "/");

                        var uri = new Uri(returnUrl, UriKind.RelativeOrAbsolute);
                        if (!uri.IsAbsoluteUri) 
                            return Redirect(returnUrl ?? "/");

                        using (var client = DnsApiWebsiteServiceClient.GetClient())
                        {
                            var ssoEndpoint = (await client.SsoEndpoints.List("$filter=startswith(PostUrl, '" + HttpUtility.UrlEncode("http://" + uri.Authority) + "') or startswith(PostUrl, '" + HttpUtility.UrlEncode("https" + "://" + uri.Authority) + "')")).FirstOrDefault();

                            if (ssoEndpoint == null)
                                return Redirect("/");

                            return SiteLoginFromEndpoint(ssoEndpoint, model.Password, returnUrl);
                        }
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ReturnUrl = returnUrl;
#if(DEBUG)
                ViewBag.Error = ex.Message;
#else 
                ViewBag.Error = "Please check your username and password and try again";
#endif
                return View(model);
            }
        }

        /// <summary>
        /// Logs into the site passed.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [LppDnsAuthorize]
        public async Task<ActionResult> SiteLogin(Guid ID)
        {            
            using (var client = DnsApiWebsiteServiceClient.GetClient())
            {
                var sso = await client.SsoEndpoints.Get(ID);

                var context = System.Web.HttpContext.Current;

                var cookie = JsonConvert.DeserializeObject<LoginResponseModel>(context.Request.Cookies["Authorization"].Value);

                string username, password;
                LoginResponseModel.DecryptCredentials(cookie.Authorization, out username, out password);

                return SiteLoginFromEndpoint(sso, password);
            }
        }

        private ActionResult SiteLoginFromEndpoint(SsoEndpointDTO endpoint, string password, string returnUrl = null)
        {
            //this has to be done this way instead of just pulling the cookie because on redirect pass through the cookie won't exist in the request.
            var userName = HttpContext.User.Identity.Name;

            var data = userName;

            if (endpoint.RequirePassword)
                data += ":" + password;
           
            var url = endpoint.PostUrl;

            if (!endpoint.oAuthHash.IsEmpty() && !endpoint.oAuthKey.IsEmpty()) { 
                var encryptedData = Crypto.EncryptStringAES(data, endpoint.oAuthHash, endpoint.oAuthKey);
                url += "?Data=" + HttpUtility.UrlEncode(encryptedData);
            }

            if (!returnUrl.IsEmpty())
                url += (url.Contains("?") ? "&" : "?") + "returnUrl=" + HttpUtility.UrlEncode(returnUrl);            
            return new RedirectResult(url);
        }

        [AllowAnonymous, RemoteRequireHttps]
        public ActionResult ForgotPassword()
        {
            return View("_ForgotPassword");
        }

        [LppDnsAuthorize, RemoteRequireHttps]
        public ActionResult LogOff()
        {
            var context = System.Web.HttpContext.Current;
            var cookie = JsonConvert.DeserializeObject<LoginResponseModel>(context.Request.Cookies["Authorization"].Value);
            HttpContext.Cache.Remove(cookie.UserName + cookie.ID.Value);

            System.Web.HttpContext.Current.Response.Cookies.Remove("Authorization");
            HttpContext.User = null;

            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("UserRegistration");
        }
        
    }
}