using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Utilities.WebSites.Security
{
    public class PopMedNetAuthenticationSchemeOptions : AuthenticationSchemeOptions {
            public PopMedNetAuthenticationSchemeOptions() { }
    }

    public static class AuthSchemeConstants
    {
        public const string BasicScheme = "Basic";
        public const string Scheme = "PopMedNet";
        public const string NToken = $"{Scheme} (?<token>.*)";
    }

    public class PopMedNetAuthenticationHandler : AuthenticationHandler<PopMedNetAuthenticationSchemeOptions>
    {
        Utilities.Security.IPopMedNetAuthenticationManager _authenticationManager;

        public PopMedNetAuthenticationHandler(
            IOptionsMonitor<PopMedNetAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            PopMedNet.Utilities.Security.IPopMedNetAuthenticationManager authenticationManager) : base(options, logger, encoder, clock)
        {
            _authenticationManager = authenticationManager;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            //{
            //    return Task.FromResult(AuthenticateResult.Fail("Authoriation header not found."));
            //}


            if (System.Net.Http.Headers.AuthenticationHeaderValue.TryParse(Request.Headers[HeaderNames.Authorization], out System.Net.Http.Headers.AuthenticationHeaderValue? header))
            {
                //ApiIdentity identity = null;
                string username;
                string password;
                Guid? employerID = null;

                var authToken = header.Parameter;
                if (header.Scheme == "Basic")
                {
                    var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken!)).Split(':');
                    username = decodedToken[0];
                    password = decodedToken[1];

                    if (decodedToken.Length > 2)
                        employerID = new Guid(decodedToken[2]);
                }
                else if(header.Scheme == AuthSchemeConstants.Scheme)
                {
                    var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken!)).Split(':');
                    if (decodedToken.Length > 1)
                    {
                        if (Guid.TryParse(decodedToken[1], out Guid emp))
                        {
                            employerID = emp;
                        }
                    }

                    Models.LoginResponseModel.DecryptCredentials(decodedToken[0], out username, out password);
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authorization failed"));
                }

                //TODO: if identity exists in cache update the employerID and the cache item, else add to the cache
                try
                {
                    IUser contact;
                    if (_authenticationManager.ValidateUser(username, password, out contact))
                    {
                        ApiIdentity identity = new ApiIdentity(contact.ID, contact.UserName, $"{ contact.FirstName } { contact.LastOrCompanyName }", employerID);

                        var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                }
                catch
                {
                    return Task.FromResult(AuthenticateResult.Fail("An error occured validating the credentials."));
                }
            }

            return Task.FromResult(AuthenticateResult.Fail("Authorization failed"));
        }
    }
}
