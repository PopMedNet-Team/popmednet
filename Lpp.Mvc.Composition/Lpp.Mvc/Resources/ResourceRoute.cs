using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Lpp.Mvc.ModelBinders;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public class ResourceRoute : Route
    {
        public ResourceRoute( Assembly assembly, string url, RouteValueDictionary defaults = null, RouteValueDictionary constraints = null ) 
            : base( url, defaults, constraints, new ResourceRouteHandler( assembly ) )
        {
            _assembly = assembly;
        }

        private readonly Assembly _assembly;
        public const string ResourceNameRouteDictionaryKey = "resourceName";
        public const string ThemeRouteDictionaryKey = "theme";
        public const string DefaultThemePlaceholder = "-";

        public VirtualPathData GetVirtualPathForAssembly( Assembly a, RequestContext requestContext, RouteValueDictionary values )
        {
            return _assembly == a ? base.GetVirtualPath( requestContext, values ) : null;
        }

        public override VirtualPathData GetVirtualPath( RequestContext requestContext, RouteValueDictionary values )
        {
            return null;
        }
    }

    class ResourceRouteHandler : IRouteHandler
    {
        struct AssemblyDescription
        {
            public string[] ResourceNames { get; private set; }
            public Assembly Assembly { get; private set; }
            public AssemblyDescription( Assembly a ) : this()
            {
                //Contract.Requires( a != null );
                Assembly = a;
                ResourceNames = a.GetManifestResourceNames();
            }
        }

        public struct ResourceDescription
        {
            public string ResourceName { get; private set; }
            public Assembly Assembly { get; private set; }
            public bool IsValid { get { return ResourceName != null && Assembly != null; } }
            public static ResourceDescription Empty = new ResourceDescription();

            public ResourceDescription( string resourceName, Assembly assembly ) : this()
	        {
                //Contract.Requires(!String.IsNullOrEmpty(resourceName));
                //Contract.Requires(assembly != null);
                ResourceName = resourceName;
                Assembly = assembly;
	        }

            public Stream GetStream()
            {
                return IsValid ? Assembly.GetManifestResourceStream( ResourceName ) : null;
            }
        }

        readonly AssemblyDescription _root;
        readonly ConcurrentDictionary<string, AssemblyDescription> _themedAssemblies = new ConcurrentDictionary<string, AssemblyDescription>();

        public ResourceRouteHandler( Assembly assembly )
        {
            //Contract.Requires( assembly != null );
            _themedAssemblies[""] = _root = new AssemblyDescription( assembly );
        }

        public IHttpHandler GetHttpHandler( RequestContext requestContext )
        {
            return new ResourceHttpHandler( this, requestContext );
        }

        public ResourceDescription GetResource( string rname, string theme )
        {
            // TODO: Cache the result of this method
            rname = rname.Replace( '/', '.' ).Replace( '\\', '.' );

            var themedName = string.IsNullOrEmpty( theme ) ? null :
                ".Content." + Path.GetFileNameWithoutExtension( rname ) + "." + theme + Path.GetExtension( rname );
            rname = ".Content." + rname;

            string res = null;
            if ( !string.IsNullOrEmpty( theme ) )
            {
                var ad = _themedAssemblies.GetOrAdd( theme ?? "", GetThemedAssembly );
                if ( ad.Assembly != _root.Assembly )
                {
                    res = ad.ResourceNames.FirstOrDefault( n => n.EndsWith( rname, StringComparison.OrdinalIgnoreCase ) );
                    if ( res != null ) return new ResourceDescription( res, ad.Assembly );
                }
            }

            string r = null;
            if ( themedName != null ) r = _root.ResourceNames.FirstOrDefault( n => n.EndsWith( themedName, StringComparison.OrdinalIgnoreCase ) );
            if ( r == null ) r = _root.ResourceNames.FirstOrDefault( n => n.EndsWith( rname, StringComparison.OrdinalIgnoreCase ) );

            return r == null ? ResourceDescription.Empty : new ResourceDescription( r, _root.Assembly );
        }

        AssemblyDescription GetThemedAssembly( string theme )
        {
            try { return new AssemblyDescription( Assembly.Load( _root.Assembly.GetName().Name + "." + theme ) ); }
            catch { return _root; }
        }
    }

    class ResourceHttpHandler : IHttpHandler
    {
        private readonly ResourceRouteHandler _routeHandler;
        private readonly RequestContext _requestContext;
        const string CacheKeyPrefix = "ResourceHttpHandler.Resource.";

        public ResourceHttpHandler( ResourceRouteHandler rh, RequestContext rc )
        {
            //Contract.Requires( rh != null );
            //Contract.Requires( rc != null );
            _routeHandler = rh;
            _requestContext = rc;
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest( HttpContext context )
        {
            var theme = Convert.ToString( _requestContext.RouteData.Values.ValueOrDefault( ResourceRoute.ThemeRouteDictionaryKey ) );
            if ( theme == ResourceRoute.DefaultThemePlaceholder ) theme = null;

            var rname = _requestContext.RouteData.Values.ValueOrDefault( ResourceRoute.ResourceNameRouteDictionaryKey ) as string;
            var resource = string.IsNullOrEmpty( rname ) ? 
                ResourceRouteHandler.ResourceDescription.Empty : 
                _routeHandler.GetResource( rname, theme );
            var res = GetContent( context, resource );

            if ( res == null )
            {
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "Not Found. Make sure your resource file has a build action of 'Embedded Resource'";
                context.Response.Write( "Unable to find resource " + rname + ". Make sure your resource file has a build action of 'Embedded Resource'" );
                return;
            }

            var ims = IfModifiedSinceBinder.ParseHeader( context.Request );
            if ( ims != null && (res.LastModified - ims.Value).TotalSeconds < 1 )
            {
                context.Response.StatusCode = 304;
                context.Response.StatusDescription = "Not Modified";
                return;
            }

            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            context.Response.ContentType = MimeType.FromFileName( rname );
            context.Response.Cache.SetCacheability( HttpCacheability.Public );
            context.Response.Cache.SetLastModified( res.LastModified );
            context.Response.BinaryWrite( res.Content );
        }

        static TimestampedResource GetContent( HttpContext context, ResourceRouteHandler.ResourceDescription resource )
        {
            if ( !resource.IsValid ) return null;

            var key = CacheKeyPrefix + resource.Assembly.FullName + resource.ResourceName;
            var res = context.Cache.Get( key ) as TimestampedResource;
            if ( res == null )
            {
                using ( var stream = resource.GetStream() )
                {
                    if ( stream == null ) return null;
                    res = new TimestampedResource
                    {
                        Content = new byte[stream.Length],
                        LastModified = new FileInfo( resource.Assembly.Location ).LastWriteTime
                    };
                    stream.Read( res.Content, 0, res.Content.Length );
                }

                context.Cache.Insert( key, res );
            }

            return res;
        }

        class TimestampedResource
        {
            public byte[] Content { get; set; }
            public DateTime LastModified { get; set; }
        }
    }
}