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

namespace Lpp.Dns.HealthCare.I2B2.Views.I2B2QueryBuilder
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
    
    #line 1 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
    using Lpp.Dns;
    
    #line default
    #line hidden
    using Lpp.Dns.HealthCare.Controllers;
    using Lpp.Dns.HealthCare.I2B2;
    using Lpp.Dns.HealthCare.I2B2.Models;
    using Lpp.Mvc;
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/I2B2QueryBuilder/Config.cshtml")]
    public partial class Config : System.Web.Mvc.WebViewPage<ConfigModel>
    {
        public Config()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
  
    Layout = null;
    var id = Html.UniqueId();

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"Value ModelConfigForm\"");

WriteAttribute("id", Tuple.Create(" id=\"", 130), Tuple.Create("\"", 138)
            
            #line 8 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
, Tuple.Create(Tuple.Create("", 135), Tuple.Create<System.Object, System.Int32>(id
            
            #line default
            #line hidden
, 135), false)
);

WriteLiteral(">\r\n    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"Link\"");

WriteLiteral(">[configure]</a>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function () {\r\n            var dlg = $(\"#");

            
            #line 12 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
                      Write(id);

            
            #line default
            #line hidden
WriteLiteral(" > .ModelConfig\");\r\n            var link = $(\"#");

            
            #line 13 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
                       Write(id);

            
            #line default
            #line hidden
WriteLiteral(" > a.Link\");\r\n            link.click(function () {\r\n                dlg.dialog({\r" +
"\n                    modal: true, title: \"");

            
            #line 16 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
                                    Write(Model.Model.Name);

            
            #line default
            #line hidden
WriteLiteral(@""",
                    width: 540, buttons: {
                        OK: function () {
                            dlg.dialog(""close"");
                        },
                        Cancel: function () { dlg.dialog(""close""); }
                    }
                });
                return false;
            });
        });
    </script>

    <div");

WriteAttribute("id", Tuple.Create(" id=\"", 842), Tuple.Create("\"", 862)
            
            #line 29 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
, Tuple.Create(Tuple.Create("", 847), Tuple.Create<System.Object, System.Int32>(Model.Model.ID
            
            #line default
            #line hidden
, 847), false)
);

WriteLiteral(" class=\"ModelConfig\"");

WriteLiteral(" style=\"display: none;\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"Field\"");

WriteLiteral(">\r\n                <label>Service URL</label>\r\n                <input");

WriteLiteral(" id=\"ServiceURL\"");

WriteLiteral(" name=\"ServiceURL\"");

WriteLiteral(" type=\"text\"");

WriteAttribute("value", Tuple.Create(" value=\"", 1085), Tuple.Create("\"", 1176)
            
            #line 33 "..\..\Views\I2B2QueryBuilder\Config.cshtml"
, Tuple.Create(Tuple.Create("", 1093), Tuple.Create<System.Object, System.Int32>(Model.Properties.ContainsKey("ServiceURL") ? Model.Properties["ServiceURL"] : ""
            
            #line default
            #line hidden
, 1093), false)
);

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n</div>");

        }
    }
}
#pragma warning restore 1591
