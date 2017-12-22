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
    [Table("ProjectDataMartEvents")]
    public class ProjectDataMartEvent : BaseEventPermission
    {
        [Key, Column(Order = 3)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [Key, Column(Order = 4)]
        public Guid DataMartID { get; set; }
        public virtual DataMart DataMart { get; set; }

        public virtual Event Event { get; set; }
    }

    internal class ProjectDataMartEventSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectDataMartEvent>
    {
        public override IQueryable<ProjectDataMartEvent> SecureList(DataContext db, IQueryable<ProjectDataMartEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity,
                    PermissionIdentifiers.DataMart.ManageSecurity
                };

            return from e in query join p in db.Filter(db.Projects, identity, permissions) on e.ProjectID equals p.ID join dm in db.Filter(db.DataMarts, identity, permissions) on e.DataMartID equals dm.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectDataMartEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.ManageSecurity); 
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart Events does not have direct permissions for delete, check it's parent Project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart Events does not have direct permissions for update, check it's parent Project");
        }
    }

    internal class ProjectDataMartEventDTOMappingConfiguration : EntityMappingConfiguration<ProjectDataMartEvent, ProjectDataMartEventDTO>
    {

        public override System.Linq.Expressions.Expression<Func<ProjectDataMartEvent, ProjectDataMartEventDTO>> MapExpression
        {
            get
            {
                return (a) => new ProjectDataMartEventDTO
                {
                    Allowed = a.Allowed,
                    DataMartID = a.DataMartID,
                    Overridden = a.Overridden,
                    ProjectID = a.ProjectID,
                    EventID = a.EventID,
                    Event = a.Event.Name,
                    SecurityGroup = a.SecurityGroup != null ? a.SecurityGroup.Path : null,
                    SecurityGroupID = a.SecurityGroupID
                };
            }
        }

    }
}
