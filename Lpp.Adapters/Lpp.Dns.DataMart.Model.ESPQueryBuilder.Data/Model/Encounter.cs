using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model
{
    [Table("esp_encounter", Schema = "esp_mdphnet")]
    public class Encounter
    {
        [Column("centerid")]
        public string CenterID { get; set; }

        [Column("patid")]
        public string PatID { get; set; }

        [Key, Column("encounterid")]
        public string EncounterID { get; set; }

        [Column("a_date")]
        public int A_Date { get; set; }

        [Column("d_date")]
        public int D_Date { get; set; }

        [Column("provider")]
        public string Provider { get; set; }

        [Column("facility_location")]
        public string FacilityLocation { get; set; }

        [Column("enc_type")]
        public string EncType { get; set; }

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
