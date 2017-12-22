namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingSiteWideDataMartChanged : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES('59A90001-539E-4C21-A4F2-A22200CD3C7D', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
