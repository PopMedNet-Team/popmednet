using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.WebPages.Razor;
using System.Web.Hosting;
using System.Collections.Concurrent;
using System.Web.WebPages;

namespace Lpp.Mvc
{
    public static class ReferenceAggregationShortcuts
    {
        public static void Stylesheet( this WebViewPage page, params string[] resourceNames )
        {
            //Contract.Requires( page != null );
            page.Html.Stylesheet( resourceNames.Select( rn => page.Resource( rn ) ).ToArray() );
        }

        public static void Stylesheet(this WebViewPage page, CssMediaType media, params string[] resourceNames)
        {
            page.Html.Stylesheet( media, resourceNames.Select(rn => page.Resource(rn)).ToArray());
        }

        public static void ScriptReference( this WebViewPage page, params string[] resourceNames )
        {
            //Contract.Requires( page != null );
            page.Html.ScriptReference( resourceNames.Select( rn => page.Resource( rn ) ).ToArray() );
        }

        public static void Stylesheet( this ViewStartPage page, params string[] resourceNames )
        {
            //Contract.Requires( page != null );
            page.Html.Stylesheet( resourceNames.Select( rn => page.Resource( rn ) ).ToArray() );
        }

        public static void ScriptReference( this ViewStartPage page, params string[] resourceNames )
        {
            //Contract.Requires( page != null );
            page.Html.ScriptReference( resourceNames.Select( rn => page.Resource( rn ) ).ToArray() );
        }
    }
}