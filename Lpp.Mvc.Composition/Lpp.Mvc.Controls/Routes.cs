using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using System.Web.Mvc;
using System.Diagnostics.Contracts;

namespace Lpp.Mvc.Controls
{
    [Export(typeof(IRouteRegistrar))]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes( RouteCollection routes )
        {
            routes.MapResources( this.GetType().Assembly );
        }

        public void RegisterCatchAllRoutes( RouteCollection routes )
        {
        }
    }
}