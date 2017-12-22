using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO;
using Lpp.Objects;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    public class PermissionLocation : Entity
    {
        [Key, Column(Order=1)]
        public Guid PermissionID { get; set; }
        public virtual Permission Permission { get; set; }

        [Key, Column(Order = 2)]
        public PermissionAclTypes Type { get; set; }
    }
}
