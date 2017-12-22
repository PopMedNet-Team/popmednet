using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.PubHealth
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

