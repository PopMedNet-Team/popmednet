using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("RegistryEvents")]
    public class RegistryEvent : BaseEventPermission
    {
        public RegistryEvent() { }

        [Key, Column(Order = 3)]
        public Guid RegistryID { get; set; }
        public virtual Registry Registry { get; set; }

        public virtual Event Event { get; set; }
    }
}
