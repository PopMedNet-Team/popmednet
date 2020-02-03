using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Lpp.Audit.UI;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using System.Configuration;
using System.Web.Configuration;
using System.Security;
using System.Web;
using Lpp.Dns.Data;
using System.Threading.Tasks;
using Lpp.Utilities.WebSites.Models;
using Lpp.Dns.DTO;
using Lpp.Utilities;

namespace Lpp.Dns.Portal.Controllers
{
    [Export, ExportController, AutoRoute]
    public class HomeController : BaseController
    {
        [Import]
        public IAuthenticationService Auth { get; set; }

        
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml",
                    new LandingPageModel {
                        CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                        Notifications = null,
                        AllowTaskList = true           
                    }
                );
        }


        [AjaxCall]
        public ActionResult NotificationsBody(NotificationsGetModel get)
        {
            return Mvc.View.Result<Views.Home.NotificationsBody>().WithModel(NotificationsModel(get));
        }

        IListModel<VisualizedAuditEvent, NotificationsGetModel> NotificationsModel(NotificationsGetModel get)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var from = DateTime.Today.AddDays(-get.GetPeriodDaysBack());
            //var kind = get.GetKindFilter();
            //var myKind = kind.Kind == MaybeKind.Value ?
            //    new Func<AuditEventKind, bool>(e => e.Id == kind.Value)
            //    : (_ => true);

            //var notificationsFilter = (from s in Auth.CurrentUser.Subscriptions
            //                           from ff in Audit.DeserializeFilters(s.FiltersDefinitionXml)
            //                           where myKind(ff.Key)
            //                           group ff by ff.Key into filtersForKind
            //                           from filters in filtersForKind
            //                           from filter in filters
            //                           select new { filtersForKind.Key, filter }
            //                          )
            //                          .ToLookup(x => x.Key, x => x.filter);

            //return Audit
            //    .GetEvents(from, null, notificationsFilter)
            //    .RequirePermission(notificationsFilter.Select(k => k.Key), Security, SecPrivileges.Event.Observe, Auth.CurrentUser)
            //    .ListModel(get, _eventSort, 10)
            //    .Map(ee => AuditUI.Visualize(AuditUIScope.Display, ee));
        }


        [AllowAnonymous, NoAjaxNavigation]
        public ActionResult Login(string returnUrl)
        {
            Response.SetCookie(new HttpCookie("Authorization", null));
            Auth.SetCurrentUser(null, AuthenticationScope.Permanent);

            if (Auth.CurrentUser != null && Auth.CurrentUser.Active) 
                return ReturnTo(returnUrl);

            string oAuthKey = WebConfigurationManager.AppSettings["SsoKey"];
            string oAuthHash = WebConfigurationManager.AppSettings["SsoHash"];
            string ssoUrl = WebConfigurationManager.AppSettings["SsoUrl"];

            if (!string.IsNullOrWhiteSpace(oAuthKey) && !string.IsNullOrWhiteSpace(oAuthHash) && !string.IsNullOrWhiteSpace(ssoUrl))
                return Redirect(ssoUrl + "?returnUrl=" + HttpUtility.UrlEncode(returnUrl));

            return View("~/Views/Home/Login.cshtml", new LoginModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        public ActionResult SsoLogin(string Data, string returnUrl)
        {
            string oAuthKey = WebConfigurationManager.AppSettings["SsoKey"];
            string oAuthHash = WebConfigurationManager.AppSettings["SsoHash"];
            string ssoUrl = WebConfigurationManager.AppSettings["SsoUrl"];

            if (string.IsNullOrWhiteSpace(oAuthKey) || string.IsNullOrWhiteSpace(oAuthHash))
                throw new SecurityException("This site is not configured to support Single Sign On");

            var decodedData = Lpp.Utilities.Legacy.Crypto.DecryptStringAES(Data, oAuthHash, oAuthKey).Split(':');

            var userName = decodedData[0];
            var password = decodedData[1];

            var user = DataContext.Users.SingleOrDefault(u => u.UserName == userName && !u.Deleted);

            var errors = string.Empty;
            if (user == null)
            {
                errors += "We're sorry but you do not have permission to access this site from Single Sign On.";
            }
            else
            {
                if (UsersService.IsLocked(user))
                {
                    errors += "Your account has been locked after too many unsuccessful login attempts. Please contact your administrator.";
                }
                else
                {
                    user.FailedLoginCount = 0;
                }

                if (!user.Active)
                {
                    errors += "Account not active. Please contact your administrator";
                }
            }

            if (!string.IsNullOrWhiteSpace(errors))
            {
                var url = ssoUrl + "?error=" + HttpUtility.UrlEncode(errors) + "&ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);
                Response.Redirect(url);
            }

            Auth.SetCurrentUser(user, AuthenticationScope.WebSession);

            var expireMinutes = WebConfigurationManager.AppSettings["SessionExpireMinutes"];
            var sModel = Newtonsoft.Json.JsonConvert.SerializeObject(new LoginResponseModel(user, password, user.OrganizationID, user.PasswordExpiration, expireMinutes.ToInt32()));
            var authCookie = new HttpCookie("Authorization", sModel)
            {
                Shareable = false,
                Expires = DateTime.MinValue,
            };

            Response.Cookies.Add(authCookie);

            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "~/" : returnUrl);

        }

        [HttpPost]
        [AllowAnonymous, NoAjaxNavigation]
        public async Task<ActionResult> Login(string username, string password, string returnUrl)
        {
            if (Auth.CurrentUser != null) 
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "~/" : returnUrl);

            using (var client = new Lpp.Dns.ApiClient.DnsClient(WebConfigurationManager.AppSettings["ServiceUrl"]))
            {
                UserDTO contact = null;
                try
                {
                    contact = await client.Users.ValidateLogin(new Lpp.Dns.DTO.LoginDTO
                    {
                        UserName = username,
                        Password = password,
                        IPAddress = HttpContext.Request.UserHostAddress,
                        Enviorment = "Portal"
                    });

                }
                catch (Exception ex)
                {
                    string msg = "";
                    int i = 0;

                    while (i < ex.Message.Length && !ex.Message[i].Equals(';'))
                    {
                        msg = msg + ex.Message[i];
                        i++;
                    }

                    ModelState.AddModelError("Error", msg);
                }

                if (!ModelState.IsValid)
                    return View("~/Views/Home/Login.cshtml", new LoginModel { ReturnUrl = returnUrl });

                User user = await DataContext.Users.FindAsync(contact.ID);
                Auth.SetCurrentUser(user, AuthenticationScope.WebSession);

                var expireMinutes = WebConfigurationManager.AppSettings["SessionExpireMinutes"];
                if (string.IsNullOrWhiteSpace(expireMinutes))
                    expireMinutes = "30";
                

                var sModel = Newtonsoft.Json.JsonConvert.SerializeObject(new LoginResponseModel(user, password, user.OrganizationID, user.PasswordExpiration, Convert.ToInt32(expireMinutes)));
                var authCookie = new HttpCookie("Authorization", sModel)
                {                    
                    Shareable = false,
                    Expires = DateTime.MinValue,
                };

                Response.Cookies.Add(authCookie);

                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "~/" : returnUrl);
            }
          

            
        }

        [AllowAnonymous]
        public ActionResult Logout(string returnUrl)
        {
            Response.SetCookie(new HttpCookie("Authorization", null));
            Auth.SetCurrentUser(null, AuthenticationScope.Permanent);
            string ssoUrl = WebConfigurationManager.AppSettings["SsoUrl"];

            if (!string.IsNullOrWhiteSpace(ssoUrl))
            {
                return Redirect(ssoUrl + "/account/logoff?returnUrl=" + HttpUtility.UrlEncode(returnUrl));
            }
            else
            {
                return ReturnTo(string.IsNullOrEmpty(returnUrl) ? Url.Action((HomeController c) => c.Index()) : returnUrl);
            }
        }

        [HttpGet, NoAjaxNavigation, AllowAnonymous]
        public ActionResult PasswordExpired()
        {
            return View("~/Views/Home/ExpiredPassword.cshtml");
        }

        [HttpPost, NoAjaxNavigation, AllowAnonymous]
        public async Task<ActionResult> PasswordExpired(string newPassword)
        {
            if (Lpp.Dns.Data.User.CheckPasswordStrength(newPassword) != DTO.Enums.PasswordScores.VeryStrong)
            {
                ModelState.AddModelError("Error", "The password specified is not strong enough. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':;<'.");
                return View("~/Views/Home/ExpiredPassword.cshtml");
            }

            if (Auth.ApiIdentity.IsAuthenticated == false)
            {
                ModelState.AddModelError("Error", "You must login before changing your password.");
                return View("~/Views/Home/ExpiredPassword.cshtml");
            }

            var user = await DataContext.Users.FindAsync(Auth.ApiIdentity.ID);
            string newHash = newPassword.ComputeHash();

            if (string.CompareOrdinal(user.PasswordHash, newHash) == 0)
            {
                ModelState.AddModelError("Error", "Your new password must be different than your previous password.");
                return View("~/Views/Home/ExpiredPassword.cshtml");
            }

            user.PasswordHash = newHash;
            user.PasswordExpiration = DateTime.Now.AddMonths(ConfigurationManager.AppSettings["ConfiguredPasswordExpiryMonths"].ToInt32());

            //Save it
            await DataContext.SaveChangesAsync();

            Auth.SetCurrentUser(user, AuthenticationScope.WebSession);

            var expireMinutes = WebConfigurationManager.AppSettings["SessionExpireMinutes"];
            if (string.IsNullOrWhiteSpace(expireMinutes))
                expireMinutes = "30";
            

            var sModel = Newtonsoft.Json.JsonConvert.SerializeObject(new LoginResponseModel(user, newPassword, user.OrganizationID, user.PasswordExpiration, expireMinutes.ToInt32()));
            var authCookie = new HttpCookie("Authorization", sModel)
            {
                Shareable = false,
                Expires = DateTime.MinValue,
            };

            Response.Cookies.Remove("Authorization");
            Response.Cookies.Add(authCookie);

            return Redirect("~/");
        }

        [AllowAnonymous]
        public ActionResult TermsAndConditions()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Info()
        {
            return View();
        }

        public ActionResult DefaultSecurityPermissions()
        {
            return View();
        }

        [AllowAnonymous, NoAjaxNavigation]
        public ActionResult RestorePassword(string token)
        {
            Guid tokn;
            if (!Guid.TryParse(token, out tokn))
            {
                return RedirectToAction("Index");
            }
            return View("~/Views/Home/RestorePassword.cshtml");
        }
                
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View("~/Views/Home/ForgotPassword.cshtml");
        }

        [AllowAnonymous]
        public ActionResult UserRegistration()
        {
            return View("~/Views/Home/UserRegistration.cshtml");
        }

        public ActionResult Resources()
        {
            return View("~/Views/Home/Resources.cshtml", new ResourcesModel());
        }

        public ActionResult NotYetImplemented()
        {
            return View("~/Views/Home/NotYetImplemented.cshtml");
        }
    }
}