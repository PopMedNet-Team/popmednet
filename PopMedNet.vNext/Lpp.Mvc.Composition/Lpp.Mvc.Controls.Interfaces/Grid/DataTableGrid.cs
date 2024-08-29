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
using System.Data;

namespace Lpp.Mvc.Controls
{
    public interface IDataTableGrid : IHtmlString, IUIWidget
    {
        /// <summary>
        /// Creates a grid from an ADO.NET DataTable.
        /// </summary>
        /// <param name="grid">The grid implementation to build the columns on</param>
        /// <param name="table">The DataTable to use as data source</param>
        /// <param name="jsSortValueCalculators">Dictionary of converters, one for each column, that convert value of a DataTable cell into a Javascript literal that
        /// can be used for sorting the grid with Javascript. If null, considered empty. If lacks a converter for a particular column, a default is provided (see <see cref="GetDefaultJsSortValueCalculator"/>).</param>
        IGrid<DataRow> On( IGrid grid, DataTable table, IDictionary<DataColumn, Func<object, string>> jsSortValueCalculators );

        Func<object, string> GetDefaultJsSortValueCalculator<TColumnType>();
    }

    public static class DataTableGridExtensions
    {
        /// <summary>
        /// Creates a grid from an ADO.NET DataTable.
        /// </summary>
        /// <param name="grid">The grid implementation to build the columns on</param>
        /// <param name="table">The DataTable to use as data source</param>
        /// <param name="jsSortValueCalculators">Dictionary of converters, one for each column, that convert value of a DataTable cell into a Javascript literal that
        /// can be used for sorting the grid with Javascript. If null, considered empty. If lacks a converter for a particular column, a default is provided (see <see cref="GetDefaultJsSortValueCalculator"/>).</param>
        public static IGrid<DataRow> FromDataTable( this IGrid grid, DataTable table, IDictionary<DataColumn, Func<object, string>> jsSortValueCalculators = null )
        {
            //Contract.Requires( grid != null );
            //Contract.Requires( table != null );

            return grid.Html.Render<IDataTableGrid>().On( grid, table, jsSortValueCalculators );
        }
    }
}