namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTVFS2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsRoutingStatusChange", "EventID", c => c.Guid(nullable: false, defaultValue: new Guid("5AB90001-8072-42CD-940F-A22200CC24A2")));
            CreateIndex("dbo.LogsRoutingStatusChange", "EventID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LogsRoutingStatusChange", new[] { "EventID" });
            DropColumn("dbo.LogsRoutingStatusChange", "EventID");
        }
    }
}
