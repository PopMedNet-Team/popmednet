using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Dns.Portal.Models;
using System.Linq.Expressions;
using Lpp.Dns.Portal.Controllers;
using Lpp.Mvc.Controls;
using Lpp.Security;

namespace Lpp.Dns.Portal.Controls
{
    public class RootLayout : IRootLayout
    {
        [Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public static IUIWidgetFactory<IRootLayout> Factory 
        {
            get
            { 
                return UIWidget.Factory<IRootLayout>( html => new RootLayout { Html = html } ); 
            } 
        }

        public HtmlHelper Html { get; private set; }

        public string Value 
        { 
            get
            {
                return "~/Views/_Layout.cshtml";
            }
        }
    }
}