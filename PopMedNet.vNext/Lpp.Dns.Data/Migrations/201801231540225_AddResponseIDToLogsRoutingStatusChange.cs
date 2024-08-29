namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResponseIDToLogsRoutingStatusChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsRoutingStatusChange", "ResponseID", c => c.Guid());
            CreateIndex("dbo.LogsRoutingStatusChange", "ResponseID");
            AddForeignKey("dbo.LogsRoutingStatusChange", "ResponseID", "dbo.RequestDataMartResponses", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRoutingStatusChange", "ResponseID", "dbo.RequestDataMartResponses");
            DropIndex("dbo.LogsRoutingStatusChange", new[] { "ResponseID" });
            DropColumn("dbo.LogsRoutingStatusChange", "ResponseID");
        }
    }
}
