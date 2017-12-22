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
    [Table("GroupEvents")]
    public class GroupEvent : BaseEventPermission
    {
        public GroupEvent() { }

        [Key, Column(Order = 3)]
        public Guid GroupID { get; set; }
        public virtual Group Group { get; set; }

        public virtual Event Event { get; set; }
    }

    internal class GroupEventSecurityConfiguration : DnsEntitySecurityConfiguration<GroupEvent>
    {
        public override IQueryable<GroupEvent> SecureList(DataContext db, IQueryable<GroupEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Group.ManageSecurity
                };

            return from e in query join g in db.Filter(db.Groups, identity, permissions) on e.GroupID equals g.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params GroupEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.GroupID).ToArray(), PermissionIdentifiers.Group.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Group Events does not have direct permissions for delete, check it's parent user");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Group Events does not have direct permissions for update, check it's parent user");
        }
    }

    internal class GroupEventDTOMappingConfiguration : EntityMappingConfiguration<GroupEvent, GroupEventDTO>
    {
        public override System.Linq.Expressions.Expression<Func<GroupEvent, GroupEventDTO>> MapExpression
        {
            get
            {
                return (g) => new GroupEventDTO
                {
                    Allowed = g.Allowed,
                    Event = g.Event.Name,
                    EventID = g.EventID,
                    GroupID = g.GroupID,
                    Overridden = g.Overridden,
                    SecurityGroup = g.SecurityGroup.Path,
                    SecurityGroupID = g.SecurityGroupID
                };
            }
        }
    }
}

