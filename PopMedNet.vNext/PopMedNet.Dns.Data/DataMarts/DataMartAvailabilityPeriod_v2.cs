using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("DataMartAvailabilityPeriods_v2")]
    public class DataMartAvailabilityPeriod_v2
    {
        public Guid DataMartID { get; set; }
        public DataMart? DataMart { get; set; }
        public string DataTable { get; set; }
        public string PeriodCategory { get; set; }
        public string Period { get; set; }
        public int Year { get; set; }
        public int? Quarter { get; set; }
    }

    internal class DataMartAvailabilityPeriod_v2Configuration : IEntityTypeConfiguration<DataMartAvailabilityPeriod_v2>
    {
        public void Configure(EntityTypeBuilder<DataMartAvailabilityPeriod_v2> builder)
        {
            builder.HasKey(e => new { e.DataMartID, e.DataTable, e.PeriodCategory, e.Period }).HasName("PK_dbo.DataMartAvailabilityPeriods_v2").IsClustered(true);
            builder.Property(x => x.DataMartID).IsRequired();
            builder.Property(x => x.DataTable).HasMaxLength(80).IsRequired();
            builder.Property(x => x.Period).HasMaxLength(10).IsRequired();
            builder.Property(x => x.PeriodCategory).HasColumnType("char").HasMaxLength(1).IsRequired();
        }
    }
}
