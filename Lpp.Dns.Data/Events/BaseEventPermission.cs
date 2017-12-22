using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    public abstract class BaseEventPermission
    {
        public BaseEventPermission() { }

        [Key, Column(Order=1)]
        public Guid SecurityGroupID { get; set; }
        public virtual SecurityGroup SecurityGroup { get; set; }

        public bool Allowed { get; set; }

        public bool Overridden { get; set; }

        [Key, Column(Order=2)]
        public Guid EventID {get; set;}        
    }
}
