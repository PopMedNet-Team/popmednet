using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PopMedNet.Dns.Data
{
    [Table("DataMartAvailabilityPeriods")]
    public partial class DataMartAvailabilityPeriod : Entity
    {
        public DataMartAvailabilityPeriod()
        {
        }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Guid DataMartID { get; set; }
        public virtual DataMart? DataMart { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Guid RequestTypeID { get; set; }
        public virtual RequestType? RequestType { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(TypeName = "char"), MaxLength(1)]
        public string PeriodCategory { get; set; }

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None), Column(TypeName = "varchar"), MaxLength(10)]
        public string Period { get; set; }

        [Column("isActive")]
        public bool Active { get; set; }
    }
    internal class DataMartAvailabilityPeriodConfiguration : IEntityTypeConfiguration<DataMartAvailabilityPeriod>
    {
        public void Configure(EntityTypeBuilder<DataMartAvailabilityPeriod> builder)
        {
            builder.HasKey(e => new { e.DataMartID, e.RequestTypeID, e.PeriodCategory, e.Period }).HasName("PK_dbo.DataMartAvailabilityPeriods");
        }
    }
}
