using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("ENCOUNTER")]
    public class Encounter : Lpp.Objects.Entity
    {
        public Encounter()
        {
            Diagnoses = new HashSet<Diagnosis>();
            Procedures = new HashSet<Procedure>();
            Vitals = new HashSet<Vital>();
            Prescriptions = new HashSet<Prescription>();
            LabResults = new HashSet<LabResult>();
            Conditions = new HashSet<Condition>();
            ReportedOutcomes = new HashSet<ReportedOutcome>();
        }

        [Key, Column("ENCOUNTERID")]
        public string ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }        

        [Column("ADMIT_DATE")]
        public DateTime AdmittedOn { get; set; }

        /// <summary>
        /// The admittance time in format HH:mm, can be null.
        /// </summary>
        [Column("ADMIT_TIME")]
        public string AdmittedTime { get; set; }

        [Column("DISCHARGE_DATE")]
        public DateTime? DischargedOn { get; set; }
        /// <summary>
        /// The admittance time in format HH:mm, can be null.
        /// </summary>
        [Column("DISCHARGE_TIME")]
        public string DischargedTime { get; set; }

        [Column("PROVIDERID")]
        public string ProviderID { get; set; }

        [Column("FACILITY_LOCATION")]
        public string FacilityLocation { get; set; }

        [Column("ENC_TYPE")]
        public string EncounterType { get; set; }

        [Column("FACILITYID")]
        public string FacilityID { get; set; }

        [Column("DISCHARGE_DISPOSITION")]
        public string DischargeDisposition { get; set; }

        [Column("DISCHARGE_STATUS")]
        public string DischargeStatus { get; set; }

        [Column("DRG")]
        public string DiagnosisRelatedGroup { get; set; }

        [Column("DRG_TYPE")]
        public string DiagnosisRelatedGroupType { get; set; }

        [Column("ADMITTING_SOURCE")]
        public string AdmittingSource { get; set; }

        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
        public virtual ICollection<Vital> Vitals { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<LabResult> LabResults { get; set; }
        public virtual ICollection<Condition> Conditions { get; set; }
        public virtual ICollection<ReportedOutcome> ReportedOutcomes { get; set; }
    }

    internal class EncounterConfiguration : EntityTypeConfiguration<Encounter>
    {
        public EncounterConfiguration()
        {
            HasMany(t => t.Diagnoses).WithRequired(t => t.Encounter).HasForeignKey(t => t.EncounterID).WillCascadeOnDelete(false);
            HasMany(t => t.Procedures).WithRequired(t => t.Encounter).HasForeignKey(t => t.EncounterID).WillCascadeOnDelete(false);
            HasMany(t => t.Vitals).WithRequired(t => t.Encounter).HasForeignKey(t => t.EncounterID).WillCascadeOnDelete(false);
            HasMany(t => t.LabResults).WithOptional(t => t.Encounter).HasForeignKey(t => t.EncounterID).WillCascadeOnDelete(false);
            HasMany(t => t.Conditions).WithOptional(t => t.Encounter).HasForeignKey(t => t.EncounterID).WillCascadeOnDelete(false);
            HasMany(t => t.ReportedOutcomes).WithOptional(t => t.Encounter).HasForeignKey(t => t.EncounterID).WillCascadeOnDelete(false);
            HasMany(t => t.Prescriptions).WithOptional(t => t.Encounter).HasForeignKey(t => t.EncounterID).WillCascadeOnDelete(false);
        }
    }
}
