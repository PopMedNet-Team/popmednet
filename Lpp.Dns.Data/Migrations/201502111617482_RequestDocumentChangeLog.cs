namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestDocumentChangeLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsRequestDocumentChange",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        DocumentID = c.Guid(nullable: false),
                        RequestID = c.Guid(nullable: false),
                        TaskID = c.Guid(),
                        Reason = c.Int(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskID)
                .Index(t => t.DocumentID)
                .Index(t => t.RequestID)
                .Index(t => t.TaskID)
                .Index(t => t.EventID);

            Sql(@"ALTER TABLE dbo.LogsRequestDocumentChange ADD CONSTRAINT PK_LogsRequestDocumentChange PRIMARY KEY (UserID, TimeStamp, DocumentID, RequestID) ON AuditLogs"); 
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestDocumentChange", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsRequestDocumentChange", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.LogsRequestDocumentChange", "DocumentID", "dbo.Documents");
            DropIndex("dbo.LogsRequestDocumentChange", new[] { "EventID" });
            DropIndex("dbo.LogsRequestDocumentChange", new[] { "TaskID" });
            DropIndex("dbo.LogsRequestDocumentChange", new[] { "RequestID" });
            DropIndex("dbo.LogsRequestDocumentChange", new[] { "DocumentID" });
            DropTable("dbo.LogsRequestDocumentChange");
        }
    }
}
