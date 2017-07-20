using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    [Table("GlobalEvents")]
    public class GlobalEvent : BaseEventPermission
    {
        public GlobalEvent() { }

        public virtual Event Event { get; set; }
    }

    internal class GlobalEventSecurityConfiguration : DnsEntitySecurityConfiguration<GlobalEvent>
    {
        public override IQueryable<GlobalEvent> SecureList(DataContext db, IQueryable<GlobalEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            var globalAcls = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Portal.ManageSecurity);

            if (globalAcls.Any() && globalAcls.All(a => a.Allowed)) {
                return query;
            } else {
                return new GlobalEvent[] {}.AsQueryable();
            }
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params GlobalEvent[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.ManageSecurity); 
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity); //This is only valid because there is no parent.
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity);
        }
    }

    internal class BaseEventPermissionDtoMappingConfiguration : EntityMappingConfiguration<GlobalEvent, BaseEventPermissionDTO>
    {
        public override System.Linq.Expressions.Expression<Func<GlobalEvent, BaseEventPermissionDTO>> MapExpression
        {
            get
            {
                return (p) => new BaseEventPermissionDTO
                {
                    Allowed = p.Allowed,
                    Event = p.Event.Name,
                    EventID = p.EventID,
                    Overridden = p.Overridden,
                    SecurityGroup = p.SecurityGroup.Path,
                    SecurityGroupID = p.SecurityGroupID
                };
            }
        }
    }
}
