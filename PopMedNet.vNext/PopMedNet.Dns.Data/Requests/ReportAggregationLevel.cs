using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("ReportAggregationLevels")]
    public class ReportAggregationLevel : EntityWithID
    {
        public Guid NetworkID { get; set; }

        public virtual Network? Network { get; set; }

        [MaxLength(80), Column(TypeName = "nvarchar"), Required]
        public string Name { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
    }

    internal class ReportAggregationLevelConfiguration : IEntityTypeConfiguration<ReportAggregationLevel>
    {
        public void Configure(EntityTypeBuilder<ReportAggregationLevel> builder)
        {
            builder.HasMany(t => t.Requests)
                .WithOne(t => t.ReportAggregationLevel)
                .IsRequired(false)
                .HasForeignKey(t => t.ReportAggregationLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
