using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Mvc;
using Lpp.Dns.Portal.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class OrgChildrenSelectorModel
    {
        public string ChooseFunctionName { get; set; }
        public string DialogTitle { get; set; }
        public RoutedComputation<RenderOrgChildrenList> Children { get; set; }
        public IQueryable<Organization> Organizations { get; set; }
    }
}