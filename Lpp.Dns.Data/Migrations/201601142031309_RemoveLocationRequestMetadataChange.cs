namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLocationRequestMetadataChange : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM EventLocations WHERE EventID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1' AND Location = 11");

        }
        
        public override void Down()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1' AND Location = 11) INSERT INTO EventLocations (EventID, Location) VALUES ('29AEE006-1C2A-4304-B3C9-8771D96ACDF1', 11)");
        }
    }
}
