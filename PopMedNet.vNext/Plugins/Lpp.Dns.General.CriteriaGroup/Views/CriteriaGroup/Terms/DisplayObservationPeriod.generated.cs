﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lpp.Dns.General.CriteriaGroup.Views.CriteriaGroup.Terms
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
    using Lpp.Dns.General.CriteriaGroup;
    using Lpp.Dns.General.CriteriaGroup.Models;
    using Lpp.Mvc;
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    using Lpp.Utilities.Legacy;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/CriteriaGroup/Terms/DisplayObservationPeriod.cshtml")]
    public partial class DisplayObservationPeriod : System.Web.Mvc.WebViewPage<Lpp.Dns.General.CriteriaGroup.Models.ObservationPeriodModel>
    {
        public DisplayObservationPeriod()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\CriteriaGroup\Terms\DisplayObservationPeriod.cshtml"
   this.Stylesheet("CriteriaGroup.min.css"); 
            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"ObservationPeriodTerm Term panel panel-default\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"panel-heading\"");

WriteLiteral(">\r\n        Observation Period Range<img");

WriteLiteral(" src=\"/Content/img/icons/help16.gif\"");

WriteLiteral(" class=\"helptip\"");

WriteLiteral(" title=\"Dates may differ by network and are based on encounter dates.\"");

WriteLiteral(" />\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"panel-body\"");

WriteLiteral(">\r\n        <table");

WriteLiteral(" class=\"table\"");

WriteLiteral(">\r\n            <tr>\r\n                <th>Start Period</th>\r\n                <th>E" +
"nd Period</th>\r\n            </tr>\r\n            <tr>\r\n                <td>");

            
            #line 14 "..\..\Views\CriteriaGroup\Terms\DisplayObservationPeriod.cshtml"
                Write(Model.StartPeriod == null ? "" : Model.StartPeriod.Value.ToString("MM/dd/yyyy"));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                <td>");

            
            #line 15 "..\..\Views\CriteriaGroup\Terms\DisplayObservationPeriod.cshtml"
                Write(Model.EndPeriod == null ? "" : Model.EndPeriod.Value.ToString("MM/dd/yyyy"));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n            </tr>\r\n        </table>\r\n    </div>    \r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
