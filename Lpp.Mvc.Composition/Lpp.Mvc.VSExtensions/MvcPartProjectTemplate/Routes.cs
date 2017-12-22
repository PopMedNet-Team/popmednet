using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace $safeprojectname$
{
    [Export(typeof(IRouteRegistrar))]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes( RouteCollection routes )
        {
            routes.MapRouteFor<Controllers.$PartName$Controller>( "$PartName$/{action}", new { action = "Index" } );
            routes.MapResources( this.GetType().Assembly );
        }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
        }
    }
}