using Lpp.Objects;
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
    [Table("GeographicLocations")]
    public class GeographicLocation : Entity
    {
        [MaxLength(50), Index("IX_State"), Lpp.Objects.ValidationAttributes.Required]
        public string State { get; set; }

        [Key, MaxLength(2), Column(Order = 1), Lpp.Objects.ValidationAttributes.Required]
        public string StateAbbrev { get; set; }

        [MaxLength(100), Lpp.Objects.ValidationAttributes.Required]
        public string Region { get; set; }

        [Key, MaxLength(80), Column(Order = 2), Lpp.Objects.ValidationAttributes.Required]
        public string Location { get; set; }

        [Key, MaxLength(8), Column(Order = 3), Lpp.Objects.ValidationAttributes.Required]
        public string PostalCode { get; set; }
    }

    internal class GeographicLocationConfiguration : EntityTypeConfiguration<GeographicLocation>
    {
        public GeographicLocationConfiguration()
        {
        }
    }
}
