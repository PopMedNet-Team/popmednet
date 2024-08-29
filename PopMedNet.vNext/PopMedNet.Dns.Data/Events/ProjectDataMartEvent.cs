using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("ProjectDataMartEvents")]
    public class ProjectDataMartEvent : BaseEventPermission
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }

        public virtual Event? Event { get; set; }
    }
    internal class ProjectDataMartEventConfiguration : IEntityTypeConfiguration<ProjectDataMartEvent>
    {
        public void Configure(EntityTypeBuilder<ProjectDataMartEvent> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.ProjectID, e.DataMartID, e.EventID }).HasName("PK_dbo.ProjectDataMartEvents");
        }
    }

    internal class ProjectDataMartEventSecurityConfiguration : DnsEntitySecurityConfiguration<ProjectDataMartEvent>
    {
        public override IQueryable<ProjectDataMartEvent> SecureList(DataContext db, IQueryable<ProjectDataMartEvent> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.ManageSecurity,
                    PermissionIdentifiers.DataMart.ManageSecurity
                };

            return from e in query join p in db.Filter(db.Projects, identity, permissions) on e.ProjectID equals p.ID join dm in db.Filter(db.DataMarts, identity, permissions) on e.DataMartID equals dm.ID select e;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params ProjectDataMartEvent[] objs)
        {
            return HasPermissions(db, identity, objs.Select(o => o.ProjectID).ToArray(), PermissionIdentifiers.Project.ManageSecurity);
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart Events does not have direct permissions for delete, check it's parent Project");
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            throw new NotImplementedException("Project DataMart Events does not have direct permissions for update, check it's parent Project");
        }
    }

    public class ProjectDataMartEventMappingProfile : AutoMapper.Profile
    {
        public ProjectDataMartEventMappingProfile()
        {
            CreateMap<ProjectDataMartEvent, DTO.ProjectDataMartEventDTO>()
                .ForMember(d => d.Event, opt => opt.MapFrom(src => src.Event!.Name))
                .ForMember(d => d.SecurityGroup, opt => opt.MapFrom(src => src.SecurityGroup!.Path));
        }
    }
}
