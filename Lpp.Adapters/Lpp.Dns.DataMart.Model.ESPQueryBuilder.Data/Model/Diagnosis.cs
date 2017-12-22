using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model
{
    [Table("esp_diagnosis", Schema="esp_mdphnet")]
    public class Diagnosis
    {
        [Column("centerid")]
        public string CenterID { get; set; }

        [Key, Column("patid", Order = 0)]
        public string PatID { get; set; }

        [Key, Column("encounterid", Order = 1)]
        public string EncounterID { get; set; }

        [Column("a_date")]
        public int A_Date { get; set; }

        [Column("provider")]
        public string Provider { get; set; }

        [Column("enc_type")]
        public string EncType { get; set; }

        [Key, Column("dx", Order = 2)]
        public string Dx { get; set; }

        [Column("dx_code_3dig")]
        public string DxCode3digit { get; set; }

        [Column("dx_code_4dig")]
        public string DxCode4digit { get; set; }

        [Column("dx_code_4dig_with_dec")]
        public string DxCode4digitWithDec { get; set; }

        [Column("dx_code_5dig")]
        public string DxCode5digit { get; set; }

        [Column("dx_code_5dig_with_dec")]
        public string DxCode5digitWithDec { get; set; }

        [Column("facility_location")]
        public string FacilityLocation { get; set; }

        [Column("facility_code")]
        public string FacilityCode { get; set; }

        [Column("enc_year")]
        public int EncYear { get; set; }

        [Column("age_at_enc_year")]
        public int AgeAtEncYear { get; set; }

        [Column("age_group_5yr")]
        public string AgeGroup5yr { get; set; }

        [Column("age_group_10yr")]
        public string AgeGroup10yr { get; set; }

        [Column("age_group_ms")]
        public string AgeGroupMS { get; set; }
    }
}
