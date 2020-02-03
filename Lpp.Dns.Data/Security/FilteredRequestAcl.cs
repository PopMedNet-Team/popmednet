using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    public class FilteredRequestAcl
    {
        public Guid SecurityGroupID { get; set; }
        public int Permission { get; set; }
        public Guid RequestTypeID { get; set; }
        public bool Overridden { get; set; }
    }
}
