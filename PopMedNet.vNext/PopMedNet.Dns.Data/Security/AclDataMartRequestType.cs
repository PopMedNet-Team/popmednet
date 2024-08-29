using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclDataMartRequestTypes")]
    public class AclDataMartRequestType : RequestTypeAcl
    {
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }
        public virtual RequestType? RequestType { get; set; }
    }

    internal class AclDataMartRequestTypeConfiguration : IEntityTypeConfiguration<AclDataMartRequestType>
    {
        public void Configure(EntityTypeBuilder<AclDataMartRequestType> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.RequestTypeID, e.Permission, e.DataMartID }).HasName("PK_dbo.AclDataMartRequestTypes");
            builder.Property(e => e.Permission).HasConversion<int>();
        }
    }

    internal class AclDataMartRequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<AclDataMartRequestType>
    {
        public override IQueryable<AclDataMartRequestType> SecureList(DataContext db, IQueryable<AclDataMartRequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.DataMart.ManageSecurity
                };

            return from q in query join p in db.Filter(db.DataMarts, identity, permissions) on q.DataMartID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclDataMartRequestType[] objs)
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

    public class AclDataMartRequestTypeMappingProfile : AutoMapper.Profile
    {
        public AclDataMartRequestTypeMappingProfile()
        {
            CreateMap<AclDataMartRequestType, DTO.AclDataMartRequestTypeDTO>()
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup != null ? src.SecurityGroup!.Path : null));
        }
    }


}
