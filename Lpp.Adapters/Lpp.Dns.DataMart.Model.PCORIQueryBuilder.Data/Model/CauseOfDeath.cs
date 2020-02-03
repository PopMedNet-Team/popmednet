using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("DEATH_CAUSE")]
    public class CauseOfDeath : Lpp.Objects.Entity
    {
        [Key, Column("PATID", Order = 0)]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Key, Column("DEATH_CAUSE", Order = 1)]
        public string Code { get; set; }

        [Key, Column("DEATH_CAUSE_CODE", Order = 2)]
        public string CodeType { get; set; }

        [Key, Column("DEATH_CAUSE_TYPE", Order = 3)]
        public string Type { get; set; }

        [Key, Column("DEATH_CAUSE_SOURCE", Order = 4)]
        public string Source { get; set; }        

        [Column("DEATH_CAUSE_CONFIDENCE")]
        public string ConfidenceLevel { get; set; }
    }
}
