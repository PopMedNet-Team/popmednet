using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using System.Linq.Expressions;
using Lpp.Mvc.Controls;
using Lpp.Mvc.Views;

namespace Lpp.Mvc.Controls
{
    class Menu : IMenu
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<IMenu> Factory { get { return UIWidget.Factory<IMenu>( h => new Menu( h ) ); } }

        public IHtmlString FromItems( IEnumerable<MenuItemModel> items )
        {
            return Html.Partial<Lpp.Mvc.Views.Menu>().WithModel( items );
        }

        public HtmlHelper Html { get; private set; }
        public Menu( HtmlHelper html ) { Html = html; }
    }
}