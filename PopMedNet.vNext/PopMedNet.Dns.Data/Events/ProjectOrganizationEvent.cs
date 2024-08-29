using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("ProjectOrganizationEvents")]
    public class ProjectOrganizationEvent : BaseEventPermission
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }
        public virtual Event? Event { get; set; }
    }
    internal class ProjectOrganizationEventConfiguration : IEntityTypeConfiguration<ProjectOrganizationEvent>
    {
        public void Configure(EntityTypeBuilder<ProjectOrganizationEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.ProjectID, e.OrganizationID, e.EventID }).HasName("PK_dbo.ProjectOrganizationEvents");
        }
    }

    internal class ProjectOrganizationEventSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectOrganizationEvent>
    {
        public override IQueryable<ProjectOrganizationEvent> SecureList(DataContext db, IQueryable<ProjectOrganizationEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity,
                    PermissionIdentifiers.Organization.ManageSecurity
                };

            return from e in query join p in db.Filter(db.Projects, identity, permissions) on e.ProjectID equals p.ID join o in db.Filter(db.Organizations, identity, permissions) on e.OrganizationID equals o.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectOrganizationEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization Events does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Organization Events does not have direct permissions for update, check it's parent project");
        }
    }

    public class ProjectOrganizationEventMappingProfile : AutoMapper.Profile
    {
        public ProjectOrganizationEventMappingProfile()
        {
            CreateMap<ProjectOrganizationEvent, DTO.ProjectOrganizationEventDTO>()
                .ForMember(d => d.Event, opt => opt.MapFrom(src => src.Event!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
