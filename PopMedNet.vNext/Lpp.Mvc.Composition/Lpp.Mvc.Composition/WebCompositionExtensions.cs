using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.Reactive.Linq;
using System.Threading;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Routing;
using System.IO;

namespace Lpp.Mvc
{
    public static class WebCompositionExtensions
    {
        const string ContextCompositionKey = "{F8AF6FE9-DFD7-481B-B80B-BB426608FE1C}";

        public static void SetComposition( this HttpContextBase ctx, ICompositionService comp )
        {
            //Contract.Requires( ctx != null );
            //Contract.Requires( comp != null );
            ctx.Items[ContextCompositionKey] = comp;
        }

        public static ICompositionService Composition( this HttpContextBase ctx )
        {
            //Contract.Requires( ctx != null );
            return ctx.Items[ContextCompositionKey] as ICompositionService;
        }

        public static ICompositionService Composition( this HttpContext ctx )
        {
            //Contract.Requires( ctx != null );
            return ctx.Items[ContextCompositionKey] as ICompositionService;
        }

        public static void DisposeComposition( this HttpContextBase ctx )
        {
            //Contract.Requires( ctx != null );
            var c = ctx.Items[ContextCompositionKey] as IDisposable;
            if ( c != null ) c.Dispose();
        }
    }
}