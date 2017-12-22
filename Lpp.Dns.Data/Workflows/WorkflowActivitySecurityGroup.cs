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
    [Table("WorkflowActivitySecurityGroups")]
    public class WorkflowActivitySecurityGroup : Entity
    {
        [Key, Column(Order = 1)]
        public Guid WorkflowActivityID { get; set; }
        public virtual WorkflowActivity WorkflowActivity { get; set; }

        [Key, Column(Order = 2)]
        public Guid SecurityGroupID { get; set; }
        public virtual SecurityGroup SecurityGroup { get; set; }
    }
}
