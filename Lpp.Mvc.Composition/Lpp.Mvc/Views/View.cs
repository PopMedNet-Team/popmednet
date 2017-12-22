using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using System.Web;

namespace Lpp.Mvc
{
    public static class View
    {
        public static ViewResultBuilder<TView> Result<TView>()
            where TView : System.Web.Mvc.WebViewPage
        { return new ViewResultBuilder<TView>(); }

        public static ViewResult WithModel<TView, TModel>( this ViewResultBuilder<TView> vb, TModel model )
            where TView : System.Web.Mvc.WebViewPage<TModel>
        {
            return new ViewResult
            {
                ViewName = typeof( TView ).AssemblyQualifiedName,
                ViewData = new ViewDataDictionary<TModel>( model )
            };
        }

        public static ViewResult WithoutModel<TView>( this ViewResultBuilder<TView> vb )
            where TView : System.Web.Mvc.WebViewPage
        {
            return new ViewResult { ViewName = typeof( TView ).AssemblyQualifiedName };
        }

        public struct ViewResultBuilder<TView>
            where TView : System.Web.Mvc.WebViewPage
        {
            public ViewDataDictionary ViewData { get; set; }
            public TempDataDictionary TempData { get; set; }
        }
    }
}