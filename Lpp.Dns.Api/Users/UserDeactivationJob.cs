using Lpp.Dns.DTO.Enums;
using Lpp.Dns.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Lpp.Dns.Data.Audit;

namespace Lpp.Dns.Api.Users
{
    /// <summary>
    /// Deactivates all users who have not successfully authenticated with PMN within a specified number of days.
    /// </summary>
    public class UserDeactivationJob : IDisposable
    {   
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly DataContext _db;
        bool disposedValue;

        /// <summary>
        /// A new UserDeactivationJob instance.
        /// </summary>
        public UserDeactivationJob()
        {
            _db = new DataContext();
        }

        /// <summary>
        /// Executes the user deactivation process, any user that has not successfully authenticated within the specified time period will be deactivated.
        /// </summary>
        /// <remarks>The Hangfire job is set to attempt only once, and on fail only mark the job as failed.</remarks>
        /// <returns></returns>
        [Hangfire.AutomaticRetry(Attempts = 1, OnAttemptsExceeded = Hangfire.AttemptsExceededAction.Fail)]
        public async Task DeactivateStaleUsers()
        {
            int deactivateAfterDays = int.Parse(WebConfigurationManager.AppSettings["Users.DeactivateAfterNumberOfDays"]);
            DateTime deactivationCutoffDate = DateTime.UtcNow.AddDays(deactivateAfterDays * -1).Date;

            var staleUsers = await (from u in _db.Users
                                    where _db.LogsUserAuthentication.Where(l => l.Success && l.UserID == u.ID && deactivationCutoffDate < l.TimeStamp).Count() < 1
                                    && u.Active && u.DeactivatedOn == null && u.Deleted == false && u.UserType == UserTypes.User
                                    select
                                    new
                                    {
                                        User = u,
                                        Organization = u.Organization.Name,
                                        OrganizationAcronym = u.Organization.Acronym,
                                        LastSuccessfullAuthentication = _db.LogsUserAuthentication.Where(l => l.Success && l.UserID == u.ID).OrderByDescending(l => l.TimeStamp).FirstOrDefault()
                                    }
                                    ).ToArrayAsync();

            var validationErrorUsers = new Dictionary<Guid, string[]>();
            bool reportOnly = bool.Parse(WebConfigurationManager.AppSettings["Users.DeactivationServiceReportOnly"]);
            if (reportOnly == false)
            {
                foreach (var su in staleUsers)
                {
                    var user = su.User;

                    user.Active = false;
                    user.DeactivatedOn = DateTime.UtcNow;
                    user.DeactivationReason = $"User was deactivated via system service due to not having successfully authenticated within {deactivateAfterDays} days.";

                    var validationErrors = _db.GetValidationErrors();
                    if (validationErrors.Any())
                    {
                        //log validation error to email
                        var errors = validationErrors.SelectMany(err => err.ValidationErrors.Select(ex => ex.ErrorMessage)).ToArray();
                        validationErrorUsers.Add(user.ID, errors);

                        continue;
                    }

                    _db.LogsUserChange.Add(new UserChangeLog
                    {
                        UserID = Guid.Empty,
                        UserChangedID = user.ID,
                        Reason = EntityState.Modified,
                        Description = $"User '{ su.OrganizationAcronym }\\{ user.UserName }' was deactivated via system service due to not having successfully authenticated within { deactivateAfterDays } days."
                    });

                    await _db.SaveChangesAsync();
                }

                

                

            }

            string notifyAddresses = (WebConfigurationManager.AppSettings["Users.DeactivationNotificationEmail"] ?? "").Trim();
            if (notifyAddresses == null || notifyAddresses.IndexOf('@') < 1)
            {
                var smtpSection = (System.Net.Configuration.SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                notifyAddresses = smtpSection.From;
            }

            string networkName = WebConfigurationManager.AppSettings["Network.Name"];

            var message = new MailMessage
            {
                Subject = $"User Deactivation Service Results [{ networkName }]",
                IsBodyHtml = true
            };
            message.To.Add(notifyAddresses);

            var deactivatedUsersDetails = staleUsers.GroupBy(k => k.User.OrganizationID).SelectMany(g => g.Select(v => new { Organization = v.Organization, v.OrganizationAcronym, UserID = v.User.ID, v.User.UserName, v.LastSuccessfullAuthentication })).OrderBy(s => s.Organization).ThenBy(s => s.UserName).ToArray();

            var html = new System.Text.StringBuilder();
            html.AppendLine("<!doctype html><html><head>");
            html.AppendLine("<meta name=\"viewport\" content=\"width=device-width\" />");
            html.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            html.AppendLine("<title>User Deactivation Service Results - " + DateTime.Now.ToString("MM/dd/yyyy") + "</title>");
            html.AppendLine("<style>");
            html.AppendLine("table { text-align:left; }");
            html.AppendLine("td, th { padding-left:1.5rem; vertical-align:top; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine($"<body class=\"\"><div>For { networkName }, user deactivation service successfully run to deactivate users who have not successfully authenticated in the last <strong>{ deactivateAfterDays }</strong> days (since: { deactivationCutoffDate.ToString("MM/dd/yyyy")}). <strong>{ staleUsers.Length - validationErrorUsers.Count() }</strong> deactivated.<br/><br/></div>");
            if (reportOnly)
            {
                html.AppendLine("<p style=\"text-align:center;\"><strong>NOTE: The service is set to REPORT ONLY, no users were actually deactivated.</strong></p>");
            }
            html.AppendLine("<div><table><thead><tr>");
            html.AppendLine("<th>Organization</th><th>Username</th><th>Last Login</th><th>Source</th>");
            html.AppendLine("</tr></thead>");
            html.AppendLine("<tbody>");

            if (deactivatedUsersDetails.Length > 0)
            {
                foreach (var usr in deactivatedUsersDetails.Where(k => !validationErrorUsers.Keys.Contains(k.UserID)))
                {
                    html.AppendLine($"<tr><td>{ usr.Organization } ({ usr.OrganizationAcronym })</td><td>{ usr.UserName }</td><td>{ FormatLastLoginDate(usr.LastSuccessfullAuthentication) }</td><td>{ FormatLastLoginSource(usr.LastSuccessfullAuthentication) }</td></tr>");
                }
            }

            if(validationErrorUsers.Count > 0)
            {
                html.AppendLine("<tr><td colspan=\"4\"><strong>** ERRORS Updating Users **</strong></td></tr>");
                foreach(var pair in validationErrorUsers)
                {
                    var user = staleUsers.First(u => u.User.ID == pair.Key);
                    html.AppendLine($"<tr><td>{ user.Organization } ({ user.OrganizationAcronym })</td><td>{ user.User.UserName }</td><td colspan=\"2\">{ string.Join("<br/>", pair.Value) }</td></tr>");
                }
            }
            
            if(staleUsers.Length == 0)
            {
                html.AppendLine("<tr><td colspan=\"4\">No users found to deactivate.</td></tr>");
            }

            html.AppendLine("</tbody></table></div></body><html>");            

            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html.ToString(), new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Html)));


