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

namespace Lpp.Mvc
{
    class CompiledView : IView
    {
        private readonly Type _type;
        private readonly string _virtualPath;
        private readonly Type _startPageType;

        public CompiledView( string virtualPath, Type type, Type startPageType )
        {
            //Contract.Requires( type != null );
            _type = type;
            _virtualPath = virtualPath;
            _startPageType = startPageType;
        }

        public void Render( ViewContext viewContext, System.IO.TextWriter writer )
        {
            var instance = Activator.CreateInstance( _type ) as WebViewPage;
            if ( instance == null ) throw new InvalidOperationException( "View page of invalid type" );

            instance.VirtualPath = _virtualPath;
            instance.ViewContext = viewContext;
            instance.ViewData = viewContext.ViewData;
            instance.InitHelpers();

            StartPage start = null;
            if ( _startPageType != null )
            {
                start = Activator.CreateInstance( _startPageType ) as StartPage;
                if ( start != null )
                {
                    start.VirtualPath = _virtualPath;
                    start.ChildPage = instance;
                }
            }

            instance.ExecutePageHierarchy( new WebPageContext( viewContext.HttpContext, null, null ), writer, start );
        }
    }
}