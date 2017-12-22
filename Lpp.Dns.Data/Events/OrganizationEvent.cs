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
    [Table("OrganizationEvents")]
    public class OrganizationEvent : BaseEventPermission
    {
        public OrganizationEvent() { }

        [Key, Column(Order = 3)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual Event Event { get; set; }
    }

    internal class OrganizationEventSecurityConfiguration : DnsEntitySecurityConfiguration<OrganizationEvent>
    {
        public override IQueryable<OrganizationEvent> SecureList(DataContext db, IQueryable<OrganizationEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.ManageSecurity
                };

            return from q in query join o in db.Filter(db.Organizations, identity, permissions) on q.OrganizationID equals o.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params OrganizationEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.OrganizationID).ToArray(), PermissionIdentifiers.Organization.ManageSecurity); 
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Organization Events does not have direct permissions for delete, check it's parent organization");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Organization Events does not have direct permissions for update, check it's parent organization");
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => objIDs.Contains(a.OrganizationID);
        }
    }

    internal class OrganizationEventDtoMappingConfiguration : EntityMappingConfiguration<OrganizationEvent, OrganizationEventDTO>
    {
        public override System.Linq.Expressions.Expression<Func<OrganizationEvent, OrganizationEventDTO>> MapExpression
        {
            get
            {
                return (a) => new OrganizationEventDTO
                {
                    Allowed = a.Allowed,
                    Event = a.Event.Name,
                    EventID = a.EventID,
                    OrganizationID = a.OrganizationID,
                    Overridden = a.Overridden,
                    SecurityGroup = a.SecurityGroup.Path,
                    SecurityGroupID = a.SecurityGroupID                    
                };
            }
        }
    }
}
