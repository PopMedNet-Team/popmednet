using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclRegistries")]
    public class AclRegistry : Acl
    {
        public Guid RegistryID { get; set; }
        public virtual Registry? Registry { get; set; }
    }
    internal class AclRegistryConfiguration : IEntityTypeConfiguration<AclRegistry>
    {
        public void Configure(EntityTypeBuilder<AclRegistry> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.RegistryID }).HasName("PK_dbo.AclRegistries");
        }
    }

    internal class AclRegistrySecurityConfiguration : DnsEntitySecurityConfiguration<AclRegistry>
    {
        public override IQueryable<AclRegistry> SecureList(DataContext db, IQueryable<AclRegistry> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Registry.ManageSecurity
                };

            return from q in query join r in db.Filter(db.Registries, identity, permissions) on q.RegistryID equals r.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclRegistry[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Registry.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Registry.ManageSecurity);
        }
    }

    public class AclRegistryMappingProfile : AutoMapper.Profile
    {
        public AclRegistryMappingProfile()
        {
            CreateMap<AclRegistry, DTO.AclRegistryDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
