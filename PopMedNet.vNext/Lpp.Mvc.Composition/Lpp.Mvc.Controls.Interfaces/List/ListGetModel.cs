using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.Web.Routing;

namespace Lpp.Mvc.Controls
{
    public interface IListGetModel
    {
        string Page { get; set; }
        string Sort { get; set; }
        string SortDirection { get; set; }
        string PageSize { get; set; }
    }

    public struct ListGetModel : IListGetModel
    {
        public string Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string PageSize { get; set; }
    }

    public static class ListGetModelExtensions
    {
        public static int GetPage<TModel>( this TModel m ) where TModel : IListGetModel
        {
            //Contract.Ensures( //Contract.Result<int>() >= 0 );
            int p;
            return (!string.IsNullOrEmpty( m.Page ) && int.TryParse( m.Page, out p ) ) ? Math.Max( p, 0 ) : 0;
        }

        public static int? GetPageSize<TModel>( this TModel m ) where TModel : IListGetModel
        {
            //Contract.Ensures( //Contract.Result<int?>() == null || //Contract.Result<int?>().Value > 0 );
            int s;
            return (!string.IsNullOrEmpty( m.PageSize ) && int.TryParse( m.PageSize, out s )) ? Math.Max( s, 1 ) : (int?)null;
        }

        public static bool? SortAscending<TModel>( this TModel m ) where TModel : IListGetModel
        {
            return
                string.IsNullOrEmpty( m.SortDirection ) ? null :
                string.Equals( m.SortDirection, "desc", StringComparison.OrdinalIgnoreCase ) ? false :
                string.Equals( m.SortDirection, "asc", StringComparison.OrdinalIgnoreCase ) ? true :
                (bool?)null;
        }

        public static ListGetModel Downgrade<T>( this T m ) where T : struct, IListGetModel
        {
            return new ListGetModel
            {
                Page = m.Page,
                PageSize = m.PageSize,
                Sort = m.Sort,
                SortDirection = m.SortDirection
            };
        }
    }
}