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
using System.Data;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    class DataTableGrid : IDataTableGrid
    {
        [Export]
        public static IUIWidgetFactory<IDataTableGrid> Factory { get { return UIWidget.Factory<IDataTableGrid>( h => new DataTableGrid( h ) ); } }

        public HtmlHelper Html { get; private set; }

        public DataTableGrid( HtmlHelper html )
        {
            //Contract.Requires( html != null );
            Html = html;
        }

        public IGrid<DataRow> On( IGrid grid, DataTable table, IDictionary<DataColumn, Func<object, string>> jsSortValueCalculators )
        {
            return table.Columns.Cast<DataColumn>().Aggregate(
                grid.From( table.Rows.Cast<DataRow>() ),
                ( g, col ) =>
                {
                    var sortValueCalculator = 
                        ( jsSortValueCalculators == null ? null : jsSortValueCalculators.ValueOrDefault( col ) )
                        ?? GetDefaultJsSortValueCalculator( col.DataType );
                    return g.ClientSideSortColumn(
                            item => new MvcHtmlString( Convert.ToString( item[col] ) ),
                            row => sortValueCalculator( row[col] ),
                            o => o.Title( col.ColumnName ) );
                } );
        }

        public Func<object, string> GetDefaultJsSortValueCalculator<TColumnType>()
        {
            return GetDefaultJsSortValueCalculator( typeof( TColumnType ) );
        }

        static Func<object, string> GetDefaultJsSortValueCalculator( Type type )
        {
            var res = GetJsSortValueCalculatorWithNoNullHandling( type );
            return o => o is DBNull ? "" : res( o );
        }

        static Func<object, string> GetJsSortValueCalculatorWithNoNullHandling( Type type )
        {
            if ( type == typeof( string ) ) return o => "'" + o + "'";
            if ( type == typeof( double ) || type == typeof( float ) ) return o => Convert.ToDouble( o ).ToString( "0.0000" );
            return o => o.ToString();
        }

        public string ToHtmlString()
        {
            throw new NotSupportedException( "Cannot display DataTableGrid without source DataTable. Please call the .On method and supply the IGrid argument." );
        }
    }
}