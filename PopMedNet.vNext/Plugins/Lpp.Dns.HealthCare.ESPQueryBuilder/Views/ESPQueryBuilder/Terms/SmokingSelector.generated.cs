﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms
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
    using Lpp.Dns.HealthCare.Controllers;
    using Lpp.Dns.HealthCare.ESPQueryBuilder;
    using Lpp.Dns.HealthCare.ESPQueryBuilder.Models;
    using Lpp.Dns.HealthCare.ESPQueryBuilder.Views;
    using Lpp.Mvc;
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ESPQueryBuilder/Terms/SmokingSelector.cshtml")]
    public partial class SmokingSelector : System.Web.Mvc.WebViewPage<Lpp.Dns.HealthCare.ESPQueryBuilder.Models.ESPQueryBuilderModel>
    {
        public SmokingSelector()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\ESPQueryBuilder\Terms\SmokingSelector.cshtml"
  
    var id = Html.UniqueId();
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"SmokingSelectorTerm Term panel panel-default\"");

WriteLiteral(">\r\n    <input");

WriteLiteral(" name=\"TermName\"");

WriteLiteral(" value=\"SmokingSelector\"");

WriteLiteral(" hidden=\"hidden\"");

WriteLiteral(" style=\"display:none\"");

WriteLiteral(" />\r\n    <div");

WriteLiteral(" class=\"panel-heading\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"ui-button-remove\"");

WriteLiteral("></div>\r\n        Tobacco Use\r\n    </div>\r\n    <div");

WriteAttribute("id", Tuple.Create(" id=\"", 401), Tuple.Create("\"", 448)
            
            #line 12 "..\..\Views\ESPQueryBuilder\Terms\SmokingSelector.cshtml"
, Tuple.Create(Tuple.Create("", 406), Tuple.Create<System.Object, System.Int32>(string.Format("binding-container{0}", id)
            
            #line default
            #line hidden
, 406), false)
);

WriteLiteral(" class=\"panel-body SmokingSelector\"");

WriteLiteral(">\r\n        <table");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(">\r\n            <thead>\r\n                <tr>\r\n                    <th");

WriteLiteral(" style=\"width:20px;\"");

WriteLiteral("></th>\r\n                    <th>Tobacco Use</th>\r\n                </tr>\r\n        " +
"    </thead>\r\n            <tbody");

WriteLiteral(" data-bind=\"foreach:Smokings\"");

WriteLiteral(">\r\n                <tr>\r\n                    <td><input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"value:StratificationCategoryId, checked: $root.SelectedSmokings\"");

WriteLiteral(" /></td>\r\n                    <td");

WriteLiteral(" data-bind=\"text:CategoryText\"");

WriteLiteral("></td>\r\n                </tr>\r\n            </tbody>\r\n        </table>\r\n        <i" +
"nput");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" name=\"Smoking\"");

WriteAttribute("id", Tuple.Create(" id=\"", 1084), Tuple.Create("\"", 1121)
            
            #line 27 "..\..\Views\ESPQueryBuilder\Terms\SmokingSelector.cshtml"
, Tuple.Create(Tuple.Create("", 1089), Tuple.Create<System.Object, System.Int32>(string.Format("Smoking{0}", id)
            
            #line default
            #line hidden
, 1089), false)
);

WriteLiteral(" data-bind=\"value: SelectedSmokings\"");

WriteLiteral(" />\r\n    </div>\r\n\r\n    <script>\r\n        $(function () {\r\n            ko.applyBin" +
"dings({\r\n                Smokings: ");

            
            #line 33 "..\..\Views\ESPQueryBuilder\Terms\SmokingSelector.cshtml"
                     Write(Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model.SmokingSelections)));

            
            #line default
            #line hidden
WriteLiteral(",\r\n                SelectedSmokings: ko.observableArray((\'");

            
            #line 34 "..\..\Views\ESPQueryBuilder\Terms\SmokingSelector.cshtml"
                                                  Write(Model.Smoking);

            
            #line default
            #line hidden
WriteLiteral("\' || \'\').split(\',\'))\r\n            }, $(\'#");

            
            #line 35 "..\..\Views\ESPQueryBuilder\Terms\SmokingSelector.cshtml"
              Write(string.Format("binding-container{0}", id));

            
            #line default
            #line hidden
WriteLiteral("\')[0]);\r\n        });\r\n\r\n    </script>\r\n\r\n</div>");

        }
    }
}
#pragma warning restore 1591