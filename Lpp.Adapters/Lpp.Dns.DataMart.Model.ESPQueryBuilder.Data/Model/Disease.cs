using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model
{
    [Table("esp_disease", Schema = "esp_mdphnet")]
    public class Disease
    {
        [Column("centerid")]
        public string CenterID { get; set; }

        [Key, Column("patid", Order = 0)]
        public string PatID { get; set; }

        [Key, Column("condition", Order = 1)]
        public string Condition { get; set; }

        [Key, Column("date", Order = 2)]
        public int Date { get; set; }

        [Column("age_at_detect_year")]
        public int AgeAtDetectYear { get; set; }

        [Column("age_group_5yr")]
        public string AgeGroup5yr { get; set; }

        [Column("age_group_10yr")]
        public string AgeGroup10yr { get; set; }

        [Column("age_group_ms")]
        public string AgeGroupMS { get; set; }

        [Column("criteria")]
        public string Criteria { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("notes")]
        public string Notes { get; set; }
    }
}
