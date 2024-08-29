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

namespace Lpp.Mvc.Controls
{
    public interface IGridCheckboxColumn : IUIWidget
    {
        IGrid<T> On<T>( IGrid<T> grid, string hiddenFieldName, string jsFnGetAllPossibleIdsCommaSeparated );
    }

    public static class GridCheckboxColumnExtensions
    {
        public static IGrid<T> CheckboxColumn<T>( this IGrid<T> grid, string hiddenFieldName, string jsFnGetAllPossibleIdsCommaSeparated = null )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( !String.IsNullOrEmpty( hiddenFieldName ) );
            //Contract.Ensures( //Contract.Result<IGrid<T>>() != null );

            return grid.Html.Render<IGridCheckboxColumn>().On( grid, hiddenFieldName, jsFnGetAllPossibleIdsCommaSeparated );
        }
    }
}