using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
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
        [Key, MaxLength(5), Column(Order = 1), Lpp.Objects.ValidationAttributes.Required]
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the Sex: 'M' for male, 'F' for female.
        /// </summary>
        [Key, Column("Sex", Order = 2), MaxLength(1), Lpp.Objects.ValidationAttributes.Required]
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets the age grouping.
        /// </summary>
        [Key, Column(Order = 3)]
        public int AgeGroup { get; set; }

        /// <summary>
        /// Gets or sets the Race-Ethnicity. PCORNet value set is used.
        /// 0 = Unknown, 1 = American Indian or Alaska Native, 2 = Asian, 3 = Black, 5 = White, 6 = Hispanic
        /// </summary>
        [Key, Column(Order = 4)]
        public int Ethnicity { get; set; }

        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        [Column("Count", TypeName="int")]
        public int Count { get; set; }
    }

    internal class ZCTADemographicConfiguration : EntityTypeConfiguration<ZCTADemographic>
    {
        public ZCTADemographicConfiguration()
        {
        }
    }
}
