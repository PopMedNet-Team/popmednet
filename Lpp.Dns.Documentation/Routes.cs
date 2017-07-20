using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.Portal
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes( RouteCollection routes )
        {
            routes.MapResources( this.GetType().Assembly, "Documentation/{*" + ResourceRoute.ResourceNameRouteDictionaryKey + "}" );
        }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
        }
    }
}