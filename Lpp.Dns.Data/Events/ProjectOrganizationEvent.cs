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
    [Table("ProjectOrganizationEvents")]
    public class ProjectOrganizationEvent : BaseEventPermission
    {
        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [Key, Column(Order = 4)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual Event Event { get; set; }
    }

    internal class ProjectOrganizationEventSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectOrganizationEvent>
    {
        public override IQueryable<ProjectOrganizationEvent> SecureList(DataContext db, IQueryable<ProjectOrganizationEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity,
                    PermissionIdentifiers.Organization.ManageSecurity
                };

            return from e in query join p in db.Filter(db.Projects, identity, permissions) on e.ProjectID equals p.ID join o in db.Filter(db.Organizations, identity, permissions) on e.OrganizationID equals o.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectOrganizationEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization Events does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization Events does not have direct permissions for update, check it's parent project");
        }
    }

    internal class ProjectOrganizationEventDTOMappingConfiguration : EntityMappingConfiguration<ProjectOrganizationEvent, ProjectOrganizationEventDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ProjectOrganizationEvent, ProjectOrganizationEventDTO>> MapExpression
        {
            get
            {
                return (po) => new ProjectOrganizationEventDTO
                {
                    Allowed = po.Allowed,
                    OrganizationID = po.OrganizationID,
                    Overridden = po.Overridden,
                    ProjectID = po.ProjectID,
                    EventID = po.EventID,
                    Event = po.Event.Name,
                    SecurityGroup = po.SecurityGroup.Path,
                    SecurityGroupID = po.SecurityGroupID
                };
            }
        }
    }
}
