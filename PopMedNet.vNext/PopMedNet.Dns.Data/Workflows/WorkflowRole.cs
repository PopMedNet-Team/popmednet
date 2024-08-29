using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("WorkflowRoles")]
    public class WorkflowRole : EntityWithID
    {

        public WorkflowRole() : base()
        {
        }

        public Guid WorkflowID { get; set; }
        public virtual Workflow? Workflow { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public bool IsRequestCreator { get; set; } = false;
        public virtual ICollection<RequestUser> RequestUsers { get; set; } = new HashSet<RequestUser>();
    }

    internal class WorkflowRoleConfiguration : IEntityTypeConfiguration<WorkflowRole>
    {
        public void Configure(EntityTypeBuilder<WorkflowRole> builder)
        {
            builder.HasMany(t => t.RequestUsers)
                .WithOne(t => t.WorkflowRole)
                .IsRequired(true)
                .HasForeignKey(t => t.WorkflowRoleID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