            var textBody = new System.Text.StringBuilder();
            textBody.AppendLine($"For { networkName }, user deactivation service successfully run at { DateTime.Now.ToString("g") } to deactivate users who have not successfully authenticated in the last { deactivateAfterDays } days ({ deactivationCutoffDate.ToString("MM/dd/yyyy")}). { staleUsers.Length - validationErrorUsers.Count() } deactivated.");
            textBody.AppendLine(" ");
            if (reportOnly)
            {
                textBody.AppendLine("### NOTE: The service is set to REPORT ONLY, no users were actually deactivated. ###");
            }
            textBody.AppendLine(" ");

            int padOrganization = deactivatedUsersDetails.Select(u => string.Format("{0} ({1})", u.Organization, u.OrganizationAcronym)).Distinct().Max(m => m == null ? "Organization".Length : m.Length) + 3;
            int padUsername = Math.Max("Username".Length, deactivatedUsersDetails.Select(u => u.UserName).Distinct().Max(m => m == null ? "Username".Length : m.Length)) + 3;
            int padLastLogin = 13;
            int padLastLoginSource = deactivatedUsersDetails.Where(u => u.LastSuccessfullAuthentication != null && !string.IsNullOrEmpty(u.LastSuccessfullAuthentication.Source)).Select(u => u.LastSuccessfullAuthentication.Source).Distinct().DefaultIfEmpty().Max(u => u == null ? "Source".Length : u.Length) + 3;

            textBody.AppendLine("Organization (Acronym)".PadRight(padOrganization) + "Username".PadRight(padUsername) + "LastLogin".PadRight(padLastLogin) + "Source".PadRight(padLastLoginSource));
            textBody.AppendLine(new string('-', padOrganization + padUsername + padLastLogin + padLastLoginSource));

            if (deactivatedUsersDetails.Length > 0)
            {
                foreach (var usr in deactivatedUsersDetails.Where(k => !validationErrorUsers.Keys.Contains(k.UserID)))
                {
                    textBody.AppendLine(string.Format("{0} ({1})", usr.Organization, usr.OrganizationAcronym).PadRight(padOrganization) + usr.UserName.PadRight(padUsername) + FormatLastLoginDate(usr.LastSuccessfullAuthentication).PadRight(padLastLogin) + FormatLastLoginSource(usr.LastSuccessfullAuthentication).PadRight(padLastLoginSource));
                }
            }

            if (validationErrorUsers.Count > 0)
            {
                textBody.AppendLine("#####** ERRORS Updating Users **#####");
                foreach (var pair in validationErrorUsers)
                {
                    var user = staleUsers.First(u => u.User.ID == pair.Key);
                    textBody.AppendLine(string.Format("{0} ({1})", user.Organization, user.OrganizationAcronym).PadRight(padOrganization) + user.User.UserName.PadRight(padUsername));
                    foreach(var err in pair.Value)
                    {
                        textBody.AppendLine(err.PadLeft(err.Length + 8));
                    }
                    textBody.AppendLine("");
                }
            }

            if (staleUsers.Length == 0)
            {
                textBody.AppendLine("##### No users to deactivate #####");
            }

            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(textBody.ToString(), new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain)));

            message.AlternateViews[0].TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
            message.AlternateViews[1].TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
            }
        }

        static string FormatLastLoginSource(Data.Audit.UserAuthenticationLogs log)
        {
            if (log == null)
                return string.Empty;

            return log.Source ?? "";
        }

        static string FormatLastLoginDate(Data.Audit.UserAuthenticationLogs log)
        {
            if (log == null)
                return string.Empty;

            return log.TimeStamp.ToString("MM/dd/yyyy");
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected virtual void Dispose(bool disposing)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
                }

                disposedValue = true;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void Dispose()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}