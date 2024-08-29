using Lpp.Dns.ApiClient;
using Lpp.Dns.DTO;
using Lpp.Dns.Sso.Models;
using Lpp.Dns.WebSites;
using Lpp.Utilities.WebSites.Models;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Security.Principal;
using System.IO;
using Lpp.Utilities.WebSites;
using Lpp.Utilities.WebSites.Attributes;

namespace Lpp.Dns.Sso.Controllers
{
    public class ApiController : Controller
    {
        
        
        private HttpClient client;

        public ApiController()
        {
            var auth = System.Web.HttpContext.Current.Request.Headers["Authorization"];
            var host = WebConfigurationManager.AppSettings["ServiceUrl"];

            client = new HttpClient()
            {
                BaseAddress = new Uri(host)
            };

            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", System.Web.HttpContext.Current.Request.Headers["Accept"]);

            if (auth != null)
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", auth);
        }

        protected override void Dispose(bool disposing)
        {
            if (client != null)
                client.Dispose();

            base.Dispose(disposing);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.UserHostAddress.IsEmpty())
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-ClientIP", Request.UserHostAddress);

            base.OnActionExecuting(filterContext);
        }

        [HttpGet]
        public HttpStatusCodeResult NoOp()
        {
            var authCookie = HttpContext.Request.Cookies["Authorization"];
            Response.SetCookie(authCookie);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //[HttpGet, LppDnsAuthorize]
        //public async Task<HttpStatusCodeResult> Refresh()
        //{
        //    var context = System.Web.HttpContext.Current;

        //    var cookie = JsonConvert.DeserializeObject<LoginResponseModel>(context.Server.UrlDecode(context.Request.Cookies["Authorization"].Value));

        //    var currentUser = System.Web.HttpContext.Current.User.Identity as LoginResponseModel;

        //    using (var client = DnsApiWebsiteServiceClient.GetClient())
        //    {
        //        try
        //        {
        //            var user = await client.Users.ByUserName(currentUser.UserName);

        //            cookie = new LoginResponseModel
        //            {
        //                Password = currentUser.Password,
        //                UserName = currentUser.UserName,
        //                ID = user.ID                        
        //            };

        //            var sModel = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
        //            context.Response.SetCookie(new HttpCookie("Authorization", sModel));

        //            context.Cache.Remove(cookie.UserName + cookie.ID);

        //            context.Cache.Add(cookie.UserName + cookie.ID, cookie, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), System.Web.Caching.CacheItemPriority.High, null);

        //            context.User = new GenericPrincipal(cookie, new string[] { });
        //            return new HttpStatusCodeResult(HttpStatusCode.OK);
        //        }
        //        catch
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        //        }
        //    }
        //}

        [HttpGet, LppDnsAuthorize]
        public async Task<HttpStatusCodeResult> LogOut()
        {
            var authCookie = HttpContext.Request.Cookies["Authorization"];
            if (authCookie.IsEmpty())
                return new HttpStatusCodeResult(HttpStatusCode.OK);

            var cookie = JsonConvert.DeserializeObject<LoginResponseModel>(authCookie.Value);

            string username, password;
            LoginResponseModel.DecryptCredentials(cookie.Authorization, out username, out password);

            using (var client = DnsApiWebsiteServiceClient.GetClient(null, username, password))
            {
                try
                {
                    await client.Users.Logout();
                }
                catch { } //Ignore fail.

                HttpContext.Cache.Remove(username + cookie.ID.Value);
                Response.Cookies.Remove("Authorization");
                HttpContext.User = null;

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        /// <summary>
        /// Logs a user in to the system. Is called by all clients using the website reguardless of browser version.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, NoCache]
        public async Task<JsonResult> Login(string userName, string password)
        {
            var cookie = await ApiController.Logon(userName, password);
            if (cookie == null)
            {
                return new JsonResult();
            }
            else
            {
                return Json(cookie, JsonRequestBehavior.AllowGet);
            }
        }

        internal static async Task<LoginResponseModel> Logon(string userName, string password)
        {

            using (var client = DnsApiWebsiteServiceClient.GetClient())
            {
                try
                {
                    UserDTO user = await client.Users.ValidateLogin(new LoginDTO {
                        UserName = userName, 
                        Password = password,
                        IPAddress = System.Web.HttpContext.Current.Request.UserHostAddress,
                        Enviorment = "SSO"
                    });

                    var context = System.Web.HttpContext.Current;

                    var expireMinutes = WebConfigurationManager.AppSettings["SessionExpireMinutes"];
                    if (string.IsNullOrWhiteSpace(expireMinutes))
                        expireMinutes = "30";

                    var cookie = new LoginResponseModel(new UserDetail { ID = user.ID.Value, UserName = user.UserName, FirstName = user.FirstName, LastOrCompanyName = (string.IsNullOrEmpty(user.LastName) ? user.Organization : user.LastName) }, password, user.OrganizationID, null, expireMinutes.ToInt32());
                    context.Cache.Add(userName + user.ID.Value, cookie, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), System.Web.Caching.CacheItemPriority.High, null);

                    var sModel = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
                    context.Response.SetCookie(new HttpCookie("Authorization", sModel)
                    {
                        Shareable = false,
                        Expires = DateTime.MinValue,                        
                    });

                    context.User = new GenericPrincipal(cookie, new string[] { });

                    return cookie;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        internal class UserDetail : Lpp.Utilities.Security.IUser
        {
            public string FirstName { get; set; }

            public Guid ID { get; set; }

            public string LastOrCompanyName { get; set; }

            public string UserName { get; set; }
        }
        
        /// <summary>
        /// Executes a GET against the API
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        [HttpGet, NoCache]
        public async Task<string> Get(string Url)
        {
            try
            {
                var result = await client.GetAsync(Url);
                Response.StatusCode = (int)result.StatusCode;
                Response.StatusDescription = result.ReasonPhrase;
                var sContent = (StreamContent)result.Content;
                var rContent = await sContent.ReadAsStringAsync();
                return rContent;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                Response.StatusDescription = ex.UnwindException();
                return null;
            }
        }

        [HttpPut]
        public async Task<string> Put(string Url)
        {
            Request.InputStream.Position = 0;
            using (var reader = new StreamReader(Request.InputStream))
            {
                var val = await reader.ReadToEndAsync();
                var content = new StringContent(val, Encoding.UTF8, "application/json");
                try
                {
                    var result = await client.PutAsync(Url, content);

                    Response.StatusCode = (int)result.StatusCode;
                    Response.StatusDescription = result.ReasonPhrase;
                    var sContent = (StreamContent)result.Content;
                    var rContent = await sContent.ReadAsStringAsync();
                    return rContent;
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                    Response.StatusDescription = ex.UnwindException();
                    return null;
                }
            }
        }

        [HttpPost]
        public async Task<string> Post(string Url)
        {
            Request.InputStream.Position = 0;
            using (var reader = new StreamReader(Request.InputStream))
            {
                var val = await reader.ReadToEndAsync();
                var content = new StringContent(val, Encoding.UTF8, "application/json");

                try
                {
                    var result = await client.PostAsync(Url, content);
                    Response.StatusCode = (int)result.StatusCode;
                    Response.StatusDescription = result.ReasonPhrase;
                    var sContent = (StreamContent)result.Content;
                    var rContent = await sContent.ReadAsStringAsync();
                    return rContent;
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                    Response.StatusDescription = ex.UnwindException();
                    return null;
                }
            }
        }

        [HttpDelete]
        public async Task Delete(string Url)
        {
            try
            {
                var result = await client.DeleteAsync(Url);
                if (Response != null)
                {
                    Response.StatusCode = (int)result.StatusCode;
                    Response.StatusDescription = result.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                Response.StatusDescription = ex.UnwindException();
            }
        }
	}
}