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

namespace Lpp.Dns.HealthCare.Views
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
    using Lpp.Dns.HealthCare;
    using Lpp.Dns.HealthCare.Controllers;
    using Lpp.Dns.HealthCare.Models;
    using Lpp.Mvc;
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    using Lpp.Utilities.Legacy;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/EnrollmentSelectorPlugin.cshtml")]
    public partial class EnrollmentSelectorPlugin : System.Web.Mvc.WebViewPage<Lpp.Dns.HealthCare.Models.EnrollmentSelectorPluginModel>
    {
        public EnrollmentSelectorPlugin()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\EnrollmentSelectorPlugin.cshtml"
  
    string fldPrior = string.Format("{0}_{1}", Model.ParentContext, "Prior");    
    string fldAfter = string.Format("{0}_{1}", Model.ParentContext, "After");
    string fldContinuous = string.Format("{0}_{1}", Model.ParentContext, "Continuous");

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"ui-groupbox\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"ui-groupbox-header\"");

WriteLiteral(">\r\n        <span>Enrollment Criteria</span>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n        <table>\r\n            <tr>\r\n                <td>\r\n                    <" +
"div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n                        <div>\r\n");

WriteLiteral("                            ");

            
            #line 19 "..\..\Views\EnrollmentSelectorPlugin.cshtml"
                       Write(Html.LabelFor(esm => esm.Prior));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 20 "..\..\Views\EnrollmentSelectorPlugin.cshtml"
                       Write(Html.TextBox(fldPrior, Model.Prior, new { id = fldPrior, style = "width: 30px", maxlength = 3 }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </t" +
"d>\r\n                <td>\r\n                    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n                        <div>\r\n");

WriteLiteral("                            ");

            
            #line 27 "..\..\Views\EnrollmentSelectorPlugin.cshtml"
                       Write(Html.LabelFor(esm => esm.After));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 28 "..\..\Views\EnrollmentSelectorPlugin.cshtml"
                       Write(Html.TextBox(fldAfter, Model.After, new { id = fldAfter, style = "width: 30px", maxlength = 3 }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </t" +
"d>\r\n                <td>\r\n                    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n                        <div>\r\n");

WriteLiteral("                            ");

            
            #line 35 "..\..\Views\EnrollmentSelectorPlugin.cshtml"
                       Write(Html.LabelFor(esm => esm.Continuous));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 36 "..\..\Views\EnrollmentSelectorPlugin.cshtml"
                       Write(Html.CheckBox(fldContinuous, Model.Continuous, new { id = fldContinuous }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </t" +
"d>\r\n            </tr>\r\n        </table>\r\n    </div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591