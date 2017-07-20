using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using System.Web.Mvc;
using System.Diagnostics.Contracts;

namespace Lpp.Mvc.Controls
{
    public static class UtilityExtensions
    {
        public static void SetDefault<T, U>( this IDictionary<T, U> dic, T key, U defaultValue )
        {
            //Contract.Requires( dic != null );
            if ( !dic.ContainsKey( key ) ) dic[key] = defaultValue;
        }
    }
}