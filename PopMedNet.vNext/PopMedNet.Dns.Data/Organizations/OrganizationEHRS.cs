using PopMedNet.Utilities.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Dns.DTO.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PopMedNet.Dns.Data
{
    [Table("OrganizationElectronicHealthRecordSystems")]
    public class OrganizationEHRS : EntityWithID
    {
        public Guid OrganizationID { get; set; }
        public virtual Organization? Organization { get; set; }

        public EHRSTypes Type { get; set; } = EHRSTypes.Inpatient;

        public EHRSSystems System { get; set; } = EHRSSystems.None;

        [MaxLength(80)]
        public string? Other { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }
    }

    internal class OrganizationEHRSConfiguration : IEntityTypeConfiguration<OrganizationEHRS>
    {
        public void Configure(EntityTypeBuilder<OrganizationEHRS> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.Type).HasConversion<int>();
            builder.Property(e => e.System).HasConversion<int>();

        }
    }


}
