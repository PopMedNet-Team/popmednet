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
    [Table("OrganizationRegistries")]
    public class OrganizationRegistry : Entity
    {
        [Key, Column(Order=0)]
        public Guid OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

        [Key, Column(Order=1)]
        public Guid RegistryID { get; set; }
        public virtual Registry Registry { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }

    internal class OrganizationRegistrySecurityConfiguration : DnsEntitySecurityConfiguration<OrganizationRegistry>
    {
        public override IQueryable<OrganizationRegistry> SecureList(DataContext db, IQueryable<OrganizationRegistry> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Organization.View,
                    PermissionIdentifiers.Registry.View
                };

            return from q in query join o in db.Filter(db.Organizations, identity, permissions) on q.OrganizationID equals o.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params OrganizationRegistry[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Registry.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Registry.Edit);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Organization.Edit, PermissionIdentifiers.Registry.Edit);
        }

        public override System.Linq.Expressions.Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return a => a.Organization.Registries.Any(r => objIDs.Contains(r.OrganizationID));
        }

        public override System.Linq.Expressions.Expression<Func<AclRegistry, bool>> RegistryFilter(params Guid[] objIDs)
        {
            return a => a.Registry.Organizations.Any(o => objIDs.Contains(o.OrganizationID));
        }
    }

    internal class OrganizationRegistryDtoMappingConfiguration : EntityMappingConfiguration<OrganizationRegistry, OrganizationRegistryDTO>
    {
        public override System.Linq.Expressions.Expression<Func<OrganizationRegistry, OrganizationRegistryDTO>> MapExpression
        {
            get
            {
                return (po) => new OrganizationRegistryDTO
                {
                    Organization = po.Organization.Name,
                    OrganizationID = po.OrganizationID,
                    Registry = po.Registry.Name,
                    RegistryID = po.RegistryID,
                    Type = po.Registry.Type,
                    Description = po.Description,
                    Acronym = po.Organization.Acronym,
                    OrganizationParent = po.Organization.ParentOrganization.Name
                };
            }
        }
    }
}
