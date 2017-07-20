using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("AclUsers")]
    public class AclUser : Acl
    {
        [Key, Column(Order = 3)]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }
    }

    internal class AclUserSecurityConfiguration : DnsEntitySecurityConfiguration<AclUser>
    {
        public override IQueryable<AclUser> SecureList(DataContext db, IQueryable<AclUser> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.User.ManageSecurity
                };

            return from q in query join u in db.Filter(db.Users, identity, permissions) on q.UserID equals u.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclUser[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.User.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.User.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.User.ManageSecurity);
        }
    }

    internal class AclUserDTOMappingConfiguration : EntityMappingConfiguration<AclUser, AclUserDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclUser, AclUserDTO>> MapExpression
        {
            get
            {
                return (a) => new AclUserDTO
                {
                    Allowed = a.Allowed,
                    Overridden = a.Overridden,
                    Permission = a.Permission.Name,
                    PermissionID = a.PermissionID,
                    SecurityGroup = a.SecurityGroup.Path,
                    SecurityGroupID = a.SecurityGroupID,
                    UserID = a.UserID
                };
            }
        }
    }
}
