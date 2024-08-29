namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Reflection;
    
    public partial class AddDemographicByZCTA_Data : DbMigration
    {
        public override void Up()
        {
            Sql("TRUNCATE TABLE DemographicsByZCTA");
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Lpp.Dns.Data.Migrations.SQLCommands.Census2010_Zip_Age_Sex_Race_Ethnicity.sql";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result;
                    while ((result = reader.ReadLine()) != null)
                        Sql(result, false);
                }
            }
        }
        
        public override void Down()
        {
            Sql("TRUNCATE TABLE DemographicsByZCTA");
        }
    }
}
