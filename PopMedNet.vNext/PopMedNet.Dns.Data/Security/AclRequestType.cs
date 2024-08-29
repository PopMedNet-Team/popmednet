using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclRequestTypes")]
    public class AclRequestType : Acl
    {
        public AclRequestType() { }

        public Guid RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }
    }
    internal class AclRequestTypeConfiguration : IEntityTypeConfiguration<AclRequestType>
    {
        public void Configure(EntityTypeBuilder<AclRequestType> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.RequestTypeID }).HasName("PK_dbo.AclRequestTypes");
        }

        internal class AclRequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<AclRequestType>
        {
            public override IQueryable<AclRequestType> SecureList(DataContext db, IQueryable<AclRequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
            {
                if (permissions == null || permissions.Length == 0)
                    permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.RequestTypes.ManageSecurity, PermissionIdentifiers.RequestTypes.View
                };

                return from q in query join r in db.Filter(db.RequestTypes, identity, permissions) on q.RequestTypeID equals r.ID select q;
            }

            public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclRequestType[] objs)
            {
                return HasPermissions(db, identity, PermissionIdentifiers.RequestTypes.ManageSecurity);
            }

            public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
            {
                return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestTypes.ManageSecurity);
            }

            public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
            {
                return HasPermissions(db, identity, keys, PermissionIdentifiers.RequestTypes.ManageSecurity);
            }
        }
    }

    internal class AclRequestTypeMappingProfile : AutoMapper.Profile
    {
        public AclRequestTypeMappingProfile()
        {
            CreateMap<AclRequestType, DTO.AclRequestTypeDTO>()
                .ForMember(t => t.RequestType, opt => opt.MapFrom(src => src.RequestType!.Name))
                .ForMember(t => t.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(t => t.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));

            CreateMap<DTO.AclRequestTypeDTO, AclRequestType>()
                .ForMember(t => t.RequestType, opt => opt.Ignore())
                .ForMember(t => t.Permission, opt => opt.Ignore())
                .ForMember(t => t.SecurityGroup, opt => opt.Ignore());
        }
    }
}
