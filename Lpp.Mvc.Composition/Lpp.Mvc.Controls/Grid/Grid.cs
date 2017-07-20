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
    class Grid : IGrid
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<IGrid> Factory { get { return UIWidget.Factory<IGrid>( h => new Grid( h ) ); } }

        public HtmlHelper Html { get; private set; }

        public Grid( HtmlHelper html )
        {
            //Contract.Requires( html != null );
            Html = html;
        }

        public IGrid<T> From<T>( IEnumerable<T> source )
        {
            return new Grid<T>( Html, source );
        }

        public string ToHtmlString()
        {
            throw new NotSupportedException( "Cannot display grid without source list. Please call the .From method first." );
        }
    }

    class Grid<T> : IGrid<T>
    {
        readonly IEnumerable<T> _source;
        readonly GridOptions _def;
        readonly IEnumerable<ColumnDef> _columns;
        readonly Func<T, object> _id;

        public HtmlHelper Html { get; private set; }

        public Grid( HtmlHelper html, IEnumerable<T> source )
            : this( html, source, GridOptions.Default, Enumerable.Empty<ColumnDef>(), null )
        {
        }

        Grid( HtmlHelper html, IEnumerable<T> source, GridOptions def, IEnumerable<ColumnDef> columns, Func<T, object> id )
        {
            //Contract.Requires( html != null );
            //Contract.Requires( source != null );
            //Contract.Requires( columns != null );
            _source = source;
            Html = html;
            _def = def;
            _columns = columns;
            _id = id;
        }

        public IGrid<T> With( Action<IGridOptions> setOptions )
        {
            var d = (IGridOptions) _def;
            setOptions( d );
            return new Grid<T>( Html, _source, (GridOptions) d, _columns, _id );
        }

        public IGrid<T> Id<Y>( Func<T, Y> accessor )
        {
            return new Grid<T>( Html, _source, _def, _columns, t => accessor( t ) );
        }

        public IGrid<T> Column( Func<T, IHtmlString> accessor, Action<IGridColumnOptions<T>> setOptions )
        {
            var column = new ColumnDef( t => accessor( t ) );
            if ( setOptions != null ) setOptions( column.Options );
            return new Grid<T>( Html, _source, _def, _columns.Concat( new[] { column } ), _id );
        }

        public string ToHtmlString()
        {
            return Html.Partial<Lpp.Mvc.Views.Grid.Grid>().WithModel( new GridDefinition
            {
                Options = _def,
                Columns = _columns.Select( c => c.Options ),
                Rows = _source.Select( r => new GridRowDefinition
                {
                    Id = _id == null ? null : Convert.ToString( _id( r ) ),
                    Cells = _columns.Select( c => new GridCellDefinition
                    {
                        Content = c.CellValue( r ),
                        Attributes = c.Options.CellAttributes.EmptyIfNull()
                                        .Where( k => k.Value != null && !string.IsNullOrEmpty( k.Key ) )
                                        .Select( a => new KeyValuePair<string,string>( a.Key, a.Value( r ) ) )
                    } )
                } )
            } )
            .ToHtmlString();
        }

        class ColumnDef
        {
            readonly GridColumnOptions<T> _options = new GridColumnOptions<T>();
            public GridColumnOptions<T> Options { get { return _options; } }

            readonly Func<T, IHtmlString> _accessor;
            public Func<T, IHtmlString> Accessor { get { return _accessor; } }

            public ColumnDef( Func<T,IHtmlString> accessor )
            {
                //Contract.Requires( accessor != null );
                _accessor = accessor;
            }

            public IHtmlString CellValue( T row )
            {
                var v = Accessor( row );
                if ( string.IsNullOrEmpty( Options.Format ) ) return v;
                else return new MvcHtmlString( string.Format( "{0:" + Options.Format + "}", v == null ? null : v.ToHtmlString() ) );
            }
        }
    }

    public struct GridOptions : IGridOptions
    {
        public static GridOptions Default = new GridOptions
        {
            AlternatingRowCssClasses = new[] { "", "Alt" },
            MaxPageButtonsBeforeAbbreviating = 8,
            PageButtonsWhenAbbreviated = 5,
            NoDataMessage = _ => new MvcHtmlString( "No data to show" )
        };

        public IDictionary<string, string> Attributes { get; set; }
        public IEnumerable<string> AlternatingRowCssClasses { get; set; }
        public IHtmlString FooterPrefix { get; set; }
        public IHtmlString FooterSuffix { get; set; }
        public IHtmlString Appendix { get; set; }

        public string ReloadUrl { get; set; }

        public string CurrentSort { get; set; }
        public bool CurrentSortIsAscending { get; set; }

        public bool IsPaged { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public int MaxPageButtonsBeforeAbbreviating { get; set; }
        public int PageButtonsWhenAbbreviated { get; set; }

        public Func<object, IHtmlString> NoDataMessage { get; set; }
    }

    public class GridColumnOptions
    {
        public string Format { get; set; }
        public IHtmlString Title { get; set; }
        public bool Sortable { get; set; }
        public string SortProperty { get; set; }
        public IDictionary<string, string> HeaderAttributes { get; set; }
    }

    public class GridColumnOptions<T> : GridColumnOptions, IGridColumnOptions<T>
    {
        public IDictionary<string, Func<T,string>> CellAttributes { get; set; }
    }

    public class GridDefinition
    {
        public GridOptions Options { get; set; }
        public IEnumerable<GridColumnOptions> Columns { get; set; }
        public IEnumerable<GridRowDefinition> Rows { get; set; }
    }

    public class GridRowDefinition
    {
        public string Id { get; set; }
        public IEnumerable<GridCellDefinition> Cells { get; set; }
    }

    public class GridCellDefinition
    {
        public IHtmlString Content { get; set; }
        public IEnumerable<KeyValuePair<string,string>> Attributes { get; set; }
    }
}