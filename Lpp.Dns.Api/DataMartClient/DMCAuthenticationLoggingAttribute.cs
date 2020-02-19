using Lpp.Dns.Data;
using Lpp.Dns.Data.Audit;
using Lpp.Dns.DTO.DataMartClient;
using Lpp.Utilities.Security;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Lpp.Dns.Api
{
    public class DMCAuthenticationLoggingAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.Request.Headers.Authorization == null)
            {
                HttpContext.Current.User = null;
                Thread.CurrentPrincipal = null;
                return;
            }

            var authToken = context.Request.Headers.Authorization.Parameter;
            ApiIdentity ident = HttpContext.Current.Cache[authToken] as ApiIdentity;

            if(ident != null)
            {
                if (string.Equals("PopMedNet", context.Request.Headers.Authorization.Scheme))
                {
                    var ctx = context.Request.Properties["MS_HttpContext"] as HttpContextWrapper;
                    var unsplitToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    var decodedToken = unsplitToken.Split(':');

                    DMCMetadata metadata = null;
                    if (decodedToken.Length > 2)
                    {
                        metadata = JsonConvert.DeserializeObject<DMCMetadata>(unsplitToken.Substring(decodedToken[0].Length + decodedToken[1].Length + 2));
                    }

                    using (var db = new DataContext())
                    {
                        Dns.Data.Audit.UserAuthenticationLogs successAudit = new UserAuthenticationLogs
                        {
                            UserID = ident.ID,
                            Description = $"User Authenticated Successfully from DataMart Client (Release: {metadata.DMCProductVersion}, Version: {metadata.DMCFileVersion}, IP Address: {ctx.Request.UserHostAddress}).",
                            Success = true,
                            IPAddress = ctx.Request.UserHostAddress,
                            Environment = "DataMart Client",
                            Details = unsplitToken.Substring(decodedToken[0].Length + decodedToken[1].Length + 2),
                            DMCVersion = metadata.DMCFileVersion,
                            Source = ident.RawUrl
                        };
                        db.LogsUserAuthentication.Add(successAudit);

                        await db.SaveChangesAsync();
                    }
                }
            }
            else
            {
                if (string.Equals("PopMedNet", context.Request.Headers.Authorization.Scheme))
                {
                    using (var db = new DataContext())
                    {
                        string username = null;
                        string password = null;
                        IUser user = null;
                        var ctx = context.Request.Properties["MS_HttpContext"] as HttpContextWrapper;

                        var unsplitToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                        var decodedToken = unsplitToken.Split(':');


                        Lpp.Utilities.WebSites.Models.LoginResponseModel.DecryptCredentials(decodedToken[0], out username, out password);
                        if (!db.ValidateUser2(username, password, out user))
                        {
                            if (user != null)
                            {
                                DMCMetadata metadata = null;
                                string reserializedJson = "";
                                if (decodedToken.Length > 2)
                                {
                                    metadata = JsonConvert.DeserializeObject<DMCMetadata>(unsplitToken.Substring(decodedToken[0].Length + decodedToken[1].Length + 2));
                                    metadata.InvalidCredentials = Lpp.Utilities.Crypto.EncryptStringAES("UserName: " + username + " was attempted with Password:" + password, "AuthenticationLog", user.ID.ToString("D"));
                                    reserializedJson = JsonConvert.SerializeObject(metadata);
                                }

                                UserAuthenticationLogs failedAudit = new UserAuthenticationLogs
                                {
                                    UserID = user.ID,
                                    Description = $"User Authenticated Failed from DataMart Client (Release: {metadata.DMCProductVersion}, Version: {metadata.DMCFileVersion}, IP Address: {ctx.Request.UserHostAddress}).",
                                    Success = false,
                                    IPAddress = ctx.Request.UserHostAddress,
                                    Environment = "DataMart Client",
                                    Details = reserializedJson,
                                    DMCVersion = metadata.DMCFileVersion,
                                    Source = ctx.Request.RawUrl
                                };
                                db.LogsUserAuthentication.Add(failedAudit);

                                await db.SaveChangesAsync();
                            }
                        }
                    }
                }                
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }
    }
}