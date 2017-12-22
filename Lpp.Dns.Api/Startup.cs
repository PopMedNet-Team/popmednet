using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;
using Lpp.Utilities.WebSites.Hubs;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.Security;
[assembly: OwinStartup(typeof(Lpp.Dns.Api.Startup))]
namespace Lpp.Dns.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
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
    }
}