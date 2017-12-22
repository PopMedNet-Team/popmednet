using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Lpp.Composition;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Controllers;
using Lpp.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IMvcFilter))]
    public class AuthenticationFilter : IAuthorizationFilter, IMvcFilter
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(AuthenticationFilter));

        public void OnAuthorization(AuthorizationContext filterContext)
        {

            var dp = filterContext.HttpContext.User as DnsPrincipal;
            if (dp != null) return;

            var db = filterContext.HttpContext.DataContext();
            

            string token = filterContext.HttpContext.Request.QueryString.Get("token");
            Guid passwordResetToken;
            if (!string.IsNullOrEmpty(token) && !Guid.TryParse(token, out passwordResetToken))
            {
                try
                {
                    string decryptedToken = Utilities.Crypto.DecryptString(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token)));

                    string[] split = decryptedToken.Split(new[] { ':' });
                    string username = split[0];
                    string passwordHashed = Password.ComputeHash(split[1]);

                    DateTime issued = DateTime.ParseExact(string.Join(":", split[2], split[3], split[4]), "s", null, System.Globalization.DateTimeStyles.AssumeUniversal);
                    //must have been issue within the last 6 hours
                    DateTime cutoffDate = DateTime.UtcNow.AddHours(-6);

                    var user = (from u in db.Users
                                where !u.Deleted && u.Active
                                && u.UserName == username
                                && cutoffDate < issued
                                select u).FirstOrDefault();

                    if (user != null && string.Equals(user.PasswordHash, passwordHashed, StringComparison.Ordinal))
                    {
                        filterContext.HttpContext.User = new DnsPrincipal(user);
                        System.Web.Security.FormsAuthentication.SetAuthCookie(user.ID.ToString(), false);

                        var cookie = new Lpp.Utilities.WebSites.Models.LoginResponseModel(user, split[1], user.OrganizationID, user.PasswordExpiration, -1);

                        var sModel = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
                        var authCookie = new System.Web.HttpCookie("Authorization", sModel)
                        {
                            Shareable = false,
                            Expires = DateTime.MinValue,
                        };

                        filterContext.HttpContext.Response.Cookies.Add(authCookie);

                        return;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error authenticating via token.", ex);
                }
            }


            MaybeNotNull<User> result = from u in Maybe.Value(filterContext.HttpContext.User)
                                        from i in u.Identity
                                        from n in i.Name
                                        from id in Guid.Parse(n)
                                        from user in db.Users.SingleOrDefault(uu => uu.ID == id)
                                        where !user.Deleted
                                        select user;

            var anonymous = AllowAnonymous(filterContext);
            if (result.Kind == MaybeKind.Value)
            {
                if (result.Value.PasswordExpiration <= DateTime.Now && !anonymous)
                {
                    filterContext.Result = new RedirectResult(new UrlHelper(filterContext.RequestContext).Action(
                        (HomeController c) => c.PasswordExpired()));
                }
                else
                {
                    filterContext.HttpContext.User = new DnsPrincipal(result.Value);
                }
            }
            else if (!anonymous)
            {
                if (filterContext.IsChildAction)
                {
                    filterContext.Result = View.Result<Views.Errors.AccessDenied>().WithoutModel();
                }
                else if (AjaxCall(filterContext) || filterContext.HttpContext.IsEmbeddedRequest())
                {
                    filterContext.Result = new AjaxJsonResponseResult(new { code = "auth", message = "Session is no longer valid.<br/>Please refresh the page to log in.", redirectTo = LoginUrl(filterContext) });
                }
                else
                {
                    filterContext.Result = new RedirectResult(LoginUrl(filterContext));
                }
            }
        }

        static readonly char[] _questionMark = new[] { '?' };

        private static string LoginUrl(AuthorizationContext filterContext)
        {
            var rq = filterContext.HttpContext.Request;
            var url = new UrlHelper(filterContext.RequestContext);

            var res = LoginUrl(url, rq.Url);
            if (string.Equals(res.Split(_questionMark, 2)[0], rq.Url.LocalPath, StringComparison.InvariantCultureIgnoreCase))
            {
                res = LoginUrl(url, rq.UrlReferrer);
            }

            return res;
        }

        private static string LoginUrl(UrlHelper url, Uri returnUrl)
        {
            return url.Action((Controllers.HomeController c) => c.Login(returnUrl.ToString()));
        }

        private bool AllowAnonymous(AuthorizationContext filterContext)
        {
            return
                filterContext.ActionDescriptor.GetAttributes<AllowAnonymousAttribute>()
                .FirstOrDefault() != null;
        }

        private bool AjaxCall(AuthorizationContext filterContext)
        {
            return
                filterContext.ActionDescriptor.GetAttributes<AjaxCallAttribute>()
                .Select(a => a.AjaxCall)
                .FirstOrDefault();
        }

        public bool AllowMultiple
        {
            get { return true; }
        }

        public int Order
        {
            get { return 0; }
        }
    }
}