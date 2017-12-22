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
using System.Globalization;

namespace Lpp.Mvc.ModelBinders
{
    public class IfModifiedSinceBinder : IModelBinder
    {
        public object BindModel( ControllerContext controllerContext, ModelBindingContext bindingContext )
        {
            if ( bindingContext.ModelType != typeof( DateTime ) && bindingContext.ModelType != typeof( DateTime? ) )
            {
                return null;
            }

            var res = ParseHeader( controllerContext.RequestContext.HttpContext.Request );

            if ( bindingContext.ModelType == typeof( DateTime ) ) return res == null ? DateTime.MinValue : res.Value;
            return res;
        }

        public static DateTime? ParseHeader( HttpRequestBase request )
        {
            return ParseHeader( request.Headers["If-Modified-Since"] );
        }

        public static DateTime? ParseHeader( HttpRequest request )
        {
            return ParseHeader( request.Headers["If-Modified-Since"] );
        }

        public static DateTime? ParseHeader( string header )
        {
            if ( string.IsNullOrEmpty( header ) ) return null;

            DateTime res;
            return
                DateTime.TryParseExact( header, "ddd, dd MMM yyyy HH:mm:ss GMT", null, DateTimeStyles.None, out res ) ?
                (DateTime?) res : null;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class IfModifiedSinceHeaderAttribute : CustomModelBinderAttribute
    {
        static readonly IfModifiedSinceBinder _instance = new IfModifiedSinceBinder();

        public override IModelBinder GetBinder()
        {
            return _instance;
        }
    }
}