using PopMedNet.Utilities.Objects;
using PopMedNet.Workflow.Engine.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("WorkflowActivityResults")]
    public class WorkflowActivityResult : EntityWithID, IDbWorkflowActivityResult
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [MaxLength]
        public string Description { get; set; } = string.Empty;
        [MaxLength]
        public string Uri { get; set; }

        public virtual ICollection<WorkflowActivityCompletionMap> Maps { get; set; } = new HashSet<WorkflowActivityCompletionMap>();
    }

    internal class WorkflowActivityResultConfiguration : IEntityTypeConfiguration<WorkflowActivityResult>
    {
        public void Configure(EntityTypeBuilder<WorkflowActivityResult> builder)
        {
            builder.HasMany(t => t.Maps)
                .WithOne(t => t.WorkflowActivityResult)
                .IsRequired(true)
                .HasForeignKey(t => t.WorkflowActivityResultID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
