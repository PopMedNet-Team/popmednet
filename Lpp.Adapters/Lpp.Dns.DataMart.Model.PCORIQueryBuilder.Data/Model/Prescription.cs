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

        [Column("RX_DOSE_ORDERED")]
        public float? DoseOrdered { get; set; }

        [Column("RX_DOSE_ORDERED_UNIT")]
        public string DoseOrderedUnit { get; set; }

        [Column("RX_QUANTITY")]
        public float? QuantityOrdered { get; set; }

        [Column("RX_DOSE_FORM")]
        public string DoseForm { get; set; }

        [Column("RX_REFILLS")]
        public float? RefillsOrdered { get; set; }

        [Column("RX_DAYS_SUPPLY")]
        public float? DaysSupplyOrdered { get; set; }

        [Column("RX_FREQUENCY")]
        public string Frequency { get; set; }

        [Column("RX_PRN_FLAG"), MaxLength(1)]
        public string PRN_Flag { get; set; }

        [Column("RX_ROUTE")]
        public string RouteOfMedicationDelivery { get; set; }

        [Column("RX_BASIS")]
        public string Basis { get; set; }

        [Column("RXNORM_CUI")]
        public string RXNORMConceptIdentifier { get; set; }

        [Column("RX_SOURCE")]
        public string Source { get; set; }

        [Column("RX_DISPENSE_AS_WRITTEN"), MaxLength(2)]
        public string DispenseAsWritten { get; set; }
    }
}
