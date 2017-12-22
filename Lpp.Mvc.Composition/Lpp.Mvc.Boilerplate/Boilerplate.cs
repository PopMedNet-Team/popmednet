using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using log4net;
using Lpp.Auth;
using Lpp.Composition.Modules;

namespace Lpp.Mvc
{
    public static class Boilerplate
    {
        public static IModule ErrorFilter<TErrorView>()
            where TErrorView : WebViewPage<Exception>
        {
            return new ModuleBuilder()
                .Export<IMvcFilter, ErrorHandler<TErrorView>>()
                .CreateModule();
        }

        public static IModule Log4NetLogger( string loggerName )
        {
            log4net.Config.XmlConfigurator.Configure();
            return new ModuleBuilder().Export( LogManager.GetLogger( loggerName ) ).CreateModule();
        }

        public static IHtmlString JsBootstrap( this HtmlHelper html )
        {
            //Contract.Requires( html != null );
            return html.Partial<Views.JsBootstrap>();
        }

        public static IModule JsBootstrap()
        {
            return new ModuleBuilder()
                .Export( RouteRegistrar.Create( r => r.MapResources( typeof( Boilerplate ).Assembly ) ) )
                .CreateModule();
        }
    }
}