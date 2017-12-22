namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixLocationItems2 : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('553FD350-8F3B-40C6-9E31-11D8BC7420A2', 5)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B7640001-7247-49B8-A818-A22200CCEAF7', 9)");
        }
        
        public override void Down()
        {
        }
    }
}
