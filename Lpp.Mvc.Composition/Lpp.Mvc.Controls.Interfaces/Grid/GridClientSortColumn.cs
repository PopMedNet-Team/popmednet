using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Web.Routing;
using Lpp.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc.Controls
{
    public interface IGridClientSortColumn : IUIWidget
    {
        /// <summary>
        /// Modifies a column in such a way that it becomes sortable on the client-side.
        /// NOTE: if paging is involved, then this sorting will only occur across current page, since other pages do not
        /// get loaded into the browser at all.
        /// </summary>
        /// <param name="col">Column to modify</param>
        /// <param name="getJavaScriptSortValue">A function that for each grid row returns a Javascript expression that evaluates to a Javascript value that will be used as the row's value when sorting</param>
        void On<TRow>( IGridColumnOptions<TRow> col, Func<TRow, string> getJavaScriptSortValue, bool ascendingByDefault = true );
    }

    public static class GridClientSortColumnExtensions
    {
        /// <summary>
        /// Modifies a column in such a way that it becomes sortable on the client-side.
        /// NOTE: if paging is involved, then this sorting will only occur across current page, since other pages do not
        /// get loaded into the browser at all.
        /// </summary>
        /// <param name="col">Column to modify</param>
        /// <param name="html">HTML helper</param>
        /// <param name="getJavaScriptSortValue">A function that for each grid row returns a Javascript expression that evaluates to a Javascript value that will be used as the row's value when sorting</param>
        public static IGridColumnOptions<TRow> ClientSideSort<TRow>( this IGridColumnOptions<TRow> col, HtmlHelper html, Func<TRow, string> getJavaScriptSortValue, bool ascendingByDefault = true )
        {
            //Contract.Requires( col != null );
            //Contract.Requires( getJavaScriptSortValue != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<TRow>>() != null );

            var widget = html.Render<IGridClientSortColumn>();
            widget.On( col, getJavaScriptSortValue, ascendingByDefault );
            return col;
        }

        public static IGrid<TRow> ClientSideSortColumn<TRow>( this IGrid<TRow> grid, Func<TRow,IHtmlString> getCellContent, Func<TRow, string> getJavaScriptSortValue, Action<IGridColumnOptions<TRow>> setOptions, bool ascendingByDefault = true )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( getJavaScriptSortValue != null );
            //Contract.Ensures( //Contract.Result<IGrid<TRow>>() != null );

            return grid.Column( getCellContent, o =>
            {
                if ( setOptions != null ) setOptions( o );
                o.ClientSideSort( grid.Html, getJavaScriptSortValue, ascendingByDefault );
            } );
        }

        public static IGrid<TRow> ClientSideSortColumn<TRow, TValue>( this IGrid<TRow> grid, Func<TRow, TValue> accessor, Func<TValue, IHtmlString> format = null, Action<IGridColumnOptions<TRow>> setOptions = null )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( accessor != null );
            //Contract.Ensures( //Contract.Result<IGrid<TRow>>() != null );

            if ( format == null ) format = i => new MvcHtmlString( Convert.ToString( i ) );
            return grid.Column( i => format( accessor( i ) ), c =>
            {
                var sortCalc = GetSortValueCalculator<TValue>();
                if ( setOptions != null ) setOptions( c );
                c.ClientSideSort( grid.Html, r => sortCalc( accessor( r ) ) );
            } );
        }

        static Func<T, string> GetSortValueCalculator<T>()
        {
            var res = _sortValueCalculators
                .Select( c => c( typeof( T ) ) )
                .Where( r => r != null )
                .FirstOrDefault();
            if ( res != null ) return x => x == null ? "" : res( x );

            throw new NotSupportedException( "Cannot automatically infer sorting value for type " + typeof( T ).Name );
        }

        static Func<Type, Func<object, string>>[] _sortValueCalculators = new[]
            {
                SortValueCalculator( (string s) => "'" + s + "'" ),
                SortValueCalculatorStruct( (int x) => x.ToString() ),
                SortValueCalculatorStruct( (byte x) => x.ToString() ),
                SortValueCalculatorStruct( (float x) => x.ToString( "0.0000" ) ),
                SortValueCalculatorStruct( (double x) => x.ToString( "0.0000" ) ),
                SortValueCalculatorStruct( (DateTime x) => x.Ticks.ToString() )
            };

        static Func<Type, Func<object, string>> SortValueCalculatorStruct<T>( Func<T, string> how ) where T : struct
        {
            var nonNull = SortValueCalculator<T>( how );
            return type =>
            {
                var r = nonNull( type );
                if ( r != null ) return r;

                if ( type == typeof( T? ) ) return x =>
                    {
                        var n = (T?)x;
                        return n == null ? "" : how( n.Value );
                    };

                return null;
            };
        }

        static Func<Type,Func<object, string>> SortValueCalculator<T>( Func<T, string> how )
        {
            return type =>
                {
                    if ( type == typeof( T ) ) return x => how( (T)x );
                    return null;
                };
        }

        public static IGrid<TRow> ClientSideSortColumn<TRow, TValue>( this IGrid<TRow> grid, Func<TRow, TValue> accessor, string title )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( accessor != null );
            //Contract.Ensures( //Contract.Result<IGrid<TRow>>() != null );

            return grid.ClientSideSortColumn( accessor, null, c => c.Title( title ) );
        }

        public static IGrid<TRow> ClientSideSortColumn<TRow, TValue>( this IGrid<TRow> grid, Expression<Func<TRow, TValue>> accessor )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( accessor != null );
            //Contract.Ensures( //Contract.Result<IGrid<TRow>>() != null );

            return grid.ClientSideSortColumn( accessor.Compile(), accessor.MemberName() );
        }
    }
}