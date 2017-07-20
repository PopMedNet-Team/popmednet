using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model
{
    [Table("ili_summary", Schema = "esp_mdphnet")]
    public class IliSummary
    {
        [Key, Column("age_group", Order = 0)]
        public string AgeGroup { get; set; }

        [Column("period_end")]
        public DateTime? PeriodEnd { get; set; }

        [Key, Column("week", Order = 1)]
        public string Week { get; set; }

        [Column("zip5")]
        public string Zip5 { get; set; }

        [Key, Column("center", Order = 2)]
        public string Center { get; set; }

        [Column("cdc_site_id")]
        public string CDCSiteID { get; set; }

        [Column("ili_counts")]
        public long IliCounts { get; set; }

        [Column("tot_counts")]
        public long TotCounts { get; set; }
    }
}
