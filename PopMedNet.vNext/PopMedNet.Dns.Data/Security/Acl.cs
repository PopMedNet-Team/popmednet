using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace PopMedNet.Dns.Data
{
    public abstract class Acl : BaseAcl, IAcl
    {
        public bool Allowed { get; set; }
        public virtual Guid PermissionID { get; set; }
        public virtual Permission? Permission { get; set; }

        //public static Expression<Func<Acl, Acl>> CreateAcl = (s) => new Acl
        //{
        //    Allowed = s.Allowed,
        //    Overridden = s.Overridden,
        //    PermissionID = s.PermissionID,
        //    SecurityGroupID = s.SecurityGroupID
        //};
    }
}
