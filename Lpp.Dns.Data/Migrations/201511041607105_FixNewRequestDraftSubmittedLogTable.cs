namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNewRequestDraftSubmittedLogTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("LogsNewRequestDraftSubmitted");
            AddColumn("dbo.LogsNewRequestDraftSubmitted", "RequestID", c => c.Guid(nullable: false));
            AddForeignKey("dbo.LogsNewRequestDraftSubmitted", "RequestID", "dbo.Requests", "ID");

            Sql(@"ALTER TABLE dbo.LogsNewRequestDraftSubmitted ADD CONSTRAINT PK_LogsNewRequestDraftSubmitted PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs");

        }
        
        public override void Down()
        {
        }
    }
}
