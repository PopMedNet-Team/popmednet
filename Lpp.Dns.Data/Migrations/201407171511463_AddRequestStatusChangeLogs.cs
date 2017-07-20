namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestStatusChangeLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsRequestStatusChange",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestID = c.Guid(nullable: false),
                        OldStatus = c.Int(nullable: false),
                        NewStatus = c.Int(nullable: false),
                        Description = c.String(),
                    })                
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID);

            Sql(@"ALTER TABLE dbo.LogsRequestStatusChange ADD CONSTRAINT PK_LogsRequestStatusChange PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestStatusChange", "RequestID", "dbo.Requests");
            DropIndex("dbo.LogsRequestStatusChange", new[] { "RequestID" });
            DropTable("dbo.LogsRequestStatusChange");
        }
    }
}
