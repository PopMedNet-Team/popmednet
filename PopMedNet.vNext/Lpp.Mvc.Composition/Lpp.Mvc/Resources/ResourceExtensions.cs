using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using System.Web;
using System.Reflection;
using System.Web.Routing;
using System.IO;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public static class ResourceExtensions
    {
        public static string Resource( this WebViewPage page, string fileName )
        {
            //Contract.Requires( page != null );
            //Contract.Requires( !String.IsNullOrEmpty( fileName ) );
            return page.Url.Resource( page.GetType().Assembly, fileName );
        }

        public static string Resource( this WebPageRenderingBase page, string fileName )
        {
            //Contract.Requires( page != null );
            //Contract.Requires( !String.IsNullOrEmpty( fileName ) );
            return new UrlHelper( page.Context.Request.RequestContext ).Resource( page.GetType().Assembly, fileName );
        }

        public static string Resource( this UrlHelper url, Assembly assembly, string fileName, object routeValues = null )
        {
            //Contract.Requires( url != null );
            //Contract.Requires( assembly != null );
            //Contract.Requires( !String.IsNullOrEmpty( fileName ) );

            var rvd = new RouteValueDictionary( routeValues );
            rvd[ResourceRoute.ResourceNameRouteDictionaryKey] = fileName;

            var themeService = DependencyResolver.Current.GetService<IThemeService>();
            var theme = themeService == null ? null : themeService.CurrentTheme;
            if ( !string.IsNullOrEmpty( theme ) )
            {
                rvd[ResourceRoute.ThemeRouteDictionaryKey] = theme;
            }

            var res =
                url.RouteCollection
                .OfType<ResourceRoute>()
                .Select( r => r.GetVirtualPathForAssembly( assembly, url.RequestContext, rvd ) )
                .Where( vp => vp != null )
                .Select( vp => url.Content( "~/" + vp.VirtualPath ) )
                .FirstOrDefault();

            if ( res.NullOrEmpty() ) throw new InvalidOperationException( "Cannot find a route for resource " + fileName + ". Please make sure that a call to .MapResources() is included in your routes setup and that the file in question has a build action of 'Embedded Resource'" );
            return res;
        }

        public static void MapResources( this RouteCollection routes, Assembly assembly, string url = null, object defaultValues = null, object constraints = null )
        {
            //Contract.Requires( routes != null );
            //Contract.Requires( assembly != null );

            var defaults = new RouteValueDictionary( defaultValues );
            if ( !defaults.ContainsKey( ResourceRoute.ThemeRouteDictionaryKey ) )
            {
                defaults[ResourceRoute.ThemeRouteDictionaryKey] = ResourceRoute.DefaultThemePlaceholder;
            }

            routes.Add( new ResourceRoute( assembly,
                url ?? 
                    string.Format( "__r/{0}/{{{1}}}/{{*{2}}}", Path.GetFileNameWithoutExtension( assembly.Location ),
                        ResourceRoute.ThemeRouteDictionaryKey, ResourceRoute.ResourceNameRouteDictionaryKey
                    ),
                defaults, 
                constraints == null ? null : new RouteValueDictionary( constraints ) ) );
        }
    }
}