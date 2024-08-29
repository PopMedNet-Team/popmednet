using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("DEATH")]
    public class Death : Lpp.Objects.Entity
    {
        [Key, Column("PATID", Order = 0)]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("DEATH_DATE")]
        public DateTime? DateOfDeath { get; set; }

        [Key, Column("DEATH_SOURCE", Order = 1)]
        public string Source { get; set; }

        [Column("DEATH_DATE_IMPUTE")]
        public string Imputed { get; set; }

        [Column("DEATH_MATCH_CONFIDENCE")]
        public string ConfidenceLevel { get; set; }
    }
}
