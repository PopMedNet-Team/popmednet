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
    public abstract class BaseAcl : Entity
    {
        [Key, Column(Order = 1)]
        public Guid SecurityGroupID { get; set; }
        public virtual SecurityGroup SecurityGroup { get; set; }

        /// <summary>
        /// True if a child does not inherit the allowed property
        /// Should always be set to true when updating or inserting into an ACL from a controller
        /// </summary>
        public bool Overridden { get; set; }
    }
}
