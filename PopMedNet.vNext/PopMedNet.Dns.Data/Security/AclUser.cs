using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclUsers")]
    public class AclUser : Acl
    {
        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
    }
    internal class AclUserConfiguration : IEntityTypeConfiguration<AclUser>
    {
        public void Configure(EntityTypeBuilder<AclUser> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.UserID }).HasName("PK_dbo.AclUsers");
        }
    }

    internal class AclUserSecurityConfiguration : DnsEntitySecurityConfiguration<AclUser>
    {
        public override IQueryable<AclUser> SecureList(DataContext db, IQueryable<AclUser> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.User.ManageSecurity
                };

            return from q in query join u in db.Filter(db.Users, identity, permissions) on q.UserID equals u.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclUser[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.User.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.User.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.User.ManageSecurity);
        }
    }

    public class AclUserMappingProfile : AutoMapper.Profile
    {
        public AclUserMappingProfile()
        {
            CreateMap<AclUser, DTO.AclUserDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
