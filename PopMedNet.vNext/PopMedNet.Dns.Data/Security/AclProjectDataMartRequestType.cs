using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclProjectDataMartRequestTypes")]
    public class AclProjectDataMartRequestType : RequestTypeAcl
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }
        public virtual RequestType? RequestType { get; set; }
    }

    internal class AclProjectDataMartRequestTypeConfiguration : IEntityTypeConfiguration<AclProjectDataMartRequestType>
    {
        public void Configure(EntityTypeBuilder<AclProjectDataMartRequestType> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.RequestTypeID, e.Permission, e.ProjectID, e.DataMartID }).HasName("PK_dbo.AclProjectDataMartRequestTypes");
            builder.Property(e => e.Permission).HasConversion<int>();
        }
    }

    internal class AclProjectDataMartRequestTypeSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectDataMartRequestType>
    {
        public override IQueryable<AclProjectDataMartRequestType> SecureList(DataContext db, IQueryable<AclProjectDataMartRequestType> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectDataMartRequestType[] objs)
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

    public class AclProjectDataMartRequestTypeMappingProfile : AutoMapper.Profile
    {
        public AclProjectDataMartRequestTypeMappingProfile()
        {
            CreateMap<AclProjectDataMartRequestType, DTO.AclProjectDataMartRequestTypeDTO>()
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
