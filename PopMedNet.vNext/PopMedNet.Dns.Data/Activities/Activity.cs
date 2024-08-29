using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Objects;

namespace PopMedNet.Dns.Data
{
    [Table("Activities")]
    public class Activity : EntityWithID
    {
        public Activity()
        {
        }

        public int? ExternalKey { get; set; }
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? Acronym { get; set; }
        public string? Description { get; set; }
        public Guid? ProjectID { get; set; }
        public virtual Project? Project { get; set; }
        public Guid? ParentActivityID { get; set; }
        public virtual Activity? ParentActivity { get; set; }
        public int DisplayOrder { get; set; }
        public int TaskLevel { get; set; }
        public bool Deleted { get; set; } = false;

        public virtual ICollection<Activity> DependantActivities { get; set; } = new HashSet<Activity>();

        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
    }

    internal class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasMany(t => t.DependantActivities).WithOne(t => t.ParentActivity).IsRequired(false).HasForeignKey(t => t.ParentActivityID).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(t => t.Requests).WithOne(t => t.Activity).IsRequired(false).HasForeignKey(t => t.ActivityID).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

}
