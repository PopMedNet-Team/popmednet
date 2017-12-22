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
    [Table("AclRegistries")]
    public class AclRegistry : Acl
    {
        [Key, Column(Order = 3)]
        public Guid RegistryID { get; set; }
        public virtual Registry Registry { get; set; }
    }

    internal class AclRegistrySecurityConfiguration : DnsEntitySecurityConfiguration<AclRegistry>
    {
        public override IQueryable<AclRegistry> SecureList(DataContext db, IQueryable<AclRegistry> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Registry.ManageSecurity
                };

            return from q in query join r in db.Filter(db.Registries, identity, permissions) on q.RegistryID equals r.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclRegistry[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Registry.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.ManageSecurity);
        }
    }

    internal class AclRegistryDTOMappingConfiguration : EntityMappingConfiguration<AclRegistry, AclRegistryDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclRegistry, AclRegistryDTO>> MapExpression
        {
            get
            {
                return (dm) => new AclRegistryDTO
                {
                    Allowed = dm.Allowed,
                    RegistryID = dm.RegistryID,
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
