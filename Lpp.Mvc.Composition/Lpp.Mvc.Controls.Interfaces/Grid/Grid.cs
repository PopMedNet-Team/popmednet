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
    public interface IGrid : IHtmlString, IUIWidget
    {
        IGrid<T> From<T>( IEnumerable<T> source );
    }

    public interface IGrid<T> : IHtmlString, IUIWidget
    {
        IGrid<T> With( Action<IGridOptions> setOptions );
        IGrid<T> Id<Y>( Func<T, Y> accessor );
        IGrid<T> Column( Func<T, IHtmlString> accessor, Action<IGridColumnOptions<T>> setOptions );
    }

    public interface IGridOptions
    {
        IDictionary<string, string> Attributes { get; set; }
        IEnumerable<string> AlternatingRowCssClasses { get; set; }
        IHtmlString FooterPrefix { get; set; }
        IHtmlString FooterSuffix { get; set; }
        IHtmlString Appendix { get; set; }

        string ReloadUrl { get; set; }
        string CurrentSort { get; set; }
        bool CurrentSortIsAscending { get; set; }

        bool IsPaged { get; set; }
        int CurrentPage { get; set; }
        int TotalPages { get; set; }
        int PageSize { get; set; }

        int MaxPageButtonsBeforeAbbreviating { get; set; }
        int PageButtonsWhenAbbreviated { get; set; }

        Func<object, IHtmlString> NoDataMessage { get; set; }
    }

    public interface IGridColumnOptions<TRow>
    {
        string Format { get; set; }
        IHtmlString Title { get; set; }
        bool Sortable { get; set; }
        string SortProperty { get; set; }
        IDictionary<string, string> HeaderAttributes { get; set; }
        IDictionary<string, Func<TRow,string>> CellAttributes { get; set; }
    }

    public static class GridExtensions
    {
        public static IGrid<T> Column<T, Y>( this IGrid<T> grid, Func<T, Y> accessor, Action<IGridColumnOptions<T>> setOptions )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( accessor != null );
            return grid.Column( t => new MvcHtmlString( Convert.ToString( accessor( t ) ) ), setOptions );
        }

        public static IGrid<T> Column<T, Y>( this IGrid<T> grid, Expression<Func<T, Y>> accessor, string title = null )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( accessor != null );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );

            var name = accessor.MemberName();
            return grid.Column( accessor.Compile(), o =>
            {
                o.Title = new MvcHtmlString( title ?? name ); 
                o.SortProperty = name; 
                o.Sortable = !string.IsNullOrEmpty( name );
            } );
        }

        public static IGrid<T> AutoColumns<T>( this IGrid<T> grid )
        {
            //Contract.Requires( grid != null );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );

            foreach ( var p in TypeDescriptor.GetProperties( typeof( T ) ).Cast<PropertyDescriptor>() )
            {
                grid = grid.Column( t => p.GetValue( t ), p.DisplayName );
            }
            return grid;
        }

        public static IGrid<T> Paging<T>( this IGrid<T> grid, int currentPage, int totalPages, int pageSize )
        {
            //Contract.Requires( grid != null );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );
            return grid.With( o =>
            {
                o.IsPaged = true;
                o.CurrentPage = currentPage;
                o.TotalPages = totalPages;
                o.PageSize = pageSize;
            } );
        }

        public static IGrid<T> SortedBy<T>( this IGrid<T> grid, string sortby, bool isAscending = true )
        {
            //Contract.Requires( grid != null );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );
            return grid.With( o => { o.CurrentSort = sortby; o.CurrentSortIsAscending = isAscending; } );
        }

        public static IGrid<T> Attributes<T>( this IGrid<T> grid, object attributes )
        {
            //Contract.Requires( grid != null );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );
            return grid.With( o => o.Attributes = o.Attributes.Merge( AttrsFrom( attributes ) ) );
        }

        public static IGrid<T> AltRowClasses<T>( this IGrid<T> grid, params string[] classes )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( classes != null );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );
            return grid.With( o => o.AlternatingRowCssClasses = classes );
        }

        public static IGrid<T> AltRowClass<T>( this IGrid<T> grid, string cls )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( !String.IsNullOrEmpty( cls ) );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );
            return grid.AltRowClasses( "", cls );
        }

        public static IGrid<T> ReloadUrl<T>( this IGrid<T> grid, string url )
        {
            //Contract.Requires( grid != null );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );
            return grid.With( o => o.ReloadUrl = url );
        }

        public static IGrid<T> FooterSuffix<T>(this IGrid<T> grid, Func<object,IHtmlString> footer)
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( footer != null );
            return grid.With( o => o.FooterSuffix = o.FooterSuffix == null ? footer( null ) : 
                new MvcHtmlString( o.FooterSuffix.ToHtmlString() + footer( null ).ToHtmlString() ) );
        }

        public static IGrid<T> FooterPrefix<T>( this IGrid<T> grid, Func<object, IHtmlString> footer )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( footer != null );
            return grid.With( o => o.FooterPrefix = o.FooterPrefix == null ? footer( null ) : 
                new MvcHtmlString( o.FooterPrefix.ToHtmlString() + footer( null ).ToHtmlString() ) );
        }

        public static IGrid<T> Appendix<T>( this IGrid<T> grid, Func<object, IHtmlString> appendix )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( appendix != null );
            return grid.With( o => o.Appendix = o.Appendix == null ? appendix( null ) : 
                new MvcHtmlString( o.Appendix.ToHtmlString() + appendix( null ).ToHtmlString() ) );
        }

        public static IGridColumnOptions<T> Title<T>( this IGridColumnOptions<T> opts, Func<object, IHtmlString> titleTemplate )
        {
            //Contract.Requires( opts != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.Title = titleTemplate( null );
            return opts;
        }

        public static IGridColumnOptions<T> Title<T>( this IGridColumnOptions<T> opts, string title )
        {
            //Contract.Requires( opts != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.Title = new MvcHtmlString( title );
            return opts;
        }

        public static IGridColumnOptions<T> Sortable<T>( this IGridColumnOptions<T> opts, string sortProperty )
        {
            //Contract.Requires( opts != null );
            //Contract.Requires( !String.IsNullOrEmpty( sortProperty ) );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.Sortable = true;
            opts.SortProperty = sortProperty;
            return opts;
        }

        public static IGridColumnOptions<T> NotSortable<T>( this IGridColumnOptions<T> opts )
        {
            //Contract.Requires( opts != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.Sortable = false;
            return opts;
        }

        public static IGridColumnOptions<T> Format<T>( this IGridColumnOptions<T> opts, string formatString )
        {
            //Contract.Requires( opts != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.Format = formatString;
            return opts;
        }

        public static IGridColumnOptions<T> HeaderAttributes<T>( this IGridColumnOptions<T> opts, object attributes )
        {
            //Contract.Requires( opts != null );
            //Contract.Requires( attributes != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.HeaderAttributes = opts.HeaderAttributes.Merge( AttrsFrom( attributes ) );
            return opts;
        }

        public static IGridColumnOptions<T> CellAttribute<T>( this IGridColumnOptions<T> opts, string attributeName, Func<T,string> getValue )
        {
            //Contract.Requires( opts != null );
            //Contract.Requires( !String.IsNullOrEmpty( attributeName ) );
            //Contract.Requires( getValue != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.CellAttributes = opts.CellAttributes.Merge(
                EnumerableEx.Return( 0 ).ToDictionary( _ => attributeName, _ => getValue ) );
            return opts;
        }

        public static IGridColumnOptions<T> CellAttributes<T>( this IGridColumnOptions<T> opts, object attributes )
        {
            //Contract.Requires( opts != null );
            //Contract.Requires( attributes != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            opts.CellAttributes = opts.CellAttributes.Merge( 
                AttrsFrom( attributes )
                .ToDictionary( kvp => kvp.Key, kvp => (Func<T,string>)( _ => kvp.Value ) ) );
            return opts;
        }

        public static IGridColumnOptions<T> Class<T>( this IGridColumnOptions<T> opts, string className )
        {
            //Contract.Requires( opts != null );
            //Contract.Ensures( //Contract.Result<IGridColumnOptions<T>>() != null );
            var a = new { @class = className };
            return opts.HeaderAttributes( a ).CellAttributes( a );
        }

        static IDictionary<string,string> AttrsFrom( object attributes )
        {
            return ObjectDictionary.From( attributes ).ToDictionary( k => k.Key, k => Convert.ToString( k.Value ) );
        }
    }
}