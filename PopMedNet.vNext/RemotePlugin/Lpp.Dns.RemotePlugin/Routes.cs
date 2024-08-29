using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.RemotePlugin
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        public static string GetRemotePluginServiceUrl( string restOrSoap ) { return string.Format( "api/{0}/remote", restOrSoap ); }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
            routes.MapRestService<RemotePluginService, IRemotePluginService>( GetRemotePluginServiceUrl( "rest" ) );
            routes.MapSoapService<RemotePluginService, IRemotePluginService>( GetRemotePluginServiceUrl( "soap" ) );
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapResources( this.GetType().Assembly );
        }
    }
}