using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Utilities;
using System.Web;
using Lpp.Utilities.Security;

namespace Lpp.Dns.WebServices
{
    public class BasicAuthenticationMVC : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var req = actionContext.HttpContext.Request;
            if (String.IsNullOrEmpty(req.Headers["Authorization"]))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            var authToken = req.Headers["Authorization"].Substring("Basic ".Length);
            var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken)).Split(':');

            var username = decodedToken[0];
            var password = decodedToken[1];
            Guid? employerID = null;
            if (decodedToken.Length > 2)
                employerID = new Guid(decodedToken[2]);


            //Check if it's in the cache and pull it out.
            var ident = actionContext.HttpContext.Cache[authToken] as ApiIdentity;

            if (ident == null)
            {
                //Check if it's in the cache and pull it out.
                ident = HttpContext.Current.Cache[authToken] as ApiIdentity;
            }

            if (ident == null)
            {
                //Load it up here.
                using (var db = new Dns.Data.DataContext())
                {
                    var contact = (from c in db.Users
                                   where c.UserName == username && !c.Deleted
                                   select new { c.ID, c.UserName, c.PasswordHash, c.FirstName, c.LastName, Subscriptions = c.Subscriptions.Select(s => new Subscription {EventID = s.EventID, Frequency = (int) s.Frequency}) }).FirstOrDefault();

                    if (contact == null)
                    {
                        HandleUnauthorizedRequest(actionContext);
                        return;
                    }

                    if (password.ComputeHash() != contact.PasswordHash)
                    {
                        HandleUnauthorizedRequest(actionContext);
                        return;
                    }

                    ident = new ApiIdentity(contact.ID, contact.UserName, contact.FirstName + " " + contact.LastName, employerID);
                }


                HttpContext.Current.Cache.Add(authToken, ident, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }

            HttpContext.Current.User = new GenericPrincipal(ident, new string[] { });
        }

        private void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
        {
            var res = filterContext.HttpContext.Response;
            res.StatusCode = 401;
            res.AddHeader("WWW-Authenticate", "Basic realm=\"PopMedNet\"");
            res.End();
        }
    }
}
