using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Dns.Data;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Dns.Portal.Models;
using System.Linq.Expressions;
using Lpp.Dns.Portal.Controllers;
using Lpp.Audit.UI;
using Lpp.Audit;

namespace Lpp.Dns.Portal.Models
{
    public class OrgChildSelectorModel
    {
        public string ValueDisplay { get; set; }
        public string OnChangeFunction { get; set; }
        public Func<UrlHelper, RoutedComputation<Controls.RenderOrgChildrenList>> Children { get; set; }
    }
}