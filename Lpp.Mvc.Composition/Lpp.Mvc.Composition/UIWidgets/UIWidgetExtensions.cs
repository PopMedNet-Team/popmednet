using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Lpp.Composition;
using System.Web;

namespace Lpp.Mvc
{
    public static class UIWidgetExtensions
    {
        public static TWidget Render<TWidget>( this HtmlHelper html ) where TWidget : IUIWidget
        {
            //Contract.Requires( html != null );
            var factory = html.ViewContext.HttpContext.Composition().Get<IUIWidgetFactory<TWidget>>();
            if ( factory == null ) throw new NotImplementedException( "Cannot find an implementation of widget " + typeof( TWidget ).FullName );

            return factory.CreateWidget( html );
        }
    }
}