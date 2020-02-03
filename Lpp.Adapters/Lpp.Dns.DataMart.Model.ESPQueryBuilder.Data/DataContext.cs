using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder
{
    public class DataContext : DbContext
    {
        public DbSet<Model.Demographic> Demographics { get; set; }

        public DbSet<Model.Diagnosis> Diagnosis { get; set; }

        public DbSet<Model.DiagnosisICD9_3digit> DiagnosisICD9_3digit { get; set; }

        public DbSet<Model.DiagnosisICD9_4digit> DiagnosisICD9_4digit { get; set; }

        public DbSet<Model.DiagnosisICD9_5digit> DiagnosisICD9_5digit { get; set; }

        public DbSet<Model.Disease> Diseases { get; set; }

        public DbSet<Model.Encounter> Encounters { get; set; }

        public DbSet<Model.IliSummary> IliSummaries { get; set; }

        public DbSet<Model.UVT_AgeGroup10yr> UVT_AgeGroup10yr { get; set; }

        public DbSet<Model.UVT_AgeGroup5yr> UVT_AgeGroup5yr { get; set; }

        public DbSet<Model.UVT_AgeGroupMS> UVT_AgeGroupMS { get; set; }

        public DbSet<Model.UVT_Center> UVT_Center { get; set; }

        public DbSet<Model.UVT_DetectedCondition> UVT_DetectedCondition { get; set; }

        public DbSet<Model.UVT_DetectedCriteria> UVT_DetectedCriteria { get; set; }

        public DbSet<Model.UVT_DetectedStatus> UVT_DetectedStatus { get; set; }

        public DbSet<Model.UVT_Dx> UVT_Dx { get; set; }

        public DbSet<Model.UVT_Dx3Digit> UVT_Dx3Digit { get; set; }

        public DbSet<Model.UVT_Dx4Digit> UVT_Dx4Digit { get; set; }

        public DbSet<Model.UVT_Dx5Digit> UVT_Dx5Digit { get; set; }

        public DbSet<Model.UVT_Encounter> UVT_Encounter { get; set; }

        public DbSet<Model.UVT_Period> UVT_Period { get; set; }

        public DbSet<Model.UVT_Provider> UVT_Provider { get; set; }

        public DbSet<Model.UVT_Race> UVT_Race { get; set; }

        public DbSet<Model.UVT_Sex> UVT_Sex { get; set; }

        public DbSet<Model.UVT_Site> UVT_Site { get; set; }

        public DbSet<Model.UVT_Zip5> UVT_Zip5 { get; set; }

        public DbSet<Model.UVT_Race_Ethnicity> UVT_Race_Ethnicity { get; set; }

        static DataContext()
        {
            Database.SetInitializer<DataContext>(null);
        }

        public DataContext(System.Data.Common.DbConnection connection)
            : base(connection, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public static DataContext Create(string connectionString)
        {
            Npgsql.NpgsqlConnection conn = new Npgsql.NpgsqlConnection(connectionString);
            conn.Open();
            return new DataContext(conn);
        }
    }
}
