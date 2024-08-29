namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Lpp.Dns.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            CommandTimeout = Int32.MaxValue;            
        }

        protected override void Seed(Lpp.Dns.Data.DataContext context)
        {
        }
    }
}
