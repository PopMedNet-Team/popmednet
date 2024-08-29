using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Text;

namespace PopMedNet.DMCS.Code.Authorization
{
    public class RequestHeaderBasicAuthorizationAttributeAttribute : TypeFilterAttribute
    {
        public RequestHeaderBasicAuthorizationAttributeAttribute() : base(typeof(RequestHeaderBasicAuthorizationFilter))
        {
        }
    }

    public class RequestHeaderBasicAuthorizationFilter : IAuthorizationFilter
    {
        readonly SignInManager<Data.Identity.IdentityUser> signInManager;
        public RequestHeaderBasicAuthorizationFilter(SignInManager<Data.Identity.IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //check if the user is currently authenticated or not
            if (context.HttpContext.User.Identity.IsAuthenticated)
                return;

            try
            {
                //decrypt the basic auth header
                Microsoft.Extensions.Primitives.StringValues stringValues;
                if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out stringValues))
                {
                    var authToken = stringValues.FirstOrDefault();
                    if (!authToken.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                        return;

                    var decodedToken = Encoding.Default.GetString(Convert.FromBase64String(authToken.Substring("Basic ".Length))).Split(':');

                    if (decodedToken.Length < 2 || string.IsNullOrEmpty(decodedToken[0]) || string.IsNullOrEmpty(decodedToken[1]))
                    {
                        return;
                    }

                    //validate the credentials
                    var signInResult = signInManager.PasswordSignInAsync(decodedToken[0], decodedToken[1], false, false).Result;
                    if (signInResult.Succeeded)
                    {
                        var identity = signInManager.UserManager.FindByNameAsync(decodedToken[0]).Result;
                        context.HttpContext.User = signInManager.CreateUserPrincipalAsync(identity).Result;
                    }
                }

            }
            catch { }
        }
    }
}
