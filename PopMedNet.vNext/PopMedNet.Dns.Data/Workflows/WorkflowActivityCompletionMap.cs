using PopMedNet.Objects;
using PopMedNet.Workflow.Engine.Database;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("WorkflowActivityCompletionMaps")]
    public class WorkflowActivityCompletionMap : Entity, IDbWorkflowActivityCompletionMap
    {
        public Guid WorkflowID { get; set; }
        public Guid WorkflowActivityResultID { get; set; }
        public virtual WorkflowActivityResult? WorkflowActivityResult { get; set; }
        public Guid SourceWorkflowActivityID { get; set; }
        public virtual WorkflowActivity? SourceWorkflowActivity { get; set; }
        public Guid DestinationWorkflowActivityID { get; set; }
        public virtual WorkflowActivity? DestinationWorkflowActivity { get; set; }
    }
    internal class WorkflowActivityCompletionMapConfiguration : IEntityTypeConfiguration<WorkflowActivityCompletionMap>
    {
        public void Configure(EntityTypeBuilder<WorkflowActivityCompletionMap> builder)
        {
            builder.HasKey(e => new { e.WorkflowID, e.WorkflowActivityResultID, e.SourceWorkflowActivityID, e.DestinationWorkflowActivityID }).HasName("PK_dbo.WorkflowActivityCompletionMaps");
        }
    }
}
