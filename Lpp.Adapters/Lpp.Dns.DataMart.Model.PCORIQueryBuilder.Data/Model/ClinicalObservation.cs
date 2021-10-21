using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("OBS_CLIN")]
    public class ClinicalObservation : Lpp.Objects.Entity
    {
        [Key, Column("OBSCLINID")]
        public string ID { get; set; }
        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("ENCOUNTERID")]
        public string EncounterID { get; set; }
        [Column("OBSCLIN_PROVIDERID")]
        public string ProviderID { get; set; }
        [Column("OBSCLIN_START_DATE")]
        public DateTime? StartDate { get; set; }
        [Column("OBSCLIN_START_TIME")]
        public string StartTime { get; set; }
        [Column("OBSCLIN_STOP_DATE")]
        public DateTime? StopDate { get; set; }
        [Column("OBSCLIN_STOP_TIME")]
        public string StopTime { get; set; }
        [Column("OBSCLIN_TYPE")]
        public string Type { get; set; }
        [Column("OBSCLIN_CODE")]
        public string Code { get; set; }
        [Column("OBSCLIN_RESULT_QUAL")]
        public string QualitativeResult { get; set; }
        [Column("OBSCLIN_RESULT_TEXT")]
        public string TextResult { get; set; }
        [Column("OBSCLIN_RESULT_NUM")]
        public double? QuantitativeResult { get; set; }
        [Column("OBSCLIN_RESULT_MODIFIER")]
        public string ResultModifier { get; set; }
        [Column("OBSCLIN_RESULT_UNIT")]
        public string ResultUnit { get; set; }
        [Column("OBSCLIN_SOURCE"), MaxLength(2)]
        public string Source { get; set; }
        [Column("OBSCLIN_ABN_IND"), MaxLength(2)]
        public string AbnormalResultIndicator { get; set; }
    }
}
