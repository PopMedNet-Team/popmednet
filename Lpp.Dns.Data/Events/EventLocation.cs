using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("EventLocations")]
    public class EventLocation : Entity
    {
        public EventLocation() { }

        [Key, Column(Order = 1)]
        public Guid EventID { get; set; }
        public Event Event { get; set; }
        [Key, Column(Order = 2)]
        public PermissionAclTypes Location { get; set; }
    }
}
