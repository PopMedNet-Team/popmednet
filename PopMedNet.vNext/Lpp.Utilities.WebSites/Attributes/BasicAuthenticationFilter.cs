using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using Lpp.Utilities;
using System.Web;
using Lpp.Utilities.Security;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Lpp.Utilities.WebSites.Attributes
{
    public class BasicAuthenticationFilter<TDataContext, TPermission> : IAuthorizationFilter
        where TDataContext : DbContext, ISecurityContextProvider<TPermission>, new()
    {
        protected virtual async Task ParseAuthorizationHeader(AuthorizationContext actionContext)
        {
            var request = actionContext.HttpContext.Request;

            ApiIdentity ident = null;
            string username = null;
            string password = null;
            Guid? employerID = null;

            if (String.IsNullOrEmpty(request.Headers["Authorization"]))
            {
                HttpContext.Current.User = null;
                Thread.CurrentPrincipal = null;
                return;
            }

            var authToken = request.Headers["Authorization"].Substring("Basic ".Length);
            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken)).Split(':');

            username = decodedToken[0];
            password = decodedToken[1];

            if (password.IsEmpty() || username.IsEmpty())
            {
                HttpContext.Current.User = null;
                Thread.CurrentPrincipal = null;
                return;
            }

            if (decodedToken.Length > 2)
                employerID = new Guid(decodedToken[2]);


            //Check if it's in the cache and pull it out.
            ident = HttpContext.Current.Cache[username] as ApiIdentity;


            if (ident == null)
            {
                //Load it up here.
                using (var db = new TDataContext())
                {
                    try
                    {
                        var contact = await db.ValidateUser(username, password);

                        ident = new ApiIdentity(contact.ID, contact.UserName, contact.FirstName + " " + contact.LastOrCompanyName, employerID);
                    }
                    catch
                    {
                        HttpContext.Current.User = null;
                        Thread.CurrentPrincipal = null;
                        return;
                    }
                }


                HttpContext.Current.Cache.Add(username, ident, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                ident.EmployerID = employerID;
                HttpContext.Current.Cache[username] = ident;
            }

            HttpContext.Current.User = new GenericPrincipal(ident, new string[] { });
            Thread.CurrentPrincipal = HttpContext.Current.User;
        }


        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var res = filterContext.HttpContext.Response;
            res.StatusCode = 401;
            res.AddHeader("WWW-Authenticate", "Basic");
            res.End();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
                return;

            AsyncHelpers.RunSync(() => ParseAuthorizationHeader(filterContext));

            if (HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated)
                HandleUnauthorizedRequest(filterContext);

        }
    }
}
