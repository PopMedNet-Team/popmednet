using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.ComponentModel.Composition;
using Lpp.Mvc;

namespace Lpp.Mvc
{
    public abstract class BaseController : Controller
    {
        protected ActionResult RedirectToAction<TController>( Expression<Func<TController, object>> action )
            where TController : IController
        {
            return Redirect( ActionUrl<TController>( action ) );
        }

        protected ActionResult RedirectToAction<TController>( Expression<Func<TController, object>> action, object routeValues = null )
            where TController : IController
        {
            return Redirect( ActionUrl<TController>( action, routeValues ) );
        }

        protected string ActionUrl<TController>( Expression<Func<TController, object>> action, object routeValues = null )
            where TController : IController
        {
            return new UrlHelper( Request.RequestContext ).Action<TController>( action, routeValues );
        }

        protected string ActionUrl<TController>( Expression<Func<TController, object>> action )
            where TController : IController
        {
            return new UrlHelper( Request.RequestContext ).Action<TController>( action );
        }

        protected ActionResult ReturnTo( string url )
        {
            var rfr = Request.UrlReferrer;
            return Redirect( 
                !string.IsNullOrEmpty( url ) ? url : 
                rfr != null ? rfr.ToString() : 
                "~/" );
        }

        protected TypedControllerExtensions.ViewBuilder<TView> View<TView>() where TView : WebViewPage
        {
            return new TypedControllerExtensions.ViewBuilder<TView> { Controller = this };
        }

        internal ActionResult ViewRef( string name, object model ) { return View( name, model ); }

        protected new ActionResult Json( object obj )
        {
            return Content( Newtonsoft.Json.JsonConvert.SerializeObject(obj), "application/json" );
        }
    }

    public static class TypedControllerExtensions
    {
        public static RedirectResult RedirectToAction<TController>(this TController c, Expression<Func<TController,object>> action)
            where TController : Controller
        {
            return new RedirectResult( new UrlHelper( c.Request.RequestContext ).Action<TController>( action ) );
        }

        public static ViewBuilder<TView> View<TView>( this BaseController ctrl ) where TView : WebViewPage
        {
            return new ViewBuilder<TView> { Controller = ctrl };
        }

        public static ActionResult WithModel<TView, TModel>( this ViewBuilder<TView> vb, TModel model ) 
            where TView : WebViewPage<TModel>
        {
            return vb.Controller.ViewRef( typeof( TView ).AssemblyQualifiedName, model );
        }

        public static ActionResult WithoutModel<TView>( this ViewBuilder<TView> vb )
        {
            return vb.Controller.ViewRef( typeof( TView ).AssemblyQualifiedName, null );
        }

        public class ViewBuilder<TView> { public BaseController Controller { get; set; } }
    }
}