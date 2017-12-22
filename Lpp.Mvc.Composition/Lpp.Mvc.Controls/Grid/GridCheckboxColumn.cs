using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Composition;

namespace Lpp.Mvc.Controls
{
    class GridCheckboxColumn : IGridCheckboxColumn
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<IGridCheckboxColumn> Factory { get { return UIWidget.Factory<IGridCheckboxColumn>( h => new GridCheckboxColumn( h ) ); } }

        public HtmlHelper Html { get; private set; }

        public GridCheckboxColumn( HtmlHelper html )
        {
            //Contract.Requires( html != null );
            Html = html;
        }

        public IGrid<T> On<T>( IGrid<T> grid, string hiddenFieldName, string jsFnGetAllPossibleIdsCommaSeparated )
        {
            return grid.Column(
                item => new MvcHtmlString( "<input type=\"checkbox\" class=\"CheckboxColumn\" />" ),
                c => c
                    .Title( _ => Html.Partial<Views.Grid.GridCheckboxColumnTitle>().WithModel( new GridCheckboxColumnModel 
                        { 
                            HiddenFieldName = hiddenFieldName,
                            JsFnGetAllPossibleIdsCommaSeparated = jsFnGetAllPossibleIdsCommaSeparated
                        } ) )
                    .NotSortable()
            );
        }

        public string ToHtmlString()
        {
            throw new NotSupportedException( "Cannot display a Grid Checkbox Column by itself. Apply it to a Grid by calling the .On() method." );
        }
    }
}