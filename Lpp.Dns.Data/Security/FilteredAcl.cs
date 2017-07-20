using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    public class FilteredAcl
    {
        public Guid SecurityGroupID { get; set; }
        public Guid PermissionIdentifiers { get; set; }
        public bool Allowed { get; set; }
    }
}
