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

        [Column("PRO_ITEM")]
        public string Identifier { get; set; }

        [Column("PRO_DATE")]
        public DateTime RespondedOn { get; set; }

        [Column("PRO_RESPONSE")]
        public double ResponseNumber { get; set; }

        [Column("PRO_LOINC")]
        public string LogicalObservationINC { get; set; }

        [Column("PRO_TIME")]
        public string RespondedTime { get; set; }

        [Column("PRO_METHOD")]
        public string MethodOfAdministration { get; set; }

        [Column("PRO_MODE")]
        public string Mode { get; set; }

        [Column("PRO_CAT")]
        public string ComputerAdaptiveTesting { get; set; }
    }
}
