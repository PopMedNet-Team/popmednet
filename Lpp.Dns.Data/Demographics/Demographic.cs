using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Lpp.Dns.DTO;
using Lpp.Objects;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Data
{
    [Table("Demographics")]
    public class Demographic : Entity
    {
        [Key, MaxLength(2), Column(Order=1)]
        public string Country { get; set; }
        [Key, MaxLength(2), MinLength(2), Column(Order = 2)]
        public string State { get; set; }
        [Key, MaxLength(50), Column(Order = 3)]
        public string Town { get; set; }
        [Key, MaxLength(50), Column(Order = 4)]
        public string Region { get; set; }
        [Key, Column(Order = 5), MaxLength(1), MinLength(1)]
        public string Gender { get; set; }
        [Key, Column(Order=6)]
        public AgeGroups AgeGroup { get; set; }
        [Key, Column(Order = 7)]
        public Ethnicities Ethnicity { get; set; }
        [Column("Count", TypeName = "int")]
        public int Count { get; set; }
    }

    internal class DemographicConfiguration : EntityTypeConfiguration<Demographic>
    {
        public DemographicConfiguration()
        {
        }
    }
}
