using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("CONDITION")]
    public class Condition : Lpp.Objects.Entity
    {
        [Key, Column("CONDITIONID")]
        public string ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("ENCOUNTERID")]
        public string EncounterID { get; set; }
        public virtual Encounter Encounter { get; set; }

        [Column("REPORT_DATE")]
        public DateTime? ReportedOn { get; set; }

        [Column("RESOLVE_DATE")]
        public DateTime? ResolvedOn { get; set; }

        [Column("ONSET_DATE")]
        public DateTime? OnSetDate { get; set; }

        [Column("CONDITION")]
        public string Code { get; set; }

        [Column("CONDITION_TYPE")]
        public string CodeType { get; set; }

        [Column("CONDITION_SOURCE")]
        public string Source { get; set; }

        [Column("CONDITION_STATUS")]
        public string Status { get; set; }
        
    }
}
