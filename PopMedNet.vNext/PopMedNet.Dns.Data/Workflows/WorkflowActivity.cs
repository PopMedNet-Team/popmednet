using PopMedNet.Utilities.Objects;
using PopMedNet.Workflow.Engine.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("WorkflowActivities")]
    public class WorkflowActivity : EntityWithID, IDbWorkflowActivity
    {
        [MaxLength(255), Required]
        public string Name { get; set; }
        [MaxLength]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets if the activity is a starting point for a workflow.
        /// </summary>
        public bool Start { get; set; }
        /// <summary>
        /// Gets or sets if the activity is a termination point for a workflow.
        /// </summary>
        public bool End { get; set; }

        public virtual ICollection<WorkflowActivityCompletionMap> SourceMap { get; set; } = new HashSet<WorkflowActivityCompletionMap>();
        public virtual ICollection<WorkflowActivityCompletionMap> DestinationMap { get; set; } = new HashSet<WorkflowActivityCompletionMap>();
        public virtual ICollection<WorkflowActivitySecurityGroup> SecurityGroups { get; set; } = new HashSet<WorkflowActivitySecurityGroup>();
        public virtual ICollection<AclProjectRequestTypeWorkflowActivity> ProjectRequestTypeWorkflowActivityAcls { get; set; } = new HashSet<AclProjectRequestTypeWorkflowActivity>();
        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
        public virtual ICollection<Response> Responses { get; set; } = new HashSet<Response>();
    }

    internal class WorkflowActivityConfiguration : IEntityTypeConfiguration<WorkflowActivity>
    {
        public void Configure(EntityTypeBuilder<WorkflowActivity> builder)
        {
            builder.HasIndex(i => i.Name).IsClustered(false).IsUnique(false);

            builder.HasMany(t => t.SourceMap)
                .WithOne(t => t.SourceWorkflowActivity)
                .IsRequired(true)
                .HasForeignKey(t => t.SourceWorkflowActivityID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.DestinationMap)
                .WithOne(t => t.DestinationWorkflowActivity)
                .IsRequired(true)
                .HasForeignKey(t => t.DestinationWorkflowActivityID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.SecurityGroups)
                .WithOne(t => t.WorkflowActivity)
                .IsRequired(true)
                .HasForeignKey(t => t.WorkflowActivityID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.ProjectRequestTypeWorkflowActivityAcls)
                .WithOne(t => t.WorkflowActivity)
                .IsRequired(true)
                .HasForeignKey(t => t.WorkflowActivityID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Requests)
                .WithOne(t => t.WorkflowActivity)
                .IsRequired(false)
                .HasForeignKey(t => t.WorkFlowActivityID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.Responses)
                .WithOne(t => t.WorkFlowActivity)
                .IsRequired(false)
                .HasForeignKey(t => t.WorkFlowActivityID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class WorkflowActivityMappingProfile : AutoMapper.Profile
    {
        public WorkflowActivityMappingProfile()
        {
            CreateMap<WorkflowActivity, DTO.WorkflowActivityDTO>().ForMember(d => d.Timestamp, opt => opt.Ignore());
        }
    }
}
