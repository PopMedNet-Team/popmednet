using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("ProjectEvents")]
    public class ProjectEvent : BaseEventPermission
    {
        public ProjectEvent() { }

        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Event? Event { get; set; }
    }
    internal class ProjectEventConfiguration : IEntityTypeConfiguration<ProjectEvent>
    {
        public void Configure(EntityTypeBuilder<ProjectEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.ProjectID, e.EventID }).HasName("PK_dbo.ProjectEvents");
        }
    }

    internal class ProjectEventSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectEvent>
    {
        public override IQueryable<ProjectEvent> SecureList(DataContext db, IQueryable<ProjectEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity
                };

            return from e in query join p in db.Filter(db.Projects, identity, permissions) on e.ProjectID equals p.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Events does not have direct permissions for delete, check it's parent project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project Events does not have direct permissions for update, check it's parent project");
        }
    }

    public class ProjectEventMappingProfile : AutoMapper.Profile
    {
        public ProjectEventMappingProfile()
        {
            CreateMap<ProjectEvent, DTO.ProjectEventDTO>()
                .ForMember(d => d.Event, opt => opt.MapFrom(src => src.Event!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
