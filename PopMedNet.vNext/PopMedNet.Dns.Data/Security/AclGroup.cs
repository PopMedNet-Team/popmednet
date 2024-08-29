using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclGroups")]
    public class AclGroup : Acl
    {
        public Guid GroupID { get; set; }
        public virtual Group? Group { get; set; }
    }
    internal class AclGroupConfiguration : IEntityTypeConfiguration<AclGroup>
    {
        public void Configure(EntityTypeBuilder<AclGroup> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.GroupID }).HasName("PK_dbo.AclGroups");
        }
    }

    internal class AclGroupSecurityConfiguration : DnsEntitySecurityConfiguration<AclGroup>
    {

        public override IQueryable<AclGroup> SecureList(DataContext db, IQueryable<AclGroup> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Group.ManageSecurity
                };

            return from q in query join g in db.Filter(db.Groups, identity, permissions) on q.GroupID equals g.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclGroup[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Group.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Group.ManageSecurity);
        }
    }

    public class AclGroupMappingProfile : AutoMapper.Profile
    {
        public AclGroupMappingProfile()
        {
            CreateMap<AclGroup, DTO.AclGroupDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
