using System.ComponentModel.Composition;
using Lpp.Mvc;

namespace Lpp.Dns.DocumentVisualizers
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes( System.Web.Routing.RouteCollection routes )
        {
            routes.MapResources( this.GetType().Assembly );
        }

        public void RegisterCatchAllRoutes( System.Web.Routing.RouteCollection routes )
        {
        }
    }
}
