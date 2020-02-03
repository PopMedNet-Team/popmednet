using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker
{
    public class DataContext : DbContext
    {
        public DbSet<Model.Diagnosis> Diagnoses { get; set; }

        public DbSet<Model.Hispanic> Hispanics { get; set; }

        public DbSet<Model.Metadata> Metadatas { get; set; }

        public DbSet<Model.NDC> NDCs { get; set; }

        public DbSet<Model.PDX> PDXs { get; set; }

        public DbSet<Model.Procedure> Procedures { get; set; }

        public DbSet<Model.Race> Races { get; set; }

        public DbSet<Model.RXAmt> RXAmts { get; set; }

        public DbSet<Model.RXSUP> RXSups { get; set; }

        public DbSet<Model.Age> Ages { get; set; }

        public DbSet<Model.Height> Heights { get; set; }

        public DbSet<Model.Sex> Sexes { get; set; }

        public DbSet<Model.Weight> Weights { get; set; }

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

        public static DataContext Create(string connectionString)
        {
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString);
            conn.Open();
            return new DataContext(conn);
        }
    }
}
