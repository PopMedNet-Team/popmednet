using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("WorkplanTypes")]
    public class WorkplanType : EntityWithID
    {
        public WorkplanType()
        {
        }

        public int WorkplanTypeID { get; set; }

        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? Acronym { get; set; }

        public Guid NetworkID { get; set; }
        public virtual Network? Network { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();
    }

    internal class WorkplanTypeConfiguration : IEntityTypeConfiguration<WorkplanType>
    {
        public void Configure(EntityTypeBuilder<WorkplanType> builder)
        {
            builder.HasMany(t => t.Requests)
                .WithOne(t => t.WorkplanType)
                .IsRequired(false)
                .HasForeignKey(t => t.WorkplanTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
