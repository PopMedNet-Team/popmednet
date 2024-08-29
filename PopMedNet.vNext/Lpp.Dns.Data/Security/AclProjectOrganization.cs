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
    [Table("AclProjectOrganizations")]
    public class AclProjectOrganization : Acl
    {
        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [Key, Column(Order = 4)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }

    internal class AclProjectOrganizationSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectOrganization>
    {
        public override IQueryable<AclProjectOrganization> SecureList(DataContext db, IQueryable<AclProjectOrganization> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectOrganization[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }
    }

    internal class AclProjectOrganizationDTOMappingConfiguration : EntityMappingConfiguration<AclProjectOrganization, AclProjectOrganizationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<AclProjectOrganization, AclProjectOrganizationDTO>> MapExpression
        {
            get
            {
                return (po) => new AclProjectOrganizationDTO
                {
                    Allowed = po.Allowed,
                    OrganizationID = po.OrganizationID,
                    Overridden = po.Overridden,
                    Permission = po.Permission.Name,
                    PermissionID = po.PermissionID,
                    ProjectID = po.ProjectID,
                    SecurityGroup = po.SecurityGroup.Path,
                    SecurityGroupID = po.SecurityGroupID
                };
            }
        }
    }
}
