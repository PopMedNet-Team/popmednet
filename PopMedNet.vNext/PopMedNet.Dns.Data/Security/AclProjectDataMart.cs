using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclProjectDataMarts")]
    public class AclProjectDataMart : Acl
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }
    }
    internal class AclProjectDataMartConfiguration : IEntityTypeConfiguration<AclProjectDataMart>
    {
        public void Configure(EntityTypeBuilder<AclProjectDataMart> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.ProjectID, e.DataMartID }).HasName("PK_dbo.AclProjectDataMarts");
        }
    }

    internal class AclProjectDataMartSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectDataMart>
    {
        public override IQueryable<AclProjectDataMart> SecureList(DataContext db, IQueryable<AclProjectDataMart> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectDataMart[] objs)
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

    public class AclProjectDataMartMappingProfile : AutoMapper.Profile
    {
        public AclProjectDataMartMappingProfile()
        {
            CreateMap<AclProjectDataMart, DTO.AclProjectDataMartDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt=> opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
