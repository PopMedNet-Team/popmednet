using Lpp.Dns.Data;
using Lpp.Dns.WebServices;
using Lpp.Utilities.WebSites;
using Lpp.Utilities.WebSites.Attributes;
using Lpp.Utilities.WebSites.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lpp.Utilities.WebSites.Security;
using Lpp.Dns.DTO.Security;
using System.Web.OData.Extensions;
using System.Web.Configuration;
using System.Reflection;
using Lpp.Utilities.WebSites.Hubs;
using System.IO;

namespace Lpp.Dns.Api
{
#pragma warning disable 1591
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //force load all assemblies to ensure availability
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            loadedAssemblies
                .SelectMany(x => x.GetReferencedAssemblies())
                .Distinct()
                .Where(y => loadedAssemblies.Any((a) => a.FullName == y.FullName) == false)
                .ToList()
                .ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));


            //This initializes the data context and force loads all of the DLLs that are being lazy loaded.
            using (var db = new DataContext())
            {
                db.Users.Where(u => u.ID == Guid.Empty).FirstOrDefault();
            }

            RouteTable.Routes.Clear();

            AreaRegistration.RegisterAllAreas();
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.AddODataQueryFilter();
            GlobalConfiguration.Configuration.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //Add basic auth. All functions are secure by default unless otherwise attributed with allowanonymous
            GlobalConfiguration.Configuration.MessageHandlers.Add(new BasicAuthenticationHandler<DataContext, PermissionDefinition>());
            GlobalConfiguration.Configuration.Filters.Add(new BasicAuthentication());


            //Validation so that intelligent errors are returned.            
            GlobalConfiguration.Configuration.Filters.Add(new ValidationActionFilter());
            GlobalConfiguration.Configuration.Filters.Add(new UnwrapExceptionFilterAttribute());
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            //SSL Requirement
            GlobalConfiguration.Configuration.MessageHandlers.Add(new RequireHttpsMessageHandler());

            //Fix for jquery returning error on OK/Accept if no json content included even if just {}
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HttpResponseMessageHandler());

            //Remove the default json formatter that is broken.
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.JsonFormatter);

            //Add our Json formatter that works properly including with Dates
            //Insert makes it the first choice for serialization, however the end user can request xml as well.
            GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonNetFormatter());
            
#if(DEBUG)
            //GlobalConfiguration.Configuration.EnableSystemDiagnosticsTracing();
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
#else
            this.Error += WebApiApplication_Error;
#endif       
     
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            GlobalConfiguration.Configuration.EnsureInitialized();

            //Registers the notifier that will send emails etc on notifications.
            DataContext.RegisteryNotifier(new Notifier());

            //IModelTerm interrogation
            System.Threading.Tasks.Task.Run(() =>
            {
                string appDomainFolder = System.Web.Configuration.WebConfigurationManager.AppSettings["TermsRegistrationManager.AppDomainRoot"] ?? string.Empty;
                if (string.IsNullOrEmpty(appDomainFolder))
                    appDomainFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Plugins");

                string packagesFolder = System.Web.Configuration.WebConfigurationManager.AppSettings["AdapterPackages.Folder"] ?? string.Empty;
                if (string.IsNullOrEmpty(packagesFolder))
                    packagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

                using (var mgr = new Lpp.QueryComposer.TermRegistration.TermsRegistrationManager(appDomainFolder, packagesFolder))
                {
                    mgr.Run();
                }

                string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsUploadFolder"] ?? string.Empty;

                if (string.IsNullOrEmpty(uploadPath))
                    uploadPath = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\Uploads\\");

                if (Directory.Exists(uploadPath))
                {
                    var files = Directory.GetFiles(uploadPath, "*.*", SearchOption.AllDirectories);
                    foreach(var file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        if (fi.LastAccessTime < DateTime.Now.AddHours(-12))
                            fi.Delete();
                    }
                }
            });


            HangfireBootstrapper.Instance.Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            HangfireBootstrapper.Instance.Stop();
        }

#if(!DEBUG)
        void WebApiApplication_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            //Log errors here
        }
#endif

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
#pragma warning restore 1591
}
