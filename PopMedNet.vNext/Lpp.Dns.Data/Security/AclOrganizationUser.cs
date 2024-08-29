using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("AclOrganizationUsers")]
    public class AclOrganizationUser : Acl
    {
        [Key, Column(Order = 3)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        [Key, Column(Order = 4)]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }
    }
}
