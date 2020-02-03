using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("ENROLLMENT")]
    public class Enrollment : Lpp.Objects.Entity
    {
        [Key, Column("PATID", Order=0)]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Key, Column("ENR_START_DATE", Order=1)]
        public DateTime? StartedOn { get; set; }

        [Key, Column("ENR_BASIS", Order=2)]
        public string EncrollmentBasis { get; set; }

        [Column("ENR_END_DATE")]
        public DateTime? EndedOn { get; set; }

        [Column("CHART")]
        public string ChartAbstrationFlag { get; set; }
    }
}
