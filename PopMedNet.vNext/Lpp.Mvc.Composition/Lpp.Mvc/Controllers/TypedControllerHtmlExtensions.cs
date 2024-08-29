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
    public static class TypedControllerHtmlExtensions
    {
        public static void RenderAction<TController>( this HtmlHelper html, Expression<Func<TController, object>> action, RouteValueDictionary routeValues )
            where TController : IController
        {
            //Contract.Requires( action != null );
            html.RenderAction<TController>( action.MemberName(), routeValues );
        }

        public static void RenderAction<TController>( this HtmlHelper html, string action, RouteValueDictionary routeValues )
            where TController : IController
        {
            //Contract.Requires( !String.IsNullOrEmpty( action ) );
            html.RenderAction( typeof( TController ), action, routeValues );
        }

        public static void RenderAction( this HtmlHelper html, Type controllerType, string action, RouteValueDictionary routeValues )
        {
            //Contract.Requires( html != null );
            //Contract.Requires( !String.IsNullOrEmpty( action ) );

            html.RenderAction( action, controllerType.AssemblyQualifiedName, routeValues );
        }

        public static void RenderAction<TController>( this HtmlHelper html, Expression<Func<TController, object>> action )
            where TController : IController
        {
            //Contract.Requires( action != null );
            html.RenderAction<TController>( action, action.CallParameters().CoerceRouteValues() );
        }

        public static void RenderAction<TController>( this HtmlHelper html, Expression<Func<TController, object>> action, object routeValues = null )
            where TController : IController
        {
            html.RenderAction<TController>( action, routeValues.ToRouteValues() );
        }

        public static void RenderAction<TController>( this HtmlHelper html, string action, object routeValues = null )
            where TController : IController
        {
            html.RenderAction<TController>( action, routeValues.ToRouteValues() );
        }

        public static void RenderAction( this HtmlHelper html, Type controllerType, string action, object routeValues = null )
        {
            html.RenderAction( controllerType, action, routeValues.ToRouteValues() );
        }
    }
}