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

namespace Lpp.Dns.Portal.Views.SharedFolders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
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
    using Lpp.Dns;
    using Lpp.Dns.Data;
    using Lpp.Dns.Portal;
    using Lpp.Dns.Portal.Controllers;
    using Lpp.Dns.Portal.Models;
    using Lpp.Dns.Portal.Views;
    using Lpp.Mvc;
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/SharedFolders/Setup.cshtml")]
    public partial class Setup : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Setup()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Views\SharedFolders\Setup.cshtml"
  
    var shareUrl = Html.Raw( 
        Url.Action( ( SharedFolderController c ) => c.ShareRequest( "__folder__", "__req__" ) )
        .Replace( "__folder__", "' + folderId + '" )
        .Replace( "__req__", "' + requestId + '" ) );

            
            #line default
            #line hidden
WriteLiteral("\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n    require([\'");

            
            #line 8 "..\..\Views\SharedFolders\Setup.cshtml"
         Write(this.Resource("SharedFolders.js"));

            
            #line default
            #line hidden
WriteLiteral("\', \'networkTree\', \'requireCss\'], function (sf, nt, css) {\r\n        css(\'");

            
            #line 9 "..\..\Views\SharedFolders\Setup.cshtml"
        Write(this.Resource("SharedFolders.css"));

            
            #line default
            #line hidden
WriteLiteral("\');\r\n\r\n        function setup() { sf.initializeSharedFolders(function (folderId, " +
"requestId) { return \'");

            
            #line 11 "..\..\Views\SharedFolders\Setup.cshtml"
                                                                                          Write(shareUrl);

            
            #line default
            #line hidden
WriteLiteral("\'; }); }\r\n        setup();\r\n        $(nt).bind(\"treechanged\", setup);\r\n    });\r\n<" +
"/script>");

        }
    }
}
#pragma warning restore 1591