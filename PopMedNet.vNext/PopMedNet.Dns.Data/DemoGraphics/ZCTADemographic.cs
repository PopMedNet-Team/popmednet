using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    /// <summary>
    /// Represents census data for a specific ZIP Code Tabulation Area.
    /// </summary>
    [Table("DemographicsByZCTA")]
    public class ZCTADemographic
    {
        /// <summary>
        /// Gets or set the Zip code.
        /// </summary>
        /// <remarks>This should not be equated to a postal zip code, the geographic region encompassed may be different.</remarks>
        [MaxLength(5), PopMedNet.Objects.ValidationAttributes.Required]
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the Sex: 'M' for male, 'F' for female.
        /// </summary>
        [Column("Sex"), MaxLength(1), PopMedNet.Objects.ValidationAttributes.Required]
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets the age grouping.
        /// </summary>
        public int AgeGroup { get; set; }

        /// <summary>
        /// Gets or sets the Race-Ethnicity. PCORNet value set is used.
        /// 0 = Unknown, 1 = American Indian or Alaska Native, 2 = Asian, 3 = Black, 5 = White, 6 = Hispanic
        /// </summary>
        public int Ethnicity { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        [Column("Count", TypeName = "int")]
        public int Count { get; set; }
    }
    internal class ZCTADemographicConfiguration : IEntityTypeConfiguration<ZCTADemographic>
    {
        public void Configure(EntityTypeBuilder<ZCTADemographic> builder)
        {
            builder.HasKey(e => new { e.Zip, e.Sex, e.AgeGroup, e.Ethnicity }).HasName("PK_dbo.DemographicsByZCTA").IsClustered(true);
        }
    }
}
