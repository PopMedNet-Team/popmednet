using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Dns.Data;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Dns.Portal.Models;
using System.Linq.Expressions;
using Lpp.Dns.Portal.Controllers;
using Lpp.Mvc.Controls;
using Lpp.Security;

namespace Lpp.Dns.Portal
{
    public static class PageSizeSelectorExtensions
    {
        public static IHtmlString PageSizeSelector<TEntity, TListGetModel>( this HtmlHelper html, string entityName, IListModel<TEntity, TListGetModel> list, Func<TListGetModel, string> reloadUrl, IEnumerable<int> options = null )
            where TListGetModel : struct, IListGetModel
            where TEntity : class
        {
            return html.PageSizeSelector( list, "Showing " + list.PageSize + " " + entityName + " per page", reloadUrl, lines => "Show " + lines + " " + entityName + " per page", options );
        }

        public static IHtmlString PageSizeSelector<TEntity, TListGetModel>( this HtmlHelper html, IListModel<TEntity, TListGetModel> list, string currentText, Func<TListGetModel, string> reloadUrl, Func<int, string> text, IEnumerable<int> options = null )
            where TListGetModel : struct, IListGetModel
            where TEntity : class
        {
            var m = list.OriginalRequest;
            return html.PageSizeSelector( list.PageSize, currentText, lines => { m.PageSize = lines.ToString(); return reloadUrl( m ); }, text );
        }

        public static IHtmlString PageSizeSelector( this HtmlHelper html, int currentSize, string currentText, Func<int, string> reloadUrl, Func<int, string> text, IEnumerable<int> options = null )
        {
            return html.Partial<Views.Crud.PageSizeSelector>().WithModel( new PageSizeSelectorModel
            {
                CurrentSize = currentSize,
                CurrentText = currentText,
                ReloadUrl = reloadUrl,
                Text = text,
                Options = options ?? new[] { 10, 20, 50, 100 }
            } );
        }
    }
}