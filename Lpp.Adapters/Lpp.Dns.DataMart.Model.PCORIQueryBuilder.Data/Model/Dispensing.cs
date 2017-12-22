using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("DISPENSING")]
    public class Dispensing : Lpp.Objects.Entity
    {
        [Key, Column("DISPENSINGID")]
        public string ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("PRESCRIBINGID")]
        public string PrescriptionID { get; set; }
        //public Prescription Prescription { get; set; }

        [Column("NDC")]
        public string NationalDrugCode { get; set; }

        [Column("DISPENSE_DATE")]
        public DateTime? DispensedOn { get; set; }

        [Column("DISPENSE_SUP")]
        public double? DispensedSupply { get; set; }

        [Column("DISPENSE_AMT")]
        public double? DispensedUnits { get; set; }
    }
}
