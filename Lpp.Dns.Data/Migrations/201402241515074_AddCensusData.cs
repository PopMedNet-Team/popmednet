namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Reflection;
    
    public partial class AddCensusData : DbMigration
    {
        public override void Up()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Lpp.Dns.Data.Migrations.SQLCommands.census.sql";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result;
                    while ((result = reader.ReadLine()) != null)
                        Sql(result, true);
                }
            }
        }
        
        public override void Down()
        {
            Sql("TRUNCATE TABLE Demographics");
        }
    }
}
