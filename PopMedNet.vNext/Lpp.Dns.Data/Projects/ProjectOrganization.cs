using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Objects;
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
    [Table("ProjectOrganizations")]
    public class ProjectOrganization : Entity
    {
        [Key, Column(Order = 1)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [Key, Column(Order = 2)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }

    internal class ProjectOrganizationSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectOrganization>
    {
        public override IQueryable<ProjectOrganization> SecureList(DataContext db, IQueryable<ProjectOrganization> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.View
                };

            return from pdm in query join p in db.Filter(db.Projects, identity, permissions) on pdm.ProjectID equals p.ID select pdm;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectOrganization[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization does not have direct permissions for update, check it's parent project");
        }
    }

    internal class ProjectOrganizationDtoMappingConfiguration : EntityMappingConfiguration<ProjectOrganization, ProjectOrganizationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ProjectOrganization, ProjectOrganizationDTO>> MapExpression
        {
            get
            {
                return (po) => new ProjectOrganizationDTO
                {
                    Organization = po.Organization.Name,
                    OrganizationID = po.OrganizationID,
                    Project = po.Project.Name,
                    ProjectID = po.ProjectID
                };
            }
        }
    }
}
