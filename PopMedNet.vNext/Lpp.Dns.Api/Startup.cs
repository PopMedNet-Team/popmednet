using System;
using System.Collections.Generic;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;
using Lpp.Utilities.WebSites.Hubs;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
using Hangfire;
using Hangfire.SqlServer;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(Lpp.Dns.Api.Startup))]
namespace Lpp.Dns.Api
{
    public class Startup
    {
        /// <summary>
        /// The Hangfire job ID for deactivating users.
        /// </summary>
        public const string HANGFIRE_DEACTIVATEUSERS_JOBID = "deactivate-users";

        public void Configuration(IAppBuilder app)
        {

            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            //Register Hangfire recurring jobs
            if (bool.Parse(WebConfigurationManager.AppSettings["Users.EnableDeactivationService"]))
            {
                string cronString = WebConfigurationManager.AppSettings["Users.DeactivationServiceCron"];
                Hangfire.RecurringJob.AddOrUpdate<Users.UserDeactivationJob>(HANGFIRE_DEACTIVATEUSERS_JOBID, j => j.DeactivateStaleUsers(), cronString, timeZone: TimeZoneInfo.Local);
            }
            else
            {
                Hangfire.RecurringJob.RemoveIfExists(HANGFIRE_DEACTIVATEUSERS_JOBID);
            }

            // Any connection or hub wire up and configuration should go here
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true,
                    EnableJSONP = true
                });
            });

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIDProvider<DataContext, PermissionDefinition>());
        }


        IEnumerable<IDisposable> GetHangfireServers()
        {
            using (var db = new Lpp.Dns.Data.DataContext())
            {
                GlobalConfiguration.Configuration
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(db.Database.Connection.ConnectionString);
            }

            yield return new BackgroundJobServer();
        }
    }
}