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
    public static class HtmlExtensions
    {
        public static ViewBuilder<TView> Partial<TView>( this HtmlHelper html )
            where TView : System.Web.Mvc.WebViewPage
        {
            return new ViewBuilder<TView> { Html = html };
        }

        public static IHtmlString GetView<TModel>(this HtmlHelper html, string viewName, TModel model){
            return html.Partial(viewName, model);
        }

        public static IHtmlString WithModel<TView, TModel>( this ViewBuilder<TView> vb, TModel model )
            where TView : System.Web.Mvc.WebViewPage<TModel>
        {
            if ( model == null ) 
                throw new ArgumentNullException( "model", "Model cannot be null. Passing a null model will have the effect of passing the parent view's model, which may lead to typing conflict." );
           
            return vb.Html.Partial( typeof( TView ).AssemblyQualifiedName, model, null );
        }

        public static IHtmlString WithoutModel<TView>( this ViewBuilder<TView> vb )
            where TView : System.Web.Mvc.WebViewPage
        {
            return vb.Html.Partial( typeof( TView ).AssemblyQualifiedName, null, null );
        }

        public struct ViewBuilder<TView> : IHtmlString
            where TView : System.Web.Mvc.WebViewPage
        {
            public HtmlHelper Html { get; set; }

            public string ToHtmlString() 
            { 
                return this.WithoutModel().ToHtmlString(); 
            }
        }
    }
}