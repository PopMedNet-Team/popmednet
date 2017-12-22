using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Controllers;

namespace Lpp.Dns.Models.ESPQueryBuilder
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes( RouteCollection routes )
        {
            routes.MapRouteFor<ESPQueryController>("espquery/term/{term}", new { action = "Term" });
            routes.MapResources( this.GetType().Assembly );
        }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
        }
    }
}