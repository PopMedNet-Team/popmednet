using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclProjectRequestTypes")]
    public class AclProjectRequestType : RequestTypeAcl
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public virtual RequestType? RequestType { get; set; }
    }
    internal class AclProjectRequestTypeConfiguration : IEntityTypeConfiguration<AclProjectRequestType>
    {
        public void Configure(EntityTypeBuilder<AclProjectRequestType> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.RequestTypeID, e.Permission, e.ProjectID }).HasName("PK_dbo.AclProjectRequestTypes");
            builder.Property(e => e.Permission).HasConversion<int>();
        }
    }

    internal class AclProjectRequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectRequestType>
    {
        public override IQueryable<AclProjectRequestType> SecureList(DataContext db, IQueryable<AclProjectRequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectRequestType[] objs)
        {
            return HasPermissions(db, identity, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.ManageSecurity);
        }
    }

    public class AclProjectRequestTypeMappingProfile : AutoMapper.Profile
    {
        public AclProjectRequestTypeMappingProfile()
        {
            CreateMap<AclProjectRequestType, DTO.AclProjectRequestTypeDTO>()
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
