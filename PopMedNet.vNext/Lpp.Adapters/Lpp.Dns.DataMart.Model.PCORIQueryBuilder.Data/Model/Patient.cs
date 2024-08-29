using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("DEMOGRAPHIC")]
    public class Patient : Lpp.Objects.Entity
    {
        public Patient()
        {
            Diagnoses = new HashSet<Diagnosis>();
            Encounters = new HashSet<Encounter>();
            Enrollments = new HashSet<Enrollment>();
            Procedures = new HashSet<Procedure>();
            Vitals = new HashSet<Vital>();
            Dispensary = new HashSet<Dispensing>();
            LabResults = new HashSet<LabResult>();
            Conditions = new HashSet<Condition>();
            ReportedOutcomes = new HashSet<ReportedOutcome>();
            Prescriptions = new HashSet<Prescription>();
            ClinicalTrials = new HashSet<ClinicalTrial>();
            Deaths = new HashSet<Death>();
            CausesOfDeath = new HashSet<CauseOfDeath>();
            ClinicalObservations = new HashSet<ClinicalObservation>();
        }

        [Key, Column("PATID")]
        public string ID { get; set; }

        [Column("BIRTH_DATE")]
        public DateTime? BornOn { get; set; }

        /// <summary>
        /// The time of birth in format HH:mm, can be null.
        /// </summary>
        [Column("BIRTH_TIME")]
        public string BornTime { get; set; }

        [Column("SEX")]
        public string Sex { get; set; }

        [Column("HISPANIC")]
        public string Hispanic { get; set; }

        [Column("RACE")]
        public string Race { get; set; }

        [Column("BIOBANK_FLAG")]
        public string BiobankFlag { get; set; }

        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<Encounter> Encounters { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
        public virtual ICollection<Vital> Vitals { get; set; }
        public virtual ICollection<Dispensing> Dispensary { get; set; }
        public virtual ICollection<LabResult> LabResults { get; set; }
        public virtual ICollection<Condition> Conditions { get; set; }
        public virtual ICollection<ReportedOutcome> ReportedOutcomes { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<ClinicalTrial> ClinicalTrials { get; set; }
        public virtual ICollection<Death> Deaths { get; set; }
        public virtual ICollection<CauseOfDeath> CausesOfDeath { get; set; }
        public virtual ICollection<ClinicalObservation> ClinicalObservations { get; set; }
    }

    internal class PatientConfiguration : EntityTypeConfiguration<Patient>
    {
        public PatientConfiguration()
        {
            HasMany(t => t.Diagnoses).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Encounters).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Enrollments).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Procedures).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Vitals).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Dispensary).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.LabResults).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Conditions).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.ReportedOutcomes).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Prescriptions).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.ClinicalTrials).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.Deaths).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.CausesOfDeath).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
            HasMany(t => t.ClinicalObservations).WithRequired(t => t.Patient).HasForeignKey(t => t.PatientID).WillCascadeOnDelete(true);
        }
    }
}
