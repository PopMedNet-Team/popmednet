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
using Lpp.Composition;

namespace Lpp.Mvc
{
    public static class AutoRoutingExtensions
    {
        public static void MapAutomaticRoutes( this RouteCollection routes, ICompositionService comp )
        {
            //Contract.Requires( routes != null );
            //Contract.Requires( comp != null );

            foreach ( var type in
                comp.GetMany<Lazy<IController, IComposableControllerMetadata>>()
                .Where( c => c.Metadata.AutoRoute )
                .Select( c => c.Value.GetType() ) )
            {
                routes.Add( new Route( AutoRouteFor( type ), 
                    new RouteValueDictionary { { "controller", type.AssemblyQualifiedName } },
                    new MvcRouteHandler() ) );
            }
        }

        static string AutoRouteFor( Type tController )
        {
            var asmName = Path.GetFileNameWithoutExtension( tController.Assembly.Location );
            var ctrName = tController.IsGenericType ?
                string.Format( "{0}.{1}[{2}]", tController.Namespace, tController.Name.Split('`').First(), string.Join( ",", tController.GetGenericArguments().Select( t => t.Name ) ) )
                : tController.FullName;
            if ( ctrName.StartsWith( asmName+".", StringComparison.InvariantCultureIgnoreCase ) )
            {
                ctrName = "-/" + ctrName.Substring( asmName.Length + 1 );
            }
            return string.Format( "__auto/{0}/{1}/{{action}}", asmName, ctrName );
        }
    }
}