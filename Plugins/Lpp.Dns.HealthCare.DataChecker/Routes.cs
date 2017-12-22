using Lpp.Mvc;
using System.ComponentModel.Composition;
using System.Web.Routing;

namespace Lpp.Dns.HealthCare.DataChecker
{
    [Export(typeof(IRouteRegistrar))]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapResources(this.GetType().Assembly);
        }

        public void RegisterCatchAllRoutes(RouteCollection routes)
        {
        }
    }
}

