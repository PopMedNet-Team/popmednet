using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects;
using Lpp.Utilities.Security;
using Lpp.Utilities;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Data
{
    /// <summary>
    /// This is all global level Acl entries that are not linked to any other objects
    /// </summary>
    [Table("AclGlobal")]
    public class AclGlobal : Acl
    {
        public AclGlobal() { }
    }

    internal class AclGlobalSecurityConfiguration : DnsEntitySecurityConfiguration<AclGlobal>
    {
        public override IQueryable<AclGlobal> SecureList(DataContext db, IQueryable<AclGlobal> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Portal.ManageSecurity
                };

            var globalAcls = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Portal.ManageSecurity);

            if (globalAcls.Any() && globalAcls.All(a => a.Allowed))
            {
                return query;
            }
            else
            {
                return new AclGlobal[] { }.AsQueryable();
            }
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclGlobal[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity);
        }
    }

    internal class AclGlobalDTOMappingConfiguration : EntityMappingConfiguration<AclGlobal, AclDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclGlobal, AclDTO>> MapExpression
        {
            get
            {
                return (a) => new AclDTO { 
                    Allowed = a.Allowed,
                    Overridden = a.Overridden,
                    Permission = a.Permission.Name,
                    PermissionID = a.PermissionID,
                    SecurityGroup = a.SecurityGroup.Path,
                    SecurityGroupID = a.SecurityGroupID
                };
            }
        }
    }
}
