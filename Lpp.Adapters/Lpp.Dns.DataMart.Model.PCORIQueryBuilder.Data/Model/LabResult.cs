using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("LAB_RESULT_CM")]
    public class LabResult : Lpp.Objects.Entity
    {
        [Column("LAB_RESULT_CM_ID")]
        public string ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("ENCOUNTERID")]
        public string EncounterID { get; set; }
        public Encounter Encounter { get; set; }

        [Column("LAB_NAME")]
        public string LabName { get; set; }

        [Column("SPECIMEN_SOURCE")]
        public string SpecimenSource { get; set; }

        [Column("LAB_LOINC")]
        public string LogicalObservationINC { get; set; }

        [Column("PRIORITY")]
        public string Priority { get; set; }

        [Column("RESULT_LOC")]
        public string ResultLocation { get; set; }

        [Column("LAB_PX")]
        public string ProcedureCode { get; set; }

        [Column("LAB_PX_TYPE")]
        public string ProcedureCodeType { get; set; }

        [Column("LAB_ORDER_DATE")]
        public DateTime? OrderedOn { get; set; }

        [Column("SPECIMEN_DATE")]
        public DateTime? SpecimenCollectedOn { get; set; }
        /// <summary>
        /// The time of collection in format HH:mm, can be null.
        /// </summary>
        [Column("SPECIMEN_TIME")]
        public string SpecimenCollectedTime { get; set; }

        [Column("RESULT_DATE")]
        public DateTime? ResultDate { get; set; }
        /// <summary>
        /// The result time in format HH:mm, can be null.
        /// </summary>
        [Column("RESULT_TIME")]
        public string ResultTime { get; set; }

        [Column("RESULT_QUAL")]
        public string ResultQualitative { get; set; }

        [Column("RESULT_NUM")]
        public string ResultQuantitative { get; set; }

        [Column("RESULT_MODIFIER")]
        public string ResultModifier { get; set; }

        [Column("RESULT_UNIT")]
        public string ResultUnit { get; set; }

        [Column("NORM_RANGE_LOW")]
        public string NormalRangeLow { get; set; }

        [Column("NORM_MODIFIER_LOW")]
        public string ModifierLow { get; set; }

        [Column("NORM_RANGE_HIGH")]
        public string NormalRangeHigh { get; set; }

        [Column("NORM_MODIFIER_HIGH")]
        public string ModifierHigh { get; set; }

        [Column("ABN_IND")]
        public string AbnormalResultIndicator { get; set; }
    }
}
