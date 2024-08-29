using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclProjectOrganizations")]
    public class AclProjectOrganization : Acl
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
    }
    internal class AclProjectOrganizationConfiguration : IEntityTypeConfiguration<AclProjectOrganization>
    {
        public void Configure(EntityTypeBuilder<AclProjectOrganization> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.ProjectID, e.OrganizationID }).HasName("PK_dbo.AclProjectOrganizations");
        }
    }

    internal class AclProjectOrganizationSecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectOrganization>
    {
        public override IQueryable<AclProjectOrganization> SecureList(DataContext db, IQueryable<AclProjectOrganization> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from q in query join p in db.Filter(db.Projects, identity, permissions) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectOrganization[] objs)
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

    public class AclProjectOrganizationMappingProfile : AutoMapper.Profile
    {
        public AclProjectOrganizationMappingProfile()
        {
            CreateMap<AclProjectOrganization, DTO.AclProjectOrganizationDTO>()
                .ForMember(d => d.Permission, opt => opt.MapFrom(src => src.Permission!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
