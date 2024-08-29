using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopMedNet.Dns.Data
{
    [Table("Networks")]
    public class Network : EntityWithID
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(450)]
        public string Url { get; set; }

        public virtual ICollection<WorkplanType> WorkplanTypes { get; set; }
        public virtual ICollection<RequesterCenter> RequesterCenters { get; set; }
    }

    internal class NetworkConfiguration : IEntityTypeConfiguration<Network>
    {
        public void Configure(EntityTypeBuilder<Network> builder)
        {
            builder.HasMany(t => t.WorkplanTypes)
                .WithOne(t => t.Network)
                .IsRequired(true)
                .HasForeignKey(t => t.NetworkID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RequesterCenters)
                .WithOne(t => t.Network)
                .IsRequired(true)
                .HasForeignKey(t => t.NetworkID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


