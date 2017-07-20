using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Web.Routing;
using Lpp.Dns.Scheduler;
using Lpp.Mvc;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.IdentityModel.Selectors;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Lpp.Dns.Portal.Controllers;
using Lpp.Dns.LocalDataMart;

namespace Lpp.Dns.Portal
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        [Import] public ICompositionService Composition { get; set; }

        public void RegisterRoutes( RouteCollection routes )
        {

            const string Number = "^\\d+$";
            const string Guid = @"^(([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12})$";
            
            routes.MapRouteFor<Controllers.HomeController>( "", new { action = "Index" } );
            routes.MapRouteFor<Controllers.HomeController>( "login", new { action = "Login" } );
            routes.MapRouteFor<Controllers.HomeController>("account/login", new { action = "Login" });//this forces redirects to account/login to be caught by LppDnsAuthorize, which is necessary for the sso and session timeout
            routes.MapRouteFor<Controllers.HomeController>("ssologin", new { action = "SsoLogin" });
            routes.MapRouteFor<Controllers.HomeController>( "logout", new { action = "Logout" } );
            routes.MapRouteFor<Controllers.HomeController>( "PasswordExpired", new { action = "PasswordExpired" } );
            routes.MapRouteFor<Controllers.HomeController>("resources", new { action = "Resources" });
            routes.MapRouteFor<Controllers.HomeController>("notyetimplemented", new { action = "NotYetImplemented" });
            routes.MapRouteFor<Controllers.HomeController>("restorepassword", new { action = "RestorePassword" });

            //routes.MapRouteFor<Controllers.RegistrationController>("registration/{action}", new { action = "SignUp" });

            routes.MapRouteFor<Controllers.RequestController>( "request/createdialog", new { action = "CreateDialog" });
            routes.MapRouteFor<Controllers.RequestController>( "request/selectproject", new { action = "SelectProjectDialog" });
            routes.MapRouteFor<Controllers.RequestController>( "request/history", new { action = "RoutingHistory" });
            routes.MapRouteFor<Controllers.RequestController>( "request/create/{requestTypeID}", new { action = "Create" } );
            routes.MapRouteFor<Controllers.RequestController>( "request/{requestID}", new { action = "RequestView" }, new { requestId = Guid, folder = "" } );
            routes.MapRouteFor<Controllers.RequestController>( "request/{requestID}/in/{folder}", new { action = "RequestView" }, new { requestID = Guid } );
            routes.MapRouteFor<Controllers.RequestController>( "request/{action}/{requestID}", null, new { requestID = Guid } );
            routes.MapRouteFor<Controllers.RequestsController>("requests/index", new { action = "Index" });
            routes.MapRouteFor<Controllers.RequestController>( "requests/{folder}", new { action = "SearchFolder" }, new { folder = "^" + string.Join( "|", Enum.GetValues( typeof( RequestSearchFolder ) ).Cast<RequestSearchFolder>().Select( f => "(" + f + ")" ) ) + "$" } );
            routes.MapRouteFor<Controllers.RequestController>( "request/editrequestmetadatadialog", new { action = "EditRequestMetadataDialog" });

            routes.MapRouteFor<Controllers.ResponseController>("response/{requestID}/external/{responseToken}/{aggregationMode}", new { action = "External", responseToken = UrlParameter.Optional, aggregationMode = UrlParameter.Optional }, new { requestID = Guid });
            routes.MapRouteFor<Controllers.ResponseController>( "response/{requestID}/{responseToken}/{aggregationMode}", new { action = "Detail", responseToken = UrlParameter.Optional, aggregationMode = UrlParameter.Optional }, new { requestID = Guid } );
            routes.MapRouteFor<Controllers.ResponseController>("response/{requestID}/export/{formatID}/{responseToken}/{aggregationMode}", new { action = "Export", responseToken = UrlParameter.Optional, aggregationMode = UrlParameter.Optional }, new { requestID = Guid });
            routes.MapRouteFor<Controllers.ResponseController>( "history/{requestID}/{responseID}", new { action = "History" }, new { requestID = Number, responseID = Guid } );
            routes.MapRouteFor<Controllers.DocumentController>( "document/{id}/download", new { action = "Download" }, new { id = Guid } );
            routes.MapRouteFor<Controllers.DocumentController>( "document/{id}/view", new { action = "Visualize" }, new { id = Guid } );

            //routes.MapRouteFor<Controllers.ReportsController>( "report/datamartaudit/{action}", new { action = "LegacyReport" }, new { action = "LegacyReport" } );
            //routes.MapRouteFor<Controllers.ReportsController>("report/NetworkActivityReport/{action}", new { action = "NetworkActivityReport" }, new { action = "NetworkActivityReport" });
            //routes.MapRouteFor<Controllers.ReportsController>("report/NetworkActivityReport/{action}", new { action = "NetworkActivityReportResults" }, new { action = "NetworkActivityReportResults" });

            //routes.MapRouteFor<Controllers.ReportsController>("report/Lists/{action}");

            //routes.MapRouteFor<Controllers.ReportsController>( "report/events/{action}", new { action = "Create" }, new { action = "Create" } );

            routes.CrudRoutes<Controllers.SharedFolderController>( "sharedfolder", idValidationRegex: Guid );
            routes.MapRouteFor<Controllers.SharedFolderController>( "requests/shared/{folderID}", new { action = "Contents" }, new { folderID = Guid } );
            
            routes.MapResources( this.GetType().Assembly );
        }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
            routes.MapSoapService<RequestSchedulerService, ISchedulerService>( "api/soap/scheduler" );
            routes.MapSoapService<LocalDataMartService, ILocalDataMartWcfService>("api/soap/localdm");
            routes.MapSoapService<RequestMetadataCollectionService, IRequestMetadataCollectionWcfService>("api/soap/requestmetadatacollection");
            routes.MapSoapService<SftpUploadService, ISftpUploadWcfService>("api/soap/sftpupload");
        }
    }
}