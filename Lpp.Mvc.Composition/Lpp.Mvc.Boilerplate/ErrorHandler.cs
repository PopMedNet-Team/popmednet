using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using log4net;
using Lpp.Mvc;

namespace Lpp.Mvc
{
    [Export( typeof( IMvcFilter ) )]
    public class ErrorHandler<TErrorView> : IExceptionFilter, IMvcFilter
        where TErrorView : WebViewPage<Exception>
    {
        [Import] public ILog Log { get; set; }

        public bool AllowMultiple { get { return false; } }
        public int Order { get { return int.MaxValue; } }

        public void OnException( ExceptionContext filterContext )
        {
            if ( filterContext.ExceptionHandled ) return;

            var ctx = filterContext.HttpContext;
            Log.Error( string.Format( "URL: {0}{1}Method: {2}{1}User: {3}{1}Message: {4}{1}",
                ctx.Request.Url, Environment.NewLine, ctx.Request.HttpMethod, ctx.User, filterContext.Exception.Message ),
                filterContext.Exception );
            filterContext.Result = View.Result<TErrorView>().WithModel( filterContext.Exception );
            filterContext.ExceptionHandled = true;
        }
    }
}