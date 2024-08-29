namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMyNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "SupportsMyNotifications", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.UserEventSubscriptions", "FrequencyForMy", c => c.Int(nullable: true));

            Sql(@"UPDATE Events SET SupportsMyNotifications = 1 WHERE ID = '45DA0001-7E63-4578-9A19-A43B0100F7C8' OR ID = 'E7160001-D933-476E-A706-A43C0137D4E9' OR ID = 'F9C20001-E0C2-4996-B5CC-A3BF01301150' OR ID = '0A850001-FC8A-4DE2-9AA5-A22200E82398' OR ID = '5AB90001-8072-42CD-940F-A22200CC24A2'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserEventSubscriptions", "FrequencyForMy");
            DropColumn("dbo.Events", "SupportsMyNotifications");
        }
    }
}
