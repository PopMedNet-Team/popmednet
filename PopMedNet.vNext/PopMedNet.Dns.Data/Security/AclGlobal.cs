using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    /// <summary>
    /// This is all global level Acl entries that are not linked to any other objects
    /// </summary>
    [Table("AclGlobal")]
    public class AclGlobal : Acl
    {
        public AclGlobal() { }
    }
    internal class AclGlobalConfiguration : IEntityTypeConfiguration<AclGlobal>
    {
        public void Configure(EntityTypeBuilder<AclGlobal> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID }).HasName("PK_dbo.AclGlobal");
        }
    }

    internal class AclGlobalSecurityConfiguration : DnsEntitySecurityConfiguration<AclGlobal>
    {
        public override IQueryable<AclGlobal> SecureList(DataContext db, IQueryable<AclGlobal> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Portal.ManageSecurity
                };

            var globalAcls = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Portal.ManageSecurity);

            if (globalAcls.Any() && globalAcls.All(a => a.Allowed))
            {
                return query;
            }
            else
            {
                return new AclGlobal[] { }.AsQueryable();
            }
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclGlobal[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Portal.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Portal.ManageSecurity);
        }
    }

    public class AclGlobalMappingProfile : AutoMapper.Profile
    {
        public AclGlobalMappingProfile()
        {
            CreateMap<AclGlobal, DTO.AclDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }

}
