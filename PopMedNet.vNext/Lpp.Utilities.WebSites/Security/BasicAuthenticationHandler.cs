using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Lpp.Utilities.WebSites.Security
{
    public class BasicAuthenticationHandler<TDataContext, TPermission> : DelegatingHandler
        where TDataContext : DbContext, ISecurityContextProvider<TPermission>, new()
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            
            if (request.Method != HttpMethod.Options)
                await ParseAuthorizationHeader(request); //Will throw an error if not logged in

            return await base.SendAsync(request, cancellationToken);
        }

        protected async virtual Task ParseAuthorizationHeader(HttpRequestMessage request)
        {
            ApiIdentity ident = null;
            string username = null;
            string password = null;
            Guid? employerID = null;

            if (request.Headers.Authorization == null)
            {
                HttpContext.Current.User = null;
                Thread.CurrentPrincipal = null;
                return;
            }

            var authToken = request.Headers.Authorization.Parameter;

            if (string.Equals("PopMedNet", request.Headers.Authorization.Scheme))
            {
                var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken)).Split(':');
                if(decodedToken.Length > 1)
                {
                    //employerID = Guid.Parse(decodedToken[1]);
                    if(Guid.TryParse(decodedToken[1], out Guid emp))
                    {
                        employerID = emp;
                    }                        
                }

                Lpp.Utilities.WebSites.Models.LoginResponseModel.DecryptCredentials(decodedToken[0], out username, out password);

            } else {
                var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken)).Split(':');
                username = decodedToken[0];
                password = decodedToken[1];

                if (decodedToken.Length > 2)
                    employerID = new Guid(decodedToken[2]);
            }

            if (password.IsEmpty() || username.IsEmpty())
            {
                HttpContext.Current.User = null;
                Thread.CurrentPrincipal = null;
                return;
            }

            

            //Check if it's in the cache and pull it out.
            ident = HttpContext.Current.Cache[authToken] as ApiIdentity;

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
                    catch {
                        HttpContext.Current.User = null;
                        Thread.CurrentPrincipal = null;
                        return;
                    }
                }


                HttpContext.Current.Cache.Add(authToken, ident, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 10, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
            }
            else
            {
                ident.EmployerID = employerID;
                HttpContext.Current.Cache[authToken] = ident;
            }

            HttpContext.Current.User = new GenericPrincipal(ident, new string[] { });
            Thread.CurrentPrincipal = HttpContext.Current.User;
        }
    }
}
