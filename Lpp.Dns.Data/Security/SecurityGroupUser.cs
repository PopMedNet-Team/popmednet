using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Utilities;

namespace Lpp.Dns.Data
{
    [Table("SecurityGroupUsers")]
    public class SecurityGroupUser : Entity
    {
        public SecurityGroupUser() { }

        [Key, Column(Order=0)]
        public Guid SecurityGroupID { get; set; }
        public SecurityGroup SecurityGroup { get; set; }

        [Key, Column(Order=1)]
        public Guid UserID { get; set; }
        public User User { get; set; }

        public bool Overridden { get; set; }
    }
}
