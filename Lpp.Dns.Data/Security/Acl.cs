using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;

namespace Lpp.Dns.Data
{
    public class Acl : BaseAcl, IAcl
    {
        public Acl() { }


        public bool Allowed { get; set; }        

        [Key, Column(Order = 2)]
        public Guid PermissionID { get; set; }
        public virtual Permission Permission { get; set; }

        public static Expression<Func<Acl, Acl>> CreateAcl = (s) => new Acl
        {
            Allowed = s.Allowed,
            Overridden = s.Overridden,
            PermissionID = s.PermissionID,
            SecurityGroupID = s.SecurityGroupID
        };
    }

}
