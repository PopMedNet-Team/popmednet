using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("PRESCRIBING")]
    public class Prescription : Lpp.Objects.Entity
    {
        public Prescription()
        {
            //Dispensings = new HashSet<Dispensing>();
        }

        [Key, Column("PRESCRIBINGID")]
        public Guid ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("ENCOUNTERID")]
        public string EncounterID { get; set; }
        public virtual Encounter Encounter { get; set; }

        [Column("RX_PROVIDERID")]
        public string ProviderCode { get; set; }

        [Column("RX_ORDER_DATE")]
        public DateTime? OrderedOn { get; set; }

        [Column("RX_START_DATE")]
        public DateTime? StartedOn { get; set; }

        [Column("RX_END_DATE")]
        public DateTime? EndedOn { get; set; }

        [Column("RX_QUANTITY")]
        public double? QuantityOrdered { get; set; }

        [Column("RX_REFILLS")]
        public double? RefillsOrdered { get; set; }

        [Column("RX_DAYS_SUPPLY")]
        public double? DaysSupplyOrdered { get; set; }

        [Column("RX_FREQUENCY")]
        public string Frequency { get; set; }

        [Column("RX_BASIS")]
        public string Basis { get; set; }

        [Column("RXNORM_CUI")]
        public double? RXNORMConceptIdentifier { get; set; }

        //public virtual ICollection<Dispensing> Dispensings { get; set; }
    }

    internal class PrescriptionConfiguration : EntityTypeConfiguration<Prescription>
    {
        public PrescriptionConfiguration()
        {
            //HasMany(p => p.Dispensings).WithOptional(d => d.Prescription).HasForeignKey(d => d.PrescriptionID).WillCascadeOnDelete(false);
        }
    }
}
