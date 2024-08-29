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
    [Table("UserEvents")]
    public class UserEvent : BaseEventPermission
    {
        public UserEvent() { }

        [Key, Column(Order = 3)]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }

        public virtual Event Event { get; set; }
    }

    internal class UserEventSecurityConfiguration : DnsEntitySecurityConfiguration<UserEvent>
    {
        public override IQueryable<UserEvent> SecureList(DataContext db, IQueryable<UserEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.User.ManageSecurity
                };

            return from e in query join u in db.Filter(db.Users, identity, permissions) on e.UserID equals u.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params UserEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.UserID).ToArray(), PermissionIdentifiers.User.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("User Events does not have direct permissions for delete, check it's parent user");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("User Events does not have direct permissions for update, check it's parent user");
        }
    }

    internal class UserEventDTOMappingConfiguration : EntityMappingConfiguration<UserEvent, UserEventDTO>
    {
        public override System.Linq.Expressions.Expression<Func<UserEvent, UserEventDTO>> MapExpression
        {
            get
            {
                return (a) => new UserEventDTO
                {
                    Allowed = a.Allowed,
                    Event = a.Event.Name,
                    EventID = a.EventID,
                    Overridden = a.Overridden,
                    SecurityGroup = a.SecurityGroup.Path,
                    SecurityGroupID = a.SecurityGroupID,
                    UserID = a.UserID
                };
            }
        }
    }
}
