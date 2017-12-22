using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.Contracts;

namespace Lpp.Mvc
{
    public interface IRouteRegistrar
    {
        void RegisterRoutes( RouteCollection routes );
        void RegisterCatchAllRoutes( RouteCollection routes );
    }

    public static class RouteRegistrar
    {
        public static IRouteRegistrar Create( Action<RouteCollection> registerRoutes, Action<RouteCollection> registerCatchAll = null )
        {
            //Contract.Requires( registerRoutes != null );
            //Contract.Ensures( //Contract.Result<IRouteRegistrar>() != null );
            return new R( registerRoutes, registerCatchAll );
        }

        class R : IRouteRegistrar
        {
            private readonly Action<RouteCollection> _r, _c;
            public R( Action<RouteCollection> r, Action<RouteCollection> c ) { //Contract.Requires( r != null ); 
                _r = r; _c = c; }
            public void RegisterRoutes( RouteCollection routes ) { _r( routes ); }
            public void RegisterCatchAllRoutes( RouteCollection routes ) { if ( _c != null ) _c( routes ); }
        }
    }
}