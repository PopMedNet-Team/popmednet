﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lpp.Mvc.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    
    #line 1 "..\..\Views\ClientSettings.cshtml"
    using System.Security.Policy;
    
    #line default
    #line hidden
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Lpp;
    using Lpp.Mvc;
    using Lpp.Mvc.Controls;
    using Lpp.Utilities.Legacy;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ClientSettings.cshtml")]
    public partial class ClientSettings : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ClientSettings()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\ClientSettings.cshtml"
Write(Layout);

            
            #line default
            #line hidden
WriteLiteral(" = null\r\n\r\nfunction clientSetttings(){\r\n    return {\r\n        set: function( key," +
" value )\r\n        {\r\n            $.post( \'");

            
            #line 8 "..\..\Views\ClientSettings.cshtml"
                Write(Html.Raw( 
                Url.Action( (ClientSettingsController c) => c.SetSetting( "__key__", "__value__" ) )
                    .Replace( "__key__", "' + key +'" )
                    .Replace( "__value__", "' + value +'" )
                ));

            
            #line default
            #line hidden
WriteLiteral("\' )\r\n                .success( function() {} )\r\n                .error( function(" +
") { console.log( \"Thare was a problem trying to set setting \" + key ); } );\r\n   " +
"     }\r\n    };\r\n}");

        }
    }
}
#pragma warning restore 1591
