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
    public static class UIWidget
    {
        public static IUIWidgetFactory<TWidget> Factory<TWidget>( Func<HtmlHelper,TWidget> create )
            where TWidget : IUIWidget
        {
            //Contract.Requires(create != null);
            return new AnonymousUIWidgetFactory<TWidget>( create );
        }
    }
}