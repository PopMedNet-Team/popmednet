using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("GeographicLocations")]
    public class GeographicLocation : Entity
    {
        [MaxLength(50), PopMedNet.Objects.ValidationAttributes.Required]
        public string State { get; set; }

        [MaxLength(2), PopMedNet.Objects.ValidationAttributes.Required]
        public string StateAbbrev { get; set; }

        [MaxLength(100), PopMedNet.Objects.ValidationAttributes.Required]
        public string Region { get; set; }

        [MaxLength(80), PopMedNet.Objects.ValidationAttributes.Required]
        public string Location { get; set; }

        [MaxLength(8), PopMedNet.Objects.ValidationAttributes.Required]
        public string PostalCode { get; set; }
    }

    internal class GeographicLocationConfiguration : IEntityTypeConfiguration<GeographicLocation>
    {
        public void Configure(EntityTypeBuilder<GeographicLocation> builder)
        {
            builder.HasKey(e => new { e.StateAbbrev, e.Location, e.PostalCode }).HasName("PK_dbo.GeographicLocations").IsClustered(true);
            builder.HasIndex(g => g.State,"IX_State").IsClustered(false).IsUnique(false);
        }
    }
}
