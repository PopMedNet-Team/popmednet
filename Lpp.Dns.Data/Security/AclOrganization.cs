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
    [Table("AclOrganizations")]
    public class AclOrganization : Acl
    {
        public AclOrganization() { }

        [Key, Column(Order = 3)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
    
    internal class AclOrganizationSecurityConfiguration : DnsEntitySecurityConfiguration<AclOrganization>
    {
        public override IQueryable<AclOrganization> SecureList(DataContext db, IQueryable<AclOrganization> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.ManageSecurity
                };

            return from q in query join o in db.Filter(db.Organizations, identity, permissions) on q.OrganizationID equals o.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclOrganization[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Organization.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.ManageSecurity);
        }
    }

    internal class AclOrganizationDTOMappingConfiguration : EntityMappingConfiguration<AclOrganization, AclOrganizationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclOrganization, AclOrganizationDTO>> MapExpression
        {
            get
            {
                return (a) => new AclOrganizationDTO
                {
                    Allowed = a.Allowed,
                    OrganizationID = a.OrganizationID,
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
