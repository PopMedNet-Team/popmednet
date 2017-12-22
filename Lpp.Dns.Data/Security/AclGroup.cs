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
    [Table("AclGroups")]
    public class AclGroup : Acl
    {
        [Key, Column(Order=3)]
        public Guid GroupID { get; set; }
        public virtual Group Group { get; set; }
    }

    internal class AclGroupSecurityConfiguration : DnsEntitySecurityConfiguration<AclGroup>
    {

        public override IQueryable<AclGroup> SecureList(DataContext db, IQueryable<AclGroup> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Group.ManageSecurity
                };

            return from q in query join g in db.Filter(db.Groups, identity, permissions) on q.GroupID equals g.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclGroup[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Group.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.ManageSecurity);
        }
    }

    internal class AclGroupDTOMappingConfiguration : EntityMappingConfiguration<AclGroup, AclGroupDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclGroup, AclGroupDTO>> MapExpression
        {
            get
            {
                return (dm) => new AclGroupDTO
                {
                    Allowed = dm.Allowed,
                    GroupID = dm.GroupID,
                    Overridden = dm.Overridden,
                    Permission = dm.Permission.Name,
                    PermissionID = dm.PermissionID,
                    SecurityGroup = dm.SecurityGroup.Path,
                    SecurityGroupID = dm.SecurityGroupID
                };
            }
        }
    }
}
