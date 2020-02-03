using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Security.Principal;
using System.Web;
using System.Reactive.Linq;
using System.Threading;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;

namespace Lpp.Mvc
{
    public class AnonymousUIWidgetFactory<TWidget> : IUIWidgetFactory<TWidget>
        where TWidget : IUIWidget
    {
        private readonly Func<HtmlHelper, TWidget> _create;

        public AnonymousUIWidgetFactory( Func<HtmlHelper, TWidget> create )
        {
            //Contract.Requires( create != null );
            _create = create;
        }

        public TWidget CreateWidget( HtmlHelper html )
        {
            return _create( html );
        }
    }
}