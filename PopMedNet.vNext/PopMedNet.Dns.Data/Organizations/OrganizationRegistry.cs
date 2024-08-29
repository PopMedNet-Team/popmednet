using PopMedNet.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("OrganizationRegistries")]
    public class OrganizationRegistry : Entity
    {
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
        public Guid RegistryID { get; set; }
        public virtual Registry? Registry { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
    }
    internal class OrganizationRegistryConfiguration : IEntityTypeConfiguration<OrganizationRegistry>
    {
        public void Configure(EntityTypeBuilder<OrganizationRegistry> builder)
        {
            builder.HasKey(e => new { e.OrganizationID, e.RegistryID }).HasName("PK_dbo.OrganizationRegistries");
        }
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
            return a => a.Organization != null && a.Organization.Registries.Any(r => objIDs.Contains(r.OrganizationID));
        }

        public override System.Linq.Expressions.Expression<Func<AclRegistry, bool>> RegistryFilter(params Guid[] objIDs)
        {
            return a => a.Registry != null && a.Registry.Organizations.Any(o => objIDs.Contains(o.OrganizationID));
        }
    }

    public class OrganizationRegistryMappingProfile : AutoMapper.Profile
    {
        public OrganizationRegistryMappingProfile()
        {
            CreateMap<OrganizationRegistry, DTO.OrganizationRegistryDTO>()
                .ForMember(r => r.Registry, o => o.MapFrom(r => r.Registry.Name))
                .ForMember(r => r.Organization, o => o.MapFrom(r => r.Organization.Name))
                .ForMember(r => r.OrganizationParent, o => o.MapFrom(r => r.Organization != null && r.Organization.ParentOrganization != null ? r.Organization.ParentOrganization.Name : null));
        }
    }
}
