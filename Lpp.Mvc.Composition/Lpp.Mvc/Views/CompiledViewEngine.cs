using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.WebPages.Razor;
using System.Web.Hosting;
using System.Collections.Concurrent;
using System.Web.WebPages;
using System.Reflection;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public class CompiledViewEngine : IViewEngine
    {
        public RazorViewEngine Fallback { get; set; }

        public CompiledViewEngine()
        {
            Fallback = new RazorViewEngine();
        }

        public ViewEngineResult FindPartialView( ControllerContext controllerContext, string partialViewName, bool useCache )
        {
            return FindView( controllerContext, partialViewName, false, () => Fallback.FindPartialView( controllerContext, partialViewName, useCache ) );
        }

        public ViewEngineResult FindView( ControllerContext controllerContext, string viewName, string masterName, bool useCache )
        {
            if ( !string.IsNullOrEmpty( masterName ) ) throw new NotSupportedException( "We don't support overriden layouts yet." );
            try
            {
                return FindView(controllerContext, viewName, true, () => Fallback.FindView(controllerContext, viewName, masterName, useCache));
            }
            catch
            {
                return Fallback.FindView(controllerContext, viewName, masterName, useCache);
            }
        }

        ViewEngineResult FindView( ControllerContext controllerContext, string viewName, bool runStartPages, Func<ViewEngineResult> fallback )
        {
            var isAssemblyQualifiedType = viewName.Contains( ',' );
            var ns = GetDefaultNamespace( controllerContext.Controller );
            if ( ns == null && !isAssemblyQualifiedType ) return CallFallback( controllerContext, fallback );
            if ( isAssemblyQualifiedType && viewName.StartsWith( "~/" ) ) viewName = viewName.Substring( 2 );

            var theme = GetTheme( controllerContext );
            var res = GetViewType( controllerContext.Controller, viewName, ns, theme );

            if ( res.Type == null )
            {
                var r = CallFallback( controllerContext, fallback );
                return r.View == null ? new ViewEngineResult( res.Paths.Concat( r.SearchedLocations ) ) : r;
            }

            var vpath = "~/" + res.Type.FullName.Replace( '.', '/' );
            var start = runStartPages ? LookupStartPage( controllerContext, res.Type, res.EffectiveTheme ) : null;

            return new ViewEngineResult( new CompiledView( vpath, res.Type, start ), this );
        }

        private Type LookupStartPage( ControllerContext controllerContext, Type page, string theme )
        {
            return _viewStartPages.GetOrAdd(
                controllerContext.Controller.GetType().AssemblyQualifiedName + " :: " + 
                page.AssemblyQualifiedName + " :: " + theme,
                _ =>
                {
                    var typePath = page.FullName;
                    var parts = typePath.Split( '.' );
                    var possiblePaths =
                        Enumerable.Range( 1, parts.Length-1 )
                        .Select( i => string.Join( ".", parts.Take( parts.Length - i ) ) + "._ViewStart, " + page.Assembly.FullName );
                    var type = possiblePaths
                        .Select( p => GetViewType( controllerContext.Controller, p, null, theme ).Type )
                        .Where( t => t != null && typeof( StartPage ).IsAssignableFrom( t ) )
                        .FirstOrDefault();

                    if ( type == null )
                    {
                        try { type = System.Web.Compilation.BuildManager.GetCompiledType( "~/Views/_ViewStart.cshtml" ); }
                        catch { }

                        if ( type != null && !typeof( StartPage ).IsAssignableFrom( type ) ) type = null;
                    }

                    return type;
                } );
        }

        ViewEngineResult CallFallback( ControllerContext ctx, Func<ViewEngineResult> fallback )
        {
            if ( !ctx.RouteData.Values.ContainsKey( "controller" ) )
            {
                var typeName = ctx.Controller.GetType().Name;
                if ( typeName.EndsWith( "Controller", StringComparison.InvariantCultureIgnoreCase ) )
                {
                    ctx.RouteData.Values["controller"] = typeName.Substring( 0, typeName.Length - "Controller".Length );
                }
                else
                {
                    ctx.RouteData.Values["controller"] = "__Absolutely_Meaningless_Controller_Name__";
                }
            }
            return fallback();
        }

        public void ReleaseView( ControllerContext controllerContext, IView view )
        {
            var d = view as IDisposable;
            if ( d != null ) d.Dispose();
        }

        string GetDefaultNamespace( IController controller )
        {
            //Contract.Requires( controller != null );

            var type = controller.GetType();
            return _defaultNamespaces.GetOrAdd( type.FullName, _ =>
                {
                    var attr = type.GetCustomAttributes( typeof( CompiledViewsAttribute ), true ).OfType<CompiledViewsAttribute>().FirstOrDefault();
                    if ( attr == null ) return null;

                    var ns = attr.DefaultNamespace;
                    if ( string.IsNullOrEmpty( ns ) )
                    {
                        ns = type.FullName.Replace( ".Controllers.", ".Views." );
                        if ( ns.EndsWith( "Controller" ) ) ns = ns.Substring( 0, ns.Length - "Controller".Length );
                    }

                    return ns;
                } );
        }

        struct TypeLookupResult
        {
            public Type Type { get; set; }
            public string EffectiveTheme { get; set; }
            public IEnumerable<string> Paths { get; set; }
        }
        
        static TypeLookupResult GetViewType( IController ctrl, string viewName, string defaultNamespace, string theme )
        {
            return _viewTypes.GetOrAdd( ctrl.GetType().FullName + "::" + viewName + "::" + theme,
                _ =>
                {
                    var fullyQualifiedParts = viewName.Split( new[] { ',' }, 2 );
                    var assembly = fullyQualifiedParts.Length > 1 ? Assembly.Load( fullyQualifiedParts[1] ) : null;
                    assembly = assembly ?? ctrl.GetType().Assembly;

                    var possibleTypeNames = 
                        ( fullyQualifiedParts.Length > 1 || string.IsNullOrEmpty( defaultNamespace ) ) ? 
                        fullyQualifiedParts.Take(1) :                           // If the type name is fully qualified, use it as is
                        new[] { viewName, defaultNamespace + '.' + viewName };  // Otherwise, try the default namespace

                    var themedAssembly = _themedAssemblies.GetOrAdd( Tuple.Create( theme, assembly ), __ =>
                        (from t in Maybe.Value( theme )
                         where t != ""
                         let themedName = assembly.GetName().Name + "." + t
                         select Assembly.Load( themedName )
                        )
                        .Catch()
                        .ValueOrNull() );

                    var res =
                        possibleTypeNames
                        .Select( name => 
                        {
                            var themedRes =  ( themedAssembly != null ? themedAssembly.GetType( name, false, true ) : null )
                                             ?? 
                                             ( string.IsNullOrEmpty( theme ) ? null : assembly.GetType( name + "_" + theme, false, true ) );
                            return themedRes != null 
                                ? new { type = themedRes, theme } 
                                : new { type = assembly.GetType( name, false, true ), theme = "" };
                        } )
                        .Where( t => t.type != null )
                        .FirstOrDefault();

                    if ( res == null || res.type == null )
                    {
                        return new TypeLookupResult { Paths = 
                            possibleTypeNames
                            .SelectMany( name => new[] {
                                themedAssembly != null ? name + ", " + themedAssembly.FullName : null,
                                string.IsNullOrEmpty( theme ) ? null : name + "_" + theme + ", " + assembly.FullName,
                                name + ", " + assembly.FullName
                            } )
                            .Where( p => p != null )
                            .ToList()
                        };
                    }
                    else
                    {
                        return new TypeLookupResult { Type = res.type, EffectiveTheme = res.theme };
                    }
                } );
        }

        private string GetTheme( ControllerContext controllerContext )
        {
            var service = DependencyResolver.Current.GetService<IThemeService>();
            return service == null ? null : service.CurrentTheme;
        }

        static readonly ConcurrentDictionary<string, string> _defaultNamespaces = new ConcurrentDictionary<string, string>();
        static readonly ConcurrentDictionary<string, TypeLookupResult> _viewTypes = new ConcurrentDictionary<string, TypeLookupResult>();
        static readonly ConcurrentDictionary<string, Type> _viewStartPages = new ConcurrentDictionary<string, Type>();
        static readonly ConcurrentDictionary<Tuple<string, Assembly>, Assembly> _themedAssemblies = new ConcurrentDictionary<Tuple<string, Assembly>, Assembly>();
    }
}