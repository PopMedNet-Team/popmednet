using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("AclDataMarts")]
    public class AclDataMart : Acl
    {
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }
    }


    internal class AclDataMartConfiguration : IEntityTypeConfiguration<AclDataMart>
    {
        public void Configure(EntityTypeBuilder<AclDataMart> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.DataMartID }).HasName("PK_dbo.AclDataMarts");
        }
    }

    internal class AclDataMartSecurityConfiguration : DnsEntitySecurityConfiguration<AclDataMart>
    {

        public override IQueryable<AclDataMart> SecureList(DataContext db, IQueryable<AclDataMart> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] { PermissionIdentifiers.DataMart.ManageSecurity };

            return from q in query join dm in db.Filter(db.DataMarts, identity, permissions) on q.DataMartID equals dm.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclDataMart[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.DataMart.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.DataMart.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.DataMart.ManageSecurity);
        }
    }

    public class AclDataMartMappingProfile : AutoMapper.Profile
    {
        public AclDataMartMappingProfile()
        {
            CreateMap<AclDataMart, DTO.AclDataMartDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission != null ? src.Permission!.Name : string.Empty))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup != null ? src.SecurityGroup!.Path : string.Empty));
        }
    }
}
