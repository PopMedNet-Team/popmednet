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

namespace Lpp.Dns.HealthCare.FileDistribution.Views.FileDistribution
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
    using Lpp.Dns.HealthCare.FileDistribution;
    using Lpp.Dns.HealthCare.FileDistribution.Models;
    using Lpp.Mvc;
    using Lpp.Mvc.Application;
    using Lpp.Mvc.Controls;
    using Lpp.Utilities.Legacy;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/FileDistribution/Create.cshtml")]
    public partial class Create : System.Web.Mvc.WebViewPage<Lpp.Dns.HealthCare.FileDistribution.Models.FileDistributionModel>
    {
        public Create()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\FileDistribution\Create.cshtml"
   
    this.Stylesheet("FileDistribution.css");
    var id = Html.UniqueId();

            
            #line default
            #line hidden
WriteLiteral("\r\n \r\n<div");

WriteLiteral(" class=\" FileSelector ui-form\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"ui-form\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\'errorLocation\'");

WriteLiteral(" style=\"font-size: small; color: Gray;\"");

WriteLiteral("></div>\r\n        \r\n");

WriteLiteral("        ");

            
            #line 11 "..\..\Views\FileDistribution\Create.cshtml"
    Write(Html.Render<Lpp.Dns.HealthCare.FileUpload>().With(Model.RequestId, Model.RequestFileList, "UploadedFileList", "RemovedFilesList"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>        \r\n</div>\r\n ");

        }
    }
}
#pragma warning restore 1591