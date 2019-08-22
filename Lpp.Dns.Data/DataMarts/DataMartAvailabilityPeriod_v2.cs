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
    [Table("DataMartAvailabilityPeriods_v2")]
    public class DataMartAvailabilityPeriod_v2
    {
        [Key, Column(Order = 0)]
        public Guid DataMartID { get; set; }
        public DataMart DataMart { get; set; }
        [Key, Column(Order = 1)]
        public string DataTable { get; set; }
        [Key, Column(Order = 2)]
        public string PeriodCategory { get; set; }
        [Key, Column(Order = 3)]
        public string Period { get; set; }
        public int Year { get; set; }
        public int? Quarter { get; set; }
    }

    internal class DataMartAvailabilityPeriod_v2Configuration : EntityTypeConfiguration<DataMartAvailabilityPeriod_v2>
    {
        public DataMartAvailabilityPeriod_v2Configuration() 
        {
            Property(x => x.DataMartID).IsRequired();
            Property(x => x.DataTable).HasMaxLength(80).IsRequired();
            Property(x => x.Period).HasMaxLength(10).IsRequired();
            Property(x => x.PeriodCategory).HasColumnType("char").HasMaxLength(1).IsRequired();
        }
    }
}
