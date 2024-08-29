using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;

namespace PopMedNet.Dns.Data
{
    [Table("AclProjectRequestTypeWorkflowActivities")]
    public class AclProjectRequestTypeWorkflowActivity : Acl
    {
        public Guid ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }
        public Guid WorkflowActivityID { get; set; }
        public virtual WorkflowActivity? WorkflowActivity { get; set; }
    }
    internal class AclProjectRequestTypeWorkflowActivityConfiguration : IEntityTypeConfiguration<AclProjectRequestTypeWorkflowActivity>
    {
        public void Configure(EntityTypeBuilder<AclProjectRequestTypeWorkflowActivity> builder)
        {
            builder.HasKey(e => new { e.SecurityGroupID, e.PermissionID, e.ProjectID, e.RequestTypeID, e.WorkflowActivityID }).HasName("PK_dbo.AclProjectRequestTypeWorkflowActivities");
        }
    }

    internal class AclProjectRequestTypeWorkflowActivitySecurityConfiguration : DnsEntitySecurityConfiguration<AclProjectRequestTypeWorkflowActivity>
    {
        public override IQueryable<AclProjectRequestTypeWorkflowActivity> SecureList(DataContext db, IQueryable<AclProjectRequestTypeWorkflowActivity> query, ApiIdentity identity, params PermissionDefinition[] permissions)
        {
            return from q in query join p in db.Filter<Project>(db.Projects, identity, PermissionIdentifiers.Project.ManageSecurity) on q.ProjectID equals p.ID select q;
        }

        public override Task<bool> CanInsert(DataContext db, ApiIdentity identity, params AclProjectRequestTypeWorkflowActivity[] objs)
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

    public class AclProjectRequestTypeWorkflowActivityMappingProfile : AutoMapper.Profile
    {
        public AclProjectRequestTypeWorkflowActivityMappingProfile()
        {
            CreateMap<AclProjectRequestTypeWorkflowActivity, DTO.Security.AclProjectRequestTypeWorkflowActivityDTO>()
                .ForMember(d => d.Project, opt => opt.MapFrom(src => src.Project!.Name))
                .ForMember(d => d.RequestType, opt => opt.MapFrom(src => src.RequestType!.Name))
                .ForMember(d => d.WorkflowActivity, opt => opt.MapFrom(src => src.WorkflowActivity!.Name));
        }
    }
}
