using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("Demographics")]
    public class Demographic : Entity
    {
        [MaxLength(2)]
        public string Country { get; set; } = string.Empty;
        [MaxLength(2)]
        public string State { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Town { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Region { get; set; } = string.Empty;
        [MaxLength(1), MinLength(1)]
        public string Gender { get; set; } = string.Empty;
        public AgeGroups AgeGroup { get; set; }
        public Ethnicities Ethnicity { get; set; }
        [Column("Count", TypeName = "int")]
        public int Count { get; set; }
    }
    internal class DemographicConfiguration : IEntityTypeConfiguration<Demographic>
    {
        public void Configure(EntityTypeBuilder<Demographic> builder)
        {
            builder.HasKey(e => new { e.Country, e.State, e.Town, e.Region, e.Gender, e.AgeGroup, e.Ethnicity  }).HasName("PK_dbo.Demographics");
            builder.Property(e => e.AgeGroup).HasConversion<int>();
            builder.Property(e => e.Ethnicity).HasConversion<int>();

        }
    }
}
