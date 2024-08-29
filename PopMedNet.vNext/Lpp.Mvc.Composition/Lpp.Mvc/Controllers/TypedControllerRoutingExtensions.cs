using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Collections.Generic;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public static class TypedControllerRoutingExtensions
    {
        public static string Action<TController>( this UrlHelper url, Expression<Func<TController, object>> action, RouteValueDictionary routeValues )
            where TController : IController
        {
            //Contract.Requires( action != null );
            return url.Action<TController>( action.MemberName(), routeValues );
        }

        public static string Action<TController>( this UrlHelper url, string action, RouteValueDictionary routeValues )
            where TController : IController
        {
            //Contract.Requires( !String.IsNullOrEmpty( action ) );
            return url.Action( typeof( TController ), action, routeValues );
        }

        public static string Action( this UrlHelper url, Type controllerType, string action, RouteValueDictionary routeValues )
        {
            //Contract.Requires( url != null );
            //Contract.Requires( !String.IsNullOrEmpty( action ) );

            return url.Action( action, controllerType.AssemblyQualifiedName, routeValues );
        }

        public static string Action<TController>( this UrlHelper url, Expression<Func<TController, object>> action )
            where TController : IController
        {
            //Contract.Requires( action != null );
            return url.Action<TController>( action, action.CallParameters().CoerceRouteValues() );
        }

        public static string Action<TController>( this UrlHelper url, Expression<Func<TController, object>> action, object routeValues = null )
            where TController : IController
        {
            return url.Action<TController>( action, routeValues.ToRouteValues() );
        }

        public static string Action<TController>( this UrlHelper url, string action, object routeValues = null )
            where TController : IController
        {
            return url.Action<TController>( action, routeValues.ToRouteValues() );
        }

        public static string Action( this UrlHelper url, Type controllerType, string action, object routeValues = null )
        {
            return url.Action( controllerType, action, routeValues.ToRouteValues() );
        }

        public static MvcForm BeginForm<TController>( this HtmlHelper html, Expression<Func<TController, object>> action, FormMethod method = FormMethod.Post, object attributes = null )
        {
            //Contract.Requires( html != null );
            //Contract.Requires( action != null );
            return html.BeginForm( action.MemberName(), typeof( TController ).AssemblyQualifiedName,
                action.CallParameters().CoerceRouteValues(), method, attributes.ToRouteValues() );
        }

        public static MvcForm BeginForm<TController>( this HtmlHelper html, string actionName, object routeValues, FormMethod method = FormMethod.Post, object attributes = null )
        {
            //Contract.Requires( html != null );
            //Contract.Requires( !String.IsNullOrEmpty( actionName ) );
            return html.BeginForm( actionName, typeof( TController ).AssemblyQualifiedName, routeValues, method, attributes );
        }

        static readonly RouteValueDictionary _emptyDictionary = new RouteValueDictionary();
        public static void MapRouteFor<TController>( this RouteCollection routes, string url,
            RouteValueDictionary defaults, RouteValueDictionary constraints = null )
            where TController : class, IController
        {
            //Contract.Requires( routes != null );
            //Contract.Requires( url != null );

            routes.Add( new Route( url,
                new RouteValueDictionary( defaults ?? _emptyDictionary ) { { "controller", typeof( TController ).AssemblyQualifiedName } },
                constraints, new MvcRouteHandler()
            ) );
        }

        public static void MapRouteFor<TController>( this RouteCollection routes, string url,
            object defaults = null, object constraints = null )
            where TController : class, IController
        {
            //Contract.Requires( routes != null );
            //Contract.Requires( url != null );
            routes.MapRouteFor<TController>( url, 
                defaults == null ? null : new RouteValueDictionary( defaults ), 
                constraints == null ? null : new RouteValueDictionary( constraints ) );
        }

        internal static RouteValueDictionary ToRouteValues( this object rv )
        {
            return rv == null ? null : new RouteValueDictionary( rv );
        }

        internal static RouteValueDictionary CoerceRouteValues( this IDictionary<string, object> parameters )
        {
            var res = 
                parameters.SelectMany( pk =>
                    pk.Value != null && !pk.Value.GetType().IsPrimitive && !pk.Value.GetType().IsEnum && 
                    !(pk.Value is string) && !(pk.Value is Guid) && !HasBinder( pk.Value.GetType() )
                    ? new RouteValueDictionary( pk.Value ).AsEnumerable()
                    : new[] { pk }
                )
                .Where( pk => pk.Value != null )
                .Distinct( pk => pk.Key, StringComparer.InvariantCultureIgnoreCase )
                .ToDictionary( pk => pk.Key, pk => pk.Value, StringComparer.InvariantCultureIgnoreCase );

            return new RouteValueDictionary( res );
        }

        static bool HasBinder( Type t )
        {
            return Memoizer.Memoize( t, _ => new
            {
                hasBinder = 
                    t.GetCustomAttributes( typeof( ModelBinderAttribute ), true ).Any() ||
                    System.Web.Mvc.ModelBinders.Binders.ContainsKey( t )
            } )
            .hasBinder;
        }
    }
}