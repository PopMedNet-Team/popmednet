using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using Lpp.Mvc;

namespace Lpp.Security.UI
{
    [Export( typeof( IRouteRegistrar ) )]
    class Routes : IRouteRegistrar
    {
        public void RegisterRoutes( System.Web.Routing.RouteCollection routes ) { routes.MapResources( this.GetType().Assembly ); }
        public void RegisterCatchAllRoutes( System.Web.Routing.RouteCollection routes ) { }
    }
}