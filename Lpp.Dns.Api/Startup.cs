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

[assembly: OwinStartup(typeof(Lpp.Dns.Api.Startup))]
namespace Lpp.Dns.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseHangfireAspNet(GetHangfireServers);
#if DEBUG
            app.UseHangfireDashboard();
#endif

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