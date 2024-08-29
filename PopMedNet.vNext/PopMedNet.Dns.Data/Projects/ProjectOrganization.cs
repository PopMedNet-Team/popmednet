using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Objects;

namespace PopMedNet.Dns.Data
{
    [Table("ProjectOrganizations")]
    public class ProjectOrganization : Entity
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
    }
    internal class ProjectOrganizationConfiguration : IEntityTypeConfiguration<ProjectOrganization>
    {
        public void Configure(EntityTypeBuilder<ProjectOrganization> builder)
        {
            builder.HasKey(e => new { e.ProjectID, e.OrganizationID }).HasName("PK_dbo.ProjectOrganizations");
        }
    }

    internal class ProjectOrganizationSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectOrganization>
    {
        public override IQueryable<ProjectOrganization> SecureList(DataContext db, IQueryable<ProjectOrganization> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.View
                };

            return from pdm in query join p in db.Filter(db.Projects, identity, permissions) on pdm.ProjectID equals p.ID select pdm;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectOrganization[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.Edit);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization does not have direct permissions for update, check it's parent project");
        }
    }

    public class ProjectOrganizationMappingProfile : AutoMapper.Profile
    {
        public ProjectOrganizationMappingProfile()
        {
            CreateMap<ProjectOrganization, DTO.ProjectOrganizationDTO>()
                .ForMember(d => d.Organization, opt => opt.MapFrom(src => src.Organization!.Name))
                .ForMember(d => d.Project, opt => opt.MapFrom(src => src.Project!.Name));
        }
    }
}
