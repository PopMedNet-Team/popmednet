using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("DIAGNOSIS")]
    public class Diagnosis : Lpp.Objects.Entity
    {
        [Key, Column("DIAGNOSISID")]
        public string ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("ENCOUNTERID")]
        public string EncounterID { get; set; }
        public virtual Encounter Encounter { get; set; }

        [Column("ENC_TYPE")]
        public string EncounterType { get; set; }

        [Column("PROVIDERID")]
        public string ProviderID { get; set; }

        [Column("ADMIT_DATE")]
        public DateTime? AdmittedOn { get; set; }

        [Column("DX")]
        public string Code { get; set; }

        [Column("DX_TYPE")]
        public string CodeType { get; set; }

        [Column("DX_SOURCE")]
        public string Source { get; set; }

        [Column("PDX")]
        public string DischargeDiagnosisFlag { get; set; }

    }
}
