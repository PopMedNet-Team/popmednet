using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder
{    
    public class DataContext : DbContext
    {
        public DbSet<Model.Patient> Patients { get; set; }

        public DbSet<Model.Diagnosis> Diagnoses { get; set; }

        public DbSet<Model.Encounter> Encounters { get; set; }

        public DbSet<Model.Enrollment> Enrollments { get; set; }

        public DbSet<Model.Procedure> Procedures { get; set; }

        public DbSet<Model.Vital> Vitals { get; set; }

        public DbSet<Model.Dispensing> Dispensings { get; set; }

        public DbSet<Model.LabResult> LabResultCommonMeasures { get; set; }

        public DbSet<Model.Condition> Conditions { get; set; }

        public DbSet<Model.ReportedOutcome> ReportedOutcomeCommonMeasures { get; set; }

        public DbSet<Model.Prescription> Prescriptions { get; set; }

        public DbSet<Model.ClinicalTrial> ClinicalTrials { get; set; }

        public DbSet<Model.Death> Deaths { get; set; }

        public DbSet<Model.CauseOfDeath> CauseOfDeaths { get; set; }

        readonly string DefaultSchema = "";

        static DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }

        public DataContext(System.Data.Common.DbConnection connection)
            : base(connection, true)
        {            
        }

        public DataContext(System.Data.Common.DbConnection connection, string defaultSchema)
            : base(connection, true)
        {
            DefaultSchema = defaultSchema;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (!string.IsNullOrEmpty(DefaultSchema))
            {
                modelBuilder.HasDefaultSchema(DefaultSchema);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
