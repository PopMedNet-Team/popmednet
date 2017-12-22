namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaptureRoutingStatusChangeValues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsRoutingStatusChange", "OldStatus", c => c.Int(nullable: true));
            AddColumn("dbo.LogsRoutingStatusChange", "NewStatus", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogsRoutingStatusChange", "NewStatus");
            DropColumn("dbo.LogsRoutingStatusChange", "OldStatus");
        }
    }
}
