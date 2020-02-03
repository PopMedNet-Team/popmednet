using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.RedirectBridge
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        public static string GetRequestServiceUrl( string restOrSoap ) { return string.Format( "api/{0}/request", restOrSoap ); }
        public static string GetResponseServiceUrl( string restOrSoap ) { return string.Format( "api/{0}/response", restOrSoap ); }

        public void RegisterRoutes( RouteCollection routes )
        {
            routes.MapRouteFor<Controllers.PluginsController>( "models/list/{page}", new { action = "List", page = 0 } );
            routes.MapRouteFor<Controllers.PluginsController>( "model/{modelId}", new { action = "View" }, new { modelId = "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$" } );
            routes.MapRouteFor<Controllers.PluginsController>( "models/{action}" );
            routes.MapResources( this.GetType().Assembly );

            routes.MapRouteFor<Controllers.DocumentController>( "api/document/{sessionToken}/{id}", new { action = "Document" }, new { id = "^\\d+$" } );
        }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
            routes.MapRestService<RedirectModelServices, IRequestService>( GetRequestServiceUrl( "rest" ) );
            routes.MapRestService<RedirectModelServices, IResponseService>( GetResponseServiceUrl( "rest" ) );

            routes.MapSoapService<RedirectModelServices, IRequestService>( GetRequestServiceUrl( "soap" ) );
            routes.MapSoapService<RedirectModelServices, IResponseService>( GetResponseServiceUrl( "soap" ) );
        }
    }
}