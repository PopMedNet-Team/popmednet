namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMATownFromDemographics : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Demographics WHERE Town = 'Massachusetts'");
        }
        
        public override void Down()
        {
        }
    }
}
