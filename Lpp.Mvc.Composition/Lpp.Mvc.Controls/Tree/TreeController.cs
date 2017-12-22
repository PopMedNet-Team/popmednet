using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    [Export, ExportController, AutoRoute, CompiledViews( typeof( Lpp.Mvc.Views.Tree.Tree ) )]
    class TreeController : Controller
    {
        public ActionResult Children( string id, RoutedComputation<LoadSubForest> cb )
        {
            return cb.Compute()( id );
        }

        protected override IActionInvoker CreateActionInvoker()
        {
            return new AjaxResponseActionInvoker();
        }

        /// <summary>
        /// This custom action invoker intercepts response messages and cuts out status codes other than 200,
        /// passing them back to the client script in the JSON form, so that the client script can then decide
        /// whether how to display the response. This is necessary to prevent arbitrary stuff showing up as
        /// tree nodes (i.e. login pages, redirect prompts, access denied messgaes, etc.)
        /// 
        /// TODO: Figure out how to package this in a reusable way and use everywhere AJAX reloads happen
        /// </summary>
        class AjaxResponseActionInvoker : ControllerActionInvoker
        {
            protected override AuthorizationContext InvokeAuthorizationFilters( ControllerContext controllerContext, IList<IAuthorizationFilter> filters, ActionDescriptor actionDescriptor )
            {
                var ctx = base.InvokeAuthorizationFilters( controllerContext, filters, actionDescriptor );

                var url = RedirectUrlFrom( controllerContext, ctx.Result );
                if ( !url.NullOrEmpty() ) ctx.Result = new AjaxJsonResponseResult( new
                {
                    code = "auth",
                    message = "Your session is no longer valid.<br/>Please refresh the page to <a href=\"" + url + "\">log in</a>.",
                    redirectTo = url
                } );

                return ctx;
            }

            protected override void InvokeActionResult( ControllerContext controllerContext, ActionResult actionResult )
            {
                var code = actionResult as HttpStatusCodeResult;
                var rdUrl = RedirectUrlFrom( controllerContext, actionResult );

                if ( !rdUrl.NullOrEmpty() ) actionResult = new AjaxJsonResponseResult( new { code = "redirect", message = "The content has been moved <a href=\"" + rdUrl + "\">here</a>", redirectTo = rdUrl } );
                if ( code != null && code.StatusCode != 200 ) actionResult = new AjaxJsonResponseResult( new { code = code.StatusCode.ToString(), message = code.StatusDescription } );

                try
                {
                    base.InvokeActionResult( controllerContext, actionResult );
                }
                catch ( Exception ex )
                {
                    new AjaxJsonResponseResult( new { code = "500", message = ex.Message, redirectTo = "" } ).ExecuteResult( controllerContext );
                }
            }

            string RedirectUrlFrom( ControllerContext ctx, ActionResult actionResult )
            {
                var rd = actionResult as RedirectResult;
                var rdrt = actionResult as RedirectToRouteResult;
                return
                    rd != null ? rd.Url :
                    rdrt != null ? UrlHelper.GenerateUrl( rdrt.RouteName, null, null, rdrt.RouteValues, RouteTable.Routes, ctx.RequestContext, false )
                    : null;
            }
        }
    }
}