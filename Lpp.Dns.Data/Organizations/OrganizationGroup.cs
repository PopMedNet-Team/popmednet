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
    [Table("OrganizationGroups")]
    public class OrganizationGroup : Entity
    {
        [Key, Column(Order=1)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        [Key, Column(Order = 2)]
        public Guid GroupID { get; set; }
        public virtual Group Group { get; set; }
    }

    internal class OrganizationGroupSecurityConfiguration : DnsEntitySecurityConfiguration<OrganizationGroup>
    {
        public override IQueryable<OrganizationGroup> SecureList(DataContext db, IQueryable<OrganizationGroup> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.View,
                    PermissionIdentifiers.Group.View
                };

            return from q in query join o in db.Filter(db.Organizations, identity, permissions) on q.OrganizationID equals o.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params OrganizationGroup[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Group.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Group.Edit);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Group.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Groups.Any(g => objIDs.Contains(g.OrganizationID));
        }

        public override System.Linq.Expressions.Expression<Func<AclGroup, bool>> GroupFilter(params Guid[] objIDs)
        {
            return a => a.Group.Organizations.Any(o => objIDs.Contains(o.OrganizationID));
        }
    }

    internal class OrganizationGroupDtoMappingConfiguration : EntityMappingConfiguration<OrganizationGroup, OrganizationGroupDTO>
    {
        public override System.Linq.Expressions.Expression<Func<OrganizationGroup, OrganizationGroupDTO>> MapExpression
        {
            get
            {
                return (po) => new OrganizationGroupDTO
                {
                    Organization = po.Organization.Name,
                    OrganizationID = po.OrganizationID,
                    Group = po.Group.Name,
                    GroupID = po.GroupID
                };
            }
        }
    }
}
