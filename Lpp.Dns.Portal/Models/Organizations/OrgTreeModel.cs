using System.Collections.Generic;
using Lpp.Dns.Data;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class OrgTreeModel
    {
        public IEnumerable<Organization> Organizations { get; set; }
        public TreeRenderMode Mode { get; set; }
    }
}