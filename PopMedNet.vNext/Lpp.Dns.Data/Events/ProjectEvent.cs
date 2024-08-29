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
    [Table("ProjectEvents")]
    public class ProjectEvent : BaseEventPermission
    {
        public ProjectEvent() { }

        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        public virtual Event Event { get; set; }
    }

    internal class ProjectEventSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectEvent>
    {
        public override IQueryable<ProjectEvent> SecureList(DataContext db, IQueryable<ProjectEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from e in query join p in db.Filter(db.Projects, identity, permissions) on e.ProjectID equals p.ID  select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Events does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Events does not have direct permissions for update, check it's parent project");
        }
    }

    internal class ProjectEventDTOMappingConfiguration : EntityMappingConfiguration<ProjectEvent, ProjectEventDTO>
    {
        public override System.Linq.Expressions.Expression<Func<ProjectEvent, ProjectEventDTO>> MapExpression
        {
            get
            {
                return (pe) => new ProjectEventDTO
                {
                    Allowed = pe.Allowed,
                    EventID = pe.EventID,
                    Event = pe.Event.Name,
                    Overridden = pe.Overridden,
                    ProjectID = pe.ProjectID,
                    SecurityGroup = pe.SecurityGroup.Path,
                    SecurityGroupID = pe.SecurityGroupID,
                };
            }
        }
    }
}
