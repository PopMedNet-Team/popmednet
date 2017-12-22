namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewSubmittedRequestLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsNewRequestSubmitted",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID);

            Sql(@"ALTER TABLE dbo.LogsNewRequestSubmitted ADD CONSTRAINT PK_LogsNewRequestSubmitted PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs");
            
            CreateTable(
                "dbo.LogsSubmittedRequestAwaitsResponse",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID);

            Sql(@"ALTER TABLE dbo.LogsSubmittedRequestAwaitsResponse ADD CONSTRAINT PK_LogsSubmittedRequestAwaitsResponse PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsSubmittedRequestAwaitsResponse", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.LogsNewRequestSubmitted", "RequestID", "dbo.Requests");
            DropIndex("dbo.LogsSubmittedRequestAwaitsResponse", new[] { "RequestID" });
            DropIndex("dbo.LogsNewRequestSubmitted", new[] { "RequestID" });
            DropTable("dbo.LogsSubmittedRequestAwaitsResponse");
            DropTable("dbo.LogsNewRequestSubmitted");
        }
    }
}
