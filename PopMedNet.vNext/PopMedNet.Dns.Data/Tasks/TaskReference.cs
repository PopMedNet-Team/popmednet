using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("TaskReferences")]
    public class TaskReference : Entity
    {
        public Guid TaskID { get; set; }
        public virtual PmnTask? Task { get; set; }
        public Guid ItemID { get; set; }
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public string Item { get; set; } = string.Empty;
        public TaskItemTypes Type { get; set; }
    }
    internal class TaskReferenceConfiguration : IEntityTypeConfiguration<TaskReference>
    {
        public void Configure(EntityTypeBuilder<TaskReference> builder)
        {
            builder.HasKey(e => new { e.TaskID, e.ItemID }).HasName("PK_dbo.TaskReferences");
            builder.Property(e => e.Type).HasConversion<int>();
        }
    }
}
