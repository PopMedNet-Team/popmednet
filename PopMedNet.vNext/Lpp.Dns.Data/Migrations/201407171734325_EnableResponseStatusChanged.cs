namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableResponseStatusChanged : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsRoutingStatusChange",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestDataMartID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.RequestDataMarts", t => t.RequestDataMartID, cascadeDelete: true)
                .Index(t => t.RequestDataMartID);

            Sql(@"ALTER TABLE dbo.LogsRoutingStatusChange ADD CONSTRAINT PK_LogsRoutingStatusChange PRIMARY KEY (UserID, TimeStamp, RequestDataMartID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRoutingStatusChange", "RequestDataMartID", "dbo.RequestDataMarts");
            DropIndex("dbo.LogsRoutingStatusChange", new[] { "RequestDataMartID" });
            DropTable("dbo.LogsRoutingStatusChange");
        }
    }
}
