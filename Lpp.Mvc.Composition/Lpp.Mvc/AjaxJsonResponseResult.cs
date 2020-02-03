using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Lpp.Utilities.Legacy;

namespace Lpp.Mvc
{
    public class AjaxJsonResponseResult : ActionResult
    {
        private IDictionary<string, object> _json;
        public AjaxJsonResponseResult( object json ) { _json = ObjectDictionary.From( json ?? new object() ); }

        public override void ExecuteResult( ControllerContext context )
        {
            var r = context.HttpContext.Response;
            r.Status = "500 AJAX JSON Response";
            r.Write( "{" + string.Join( ",", _json.Select( k => string.Format( "{0}: {1}", ToStr( k.Key ), ToStr( k.Value ) ) ) ) + "}" );
            r.TrySkipIisCustomErrors = true;
        }

        string ToStr( object o )
        {
            if ( o is string ) return "\"" + o.ToString().Replace( "\"", "\\\"" ) + "\"";
            if ( o == null ) return "null";
            return Convert.ToString( o );
        }
    }
}