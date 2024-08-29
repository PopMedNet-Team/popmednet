using System.Web.Configuration;
using Lpp.Utilities;
using Lpp.Dns.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lpp.Dns.ApiClient;
using Lpp.Utilities.WebSites.Models;

namespace Lpp.Utilities.WebSites.Attributes
{
    /// <summary>
    /// Custom authorization attribute used by Lpp.Dns.Sso.
    /// </summary>
    public class LppDnsAuthorize : System.Web.Mvc.AuthorizeAttribute
    {
        public const string Salt = "07809072-4AB9-4998-A19C-855287983782";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(System.Web.Mvc.AllowAnonymousAttribute), true).Any())
                return;

            if (filterContext.RequestContext.HttpContext.Request.Cookies["Authorization"] == null)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }

            var authCookie = filterContext.RequestContext.HttpContext.Request.Cookies["Authorization"];

            var model = JsonConvert.DeserializeObject<LoginResponseModel>(authCookie.Value );

            if (model == null || model.UserName.IsEmpty() || model.Authorization.IsEmpty() || model.Token.IsNullOrWhiteSpace() || model.TokenIsValid() == false)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }

            string username, password;
            LoginResponseModel.DecryptCredentials(model.Authorization, out username, out password);

            //Look in the cache for the login and password
            object cacheInfo = filterContext.HttpContext.Cache[username + model.ID.Value];

            if (cacheInfo == null)
            {
                //Look it up and add it to the cache if they are vaild.
                using (var client = Lpp.Dns.WebSites.DnsApiWebsiteServiceClient.GetClient())
                {
                    try
                    {
                        var user = AsyncHelpers.RunSync(() => client.Users.ValidateLogin(new LoginDTO
                        {
                            UserName = username,
                            Password = password,
                            RememberMe = false
                        }));

                        filterContext.HttpContext.Cache.Add(username + user.ID.Value, true, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), System.Web.Caching.CacheItemPriority.High, null);

                        model.ID = user.ID.Value;
                        filterContext.HttpContext.User = new GenericPrincipal(model, new string[] { });
                    }
                    catch
                    {
                        HandleUnauthorizedRequest(filterContext);
                    }
                }

            }
            else if (cacheInfo is bool && Convert.ToBoolean(cacheInfo) == false)
            {
                //User is banned or invalid credentials
                HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.HttpContext.User = new GenericPrincipal(model, new string[] { });
            }

            var expireMinutes = WebConfigurationManager.AppSettings["SessionExpireMinutes"];
            if (string.IsNullOrWhiteSpace(expireMinutes))
                expireMinutes = "30";

            //Refresh the cookie
            filterContext.HttpContext.Response.SetCookie(new System.Web.HttpCookie("Authorization", JsonConvert.SerializeObject(new LoginResponseModel(model.ID, username, password, model.EmployerID, model.PasswordExpiration, Convert.ToInt32(expireMinutes)))));
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            System.Web.HttpContext.Current.Response.Cookies.Remove("Authorization");

            if (filterContext.HttpContext.Request.RawUrl.ToLower().Contains("/account/logoff"))
            {
                filterContext.Result = new RedirectResult("/account/login");
            }
            else
            {
                string rawUrl = filterContext.HttpContext.Request.RawUrl;
                if (rawUrl.ToUpper().Contains("RETURNURL="))
                {
                    filterContext.Result = new RedirectResult("/account/login" + filterContext.HttpContext.Request.RawUrl);
                }
                else
                {
                    if(string.IsNullOrEmpty(filterContext.HttpContext.Request.RawUrl) || string.Equals("/", filterContext.HttpContext.Request.RawUrl))
                    {
                        filterContext.Result = new RedirectResult("/account/login");
                    }
                    else
                    { 
                        filterContext.Result = new RedirectResult("/account/login?ReturnUrl=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                    }
                }
            }
        }
    }
}
