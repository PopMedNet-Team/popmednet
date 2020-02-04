using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("PRO_CM")]
    public class ReportedOutcome : Lpp.Objects.Entity
    {
        [Key, Column("PRO_CM_ID")]
        public string ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("ENCOUNTERID")]
        public string EncounterID { get; set; }
        public Encounter Encounter { get; set; }

        [Column("PRO_DATE")]
        public DateTime RespondedOn { get; set; }

        [Column("PRO_TIME")]
        public string RespondedTime { get; set; }

        [Column("PRO_TYPE")]
        public string ProcedureType { get; set; }

        [Column("PRO_ITEM_NAME")]
        public string ItemName { get; set; }

        [Column("PRO_ITEM_LOINC")]
        public string LogicalObservationINC { get; set; }

        [Column("PRO_RESPONSE_TEXT")]
        public string ResponseText { get; set; }

        [Column("PRO_RESPONSE_NUM")]
        public double? ResponseNumber { get; set; }

        [Column("PRO_METHOD")]
        public string MethodOfAdministration { get; set; }

        [Column("PRO_MODE")]
        public string Mode { get; set; }

        [Column("PRO_CAT")]
        public string ComputerAdaptiveTesting { get; set; }

        [Column("PRO_ITEM_VERSION")]
        public string ItemVersion { get; set; }

        [Column("PRO_MEASURE_NAME")]
        public string MeasureName { get; set; }

        [Column("PRO_MEASURE_SEQ")]
        public string MeasureSequence { get; set; }

        [Column("PRO_MEASURE_SCORE")]
        public double? MeasureScore { get; set; }

        [Column("PRO_MEASURE_THETA")]
        public double? MeasureTheta { get; set; }

        [Column("PRO_MEASURE_SCALED _TSCORE")]
        public double? MeasureScaledTScore { get; set; }

        [Column("PRO_MEASURE_STAND ARD_ERROR")]
        public double? MeasureStandardError { get; set; }

        [Column("PRO_MEASURE_COUNT _SCORED")]
        public double? MeasureCountScored { get; set; }

        [Column("PRO_MEASURE_LOINC")]
        public string MeasureLOINC { get; set; }

        [Column("PRO_MEASURE_VERSION")]
        public string MeasureVersion { get; set; }

        [Column("PRO_ITEM_FULLNAME")]
        public string ItemFullName { get; set; }

        [Column("PRO_ITEM_TEXT")]
        public string ItemText { get; set; }

        [Column("PRO_MEASURE_FULLNAME")]
        public string MeasureFullName { get; set; }
    }
}
