using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Registration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using Lpp.Composition;
using Lpp.Composition.Modules;
using System.Reflection;
using System.Text;
using Lpp;
using Lpp.Mvc.Composition.Application;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public class LppMvcComposableApplication : System.Web.HttpApplication
    {
        private static CompositionScopingService _composition = null;
        protected CompositionScopingService CompositionScoping { get { return _composition; } }

        public LppMvcComposableApplication()
        {
            if ( _composition == null )
            {
                _composition = MefConfig.RegisterMef();
            }

            _composition.RootScope.SatisfyImportsOnce( this );

            this.BeginRequest += OnBeginRequest;
            this.EndRequest += OnEndRequest;
#if(!DEBUG)
            this.Error += OnError;
#endif
        }

        [ImportMany] public IEnumerable<IHttpModule> HttpModules { get; set; }
        [ImportMany] public IEnumerable<IMvcFilter> MvcFilters { get; set; }
        [ImportMany] public IEnumerable<IModelBinderProvider> BinderProviders { get; set; }
        [ImportMany] public IEnumerable<Lazy<IModelBinder, IModelBinderMetadata>> Binders { get; set; }
        [ImportMany] public IEnumerable<IRouteRegistrar> RouteRegistrars { get; set; }

        protected virtual void BaseApplication_Start()
        {
            DependencyResolver.SetResolver(
                type =>
                {
                    try { return (HttpContext.Current.Composition() ?? _composition.RootScope).Get( type ); }
                    catch ( CompositionException ) { return null; }
                },
                type =>
                {
                    try { return (HttpContext.Current.Composition() ?? _composition.RootScope).GetMany( type ); }
                    catch ( CompositionException ) { return null; }
                } );

            ControllerBuilder.Current.SetControllerFactory( new FullTypeNameControllerFactory() );
            VirtualPathFactoryManager.RegisterVirtualPathFactory( new FullyQualifiedTypeVirtualPathFactory() );

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add( new CompiledViewEngine() );

            RegisterModelBinders( ModelBinderProviders.BinderProviders, System.Web.Mvc.ModelBinders.Binders );
            RegisterGlobalFilters( GlobalFilters.Filters );
            RegisterRoutes( RouteTable.Routes );
        }

        #region Composition scoping

        void OnBeginRequest( object s, EventArgs e )
        {
            var requestComposition = _composition.OpenScope( TransactionScope.Id );
            requestComposition.ComposeExportedValue<HttpContextBase>( new HttpContextWrapper( this.Context ) );
            new HttpContextWrapper( this.Context ).SetComposition( requestComposition );
        }

        void OnEndRequest( object s, EventArgs e )
        {
            new HttpContextWrapper( this.Context ).DisposeComposition();
        }

        protected void OnError(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
           

            if (exception is HttpRequestValidationException)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.Write(@"<html><head></head><body>We're sorry but HTML and Javascript may not be submitted in this site. Please use your browser's back button and remove any html and or javascript for text fields.</body></html>");
                Response.End();
            }
            else 
            {
                //This should report these errors directly to our own service server.
                Server.ClearError();
                Response.Write(exception.UnwindException());
                Response.End();               
            }
        }

        #endregion

        public override void Init()
        {
            base.Init();
            foreach ( var m in HttpModules ) m.Init( this );
        }

        public virtual void RegisterModelBinders( ModelBinderProviderCollection providers, ModelBinderDictionary binders )
        {
            foreach ( var p in BinderProviders ) providers.Add( p );
            foreach ( var b in Binders ) binders.Add( b.Metadata.TargetType, b.Value );
        }

        public virtual void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add( new HandleErrorAttribute() );
            foreach ( var f in MvcFilters ) filters.Add( f );
        }

        public virtual void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            foreach ( var reg in RouteRegistrars ) reg.RegisterRoutes( routes );
            using ( var reqScope = _composition.OpenScope( TransactionScope.Id ) )
            {
                reqScope.ComposeExportedValue<HttpContextBase>( new HttpContextWrapper( this.Context ) );
                new HttpContextWrapper( this.Context ).SetComposition( reqScope );
                routes.MapAutomaticRoutes( reqScope );
            }

            foreach ( var reg in RouteRegistrars ) reg.RegisterCatchAllRoutes( routes );
        }
    }
}