using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using System.Web;
using Lpp.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    class GridClientSortColumn : IGridClientSortColumn
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<IGridClientSortColumn> Factory { get { return UIWidget.Factory<IGridClientSortColumn>( h => new GridClientSortColumn( h ) ); } }

        public HtmlHelper Html { get; private set; }

        public GridClientSortColumn( HtmlHelper html )
        {
            //Contract.Requires( html != null );
            Html = html;
        }

        public void On<TRow>( IGridColumnOptions<TRow> col, Func<TRow,string> getJavaScriptSortValue, bool ascendingByDefault )
        {
            col.CellAttributes = 
                col.CellAttributes.Merge(
                new SortedList<string,Func<TRow,string>> 
                { { "sort-value", getJavaScriptSortValue } } );

            col.Title( _ => Html.Partial<Views.Grid.GridClientSortColumnTitle>().WithModel( new GridClientSortColumnModel 
            {
                InnerTitle = col.Title,
                AscendingByDefault = ascendingByDefault
            } ) );
        }

        public string ToHtmlString()
        {
            throw new NotSupportedException( "Cannot display a Grid client-side-sort column by itself. Apply it to a Grid by calling the .On() method." );
        }
    }
}