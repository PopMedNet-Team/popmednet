using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Objects;
using PopMedNet.Dns.DTO;
using PopMedNet.Utilities.Security;
using PopMedNet.Dns.DTO.Security;
using System.Linq.Expressions;

namespace PopMedNet.Dns.Data
{
    [Table("Projects")]
    public class Project : EntityWithID, ISupportsSoftDelete, IEntityWithName, IEntityWithDeleted
    {
        [MaxLength(255), Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Acronym { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Column("isDeleted")]
        public bool Deleted { get; set; }

        [Column("isActive")]
        public bool Active { get; set; }

        public string? Description { get; set; }

        public Guid? GroupID { get; set; }
        public virtual Group? Group { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
        public virtual ICollection<ProjectDataMart> DataMarts { get; set; } = new HashSet<ProjectDataMart>();
        public virtual ICollection<Activity> Activities { get; set; } = new HashSet<Activity>();
        public virtual ICollection<ProjectOrganization> Organizations { get; set; } = new HashSet<ProjectOrganization>();
        public virtual ICollection<AclProject> ProjectAcls { get; set; } = new HashSet<AclProject>();
        public virtual ICollection<AclProjectDataMart> ProjectDataMartAcls { get; set; } = new HashSet<AclProjectDataMart>();
        public virtual ICollection<ProjectEvent> ProjectEventAcls { get; set; } = new HashSet<ProjectEvent>();
        public virtual ICollection<AclProjectDataMartRequestType> ProjectDataMartRequestTypeAcls { get; set; } = new HashSet<AclProjectDataMartRequestType>();
        public virtual ICollection<AclProjectRequestType> ProjectRequestTypeAcls { get; set; } = new HashSet<AclProjectRequestType>();
        public virtual ICollection<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivityAcls { get; set; } = new HashSet<AclProjectRequestTypeWorkflowActivity>();
        public virtual ICollection<Audit.ProjectChangeLog> ChangeLogs { get; set; } = new HashSet<Audit.ProjectChangeLog>();
        public virtual ICollection<ProjectRequestType> RequestTypes { get; set; } = new HashSet<ProjectRequestType>();
    }

    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasMany(t => t.Requests).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DataMarts).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Activities).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectAcls).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.ProjectDataMartAcls).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.ProjectEventAcls).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectDataMartRequestTypeAcls).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.Organizations).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.ProjectRequestTypeAcls).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.ChangeLogs).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.RequestTypes).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(t => t.ProjectRequestTypeWorkflowActivityAcls).WithOne(t => t.Project).IsRequired(true).HasForeignKey(t => t.ProjectID).OnDelete(DeleteBehavior.Cascade);
        }
    }

    internal class ProjectSecurityConfiguration : DnsEntitySecurityConfiguration<Project>
    {
        public override IQueryable<Project> SecureList(DataContext db, IQueryable<Project> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            if (permissions == null || permissions.Length == 0)
                permissions = new PermissionDefinition[] {
                    PermissionIdentifiers.Project.View
                };

            return db.Filter(query, identity, permissions);
        }

        public override async Task<bool> CanInsert(DataContext db, ApiIdentity identity, params Project[] objs)
        {
            if (objs.Any(p => p.GroupID == null))
                return false;

            var groupIDs = objs.Select(p => p.GroupID.Value).ToArray();

            var aclGroups = db.GroupAcls.FilterAcl(identity, PermissionIdentifiers.Group.CreateProjects).Where(g => groupIDs.Contains(g.GroupID));
            var aclGlobal = db.GlobalAcls.FilterAcl(identity, PermissionIdentifiers.Group.CreateProjects);

            return (aclGlobal.Any() && aclGlobal.All(a => a.Allowed)) ||
                (aclGroups.Any() && aclGroups.All(a => a.Allowed));
        }

        public override Task<bool> CanDelete(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.Delete);
        }

        public override Task<bool> CanUpdate(DataContext db, ApiIdentity identity, params Guid[] keys)
        {
            return HasPermissions(db, identity, keys, PermissionIdentifiers.Project.Edit);
        }

        public override Expression<Func<AclProject, bool>> ProjectFilter(params Guid[] objIDs)
        {
            return p => objIDs.Contains(p.ProjectID);
        }

        public override Expression<Func<AclDataMart, bool>> DataMartFilter(params Guid[] objIDs)
        {
            return dm => dm.DataMart.Projects.Any(p => objIDs.Contains(p.ProjectID));
        }

        public override Expression<Func<AclGroup, bool>> GroupFilter(params Guid[] objIDs)
        {
            return g => g.Group.Projects.Any(p => objIDs.Contains(p.ID));
        }

        public override Expression<Func<AclProjectDataMart, bool>> ProjectDataMartFilter(params Guid[] objIDs)
        {
            return pdm => objIDs.Contains(pdm.ProjectID);
        }

        public override Expression<Func<AclOrganization, bool>> OrganizationFilter(params Guid[] objIDs)
        {
            return o => o.Organization.Projects.Any(p => objIDs.Contains(p.ProjectID));
        }

        public override Expression<Func<AclOrganizationDataMart, bool>> OrganizationDataMartFilter(params Guid[] objIDs)
        {
            return odm => odm.DataMart.Projects.Any(p => objIDs.Contains(p.ProjectID)) || odm.Organization.Projects.Any(p => objIDs.Contains(p.ProjectID));
        }
    }

    public class ProjectMappingProfile : AutoMapper.Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, DTO.ProjectDTO>()
                .ForMember(d => d.Group, opt => opt.MapFrom(src => src.GroupID.HasValue ? src.Group!.Name : null))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(src => src.EndDate));

            CreateMap<DateTime?, DateTimeOffset?>().ConvertUsing(d => d!.Value);

            CreateMap<DTO.ProjectDTO, Project>()
                .ForMember(d => d.EndDate, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeOffsetToNullableDateTimeConverter, DateTimeOffset?>(dto => dto.EndDate))
                .ForMember(d => d.StartDate, opt => opt.ConvertUsing<AutoMapperHelpers.NullableDateTimeOffsetToNullableDateTimeConverter, DateTimeOffset?>(dto => dto.StartDate))
                .ForMember(d => d.Timestamp, opt => opt.Ignore());            
        }
    }
}


