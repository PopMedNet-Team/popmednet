namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataCheckerToDataModel : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS(SELECT NULL FROM DataModels WHERE ID = '321ADAA1-A350-4DD0-93DE-5DE658A507DF' ) INSERT INTO DataModels (ID, Name, RequiresConfiguration, QueryComposer) VALUES ('321ADAA1-A350-4DD0-93DE-5DE658A507DF', 'Data Checker QE', 0, 1)");  
        }
        
        public override void Down()
        {
        }
    }
}
