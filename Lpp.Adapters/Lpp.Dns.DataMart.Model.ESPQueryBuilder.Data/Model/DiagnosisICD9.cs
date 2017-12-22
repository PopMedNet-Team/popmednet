using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model
{
    public abstract class DiagnosisICD9
    {
        [Key, Column("centerid", Order=1)]
        public string CenterID { get; set; }

        [Key, Column("age_group_type", Order = 2)]
        public string AgeGroupType { get; set; }

        [Key, Column("age_group", Order = 3)]
        public string AgeGroup { get; set; }

        [Key, Column("sex", Order = 4)]
        public char Sex { get; set; }

        [Key, Column("period", Order = 5)]
        public int Period { get; set; }

        [Key, Column("code_",Order = 6)]
        public string Code { get; set; }

        [Column("setting")]
        public string Setting { get; set; }

        [Column("members")]
        public long Members { get; set; }

        [Column("events")]
        public long Events { get; set; }

        [Key, Column("dx_name", Order = 7)]
        public string DxName { get; set; }
    }

    [Table("esp_diagnosis_icd9_3dig", Schema = "esp_mdphnet")]
    public class DiagnosisICD9_3digit : DiagnosisICD9
    {
    }

    [Table("esp_diagnosis_icd9_4dig", Schema = "esp_mdphnet")]
    public class DiagnosisICD9_4digit : DiagnosisICD9
    {
    }

    [Table("esp_diagnosis_icd9_5dig", Schema = "esp_mdphnet")]
    public class DiagnosisICD9_5digit : DiagnosisICD9
    {
    }
}
