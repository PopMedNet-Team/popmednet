namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentChangedLogTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LogsTaskChangedLog", newName: "LogsTaskChange");
            RenameTable(name: "dbo.LogsRequestWorkflowActivityChanged", newName: "LogsRequestWorkflowActivityChange");
            CreateTable(
                "dbo.LogsDocumentChange",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        DocumentID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: true)
                .Index(t => t.DocumentID)
                .Index(t => t.EventID);

            Sql(@"ALTER TABLE dbo.LogsDocumentChange ADD CONSTRAINT PK_LogsDocumentChange PRIMARY KEY (UserID, TimeStamp, DocumentID) ON AuditLogs");            
            AddColumn("dbo.LogsRequestWorkflowActivityChange", "TaskID", c => c.Guid());
            CreateIndex("dbo.LogsRequestWorkflowActivityChange", "TaskID");
            AddForeignKey("dbo.LogsRequestWorkflowActivityChange", "TaskID", "dbo.Tasks", "ID");

            //fix pk for log tables
            DropPrimaryKey("dbo.LogsRequestWorkflowActivityChange");
            Sql(@"ALTER TABLE dbo.LogsRequestWorkflowActivityChange ADD CONSTRAINT PK_LogsRequestWorkflowActivityChange PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs");

            DropPrimaryKey("dbo.LogsTaskChange");
            Sql(@"ALTER TABLE dbo.LogsTaskChange ADD CONSTRAINT PK_LogsTaskChange PRIMARY KEY (UserID, TimeStamp, TaskID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsDocumentChange", "DocumentID", "dbo.Documents");
            DropForeignKey("dbo.LogsRequestWorkflowActivityChange", "TaskID", "dbo.Tasks");
            DropIndex("dbo.LogsDocumentChange", new[] { "EventID" });
            DropIndex("dbo.LogsDocumentChange", new[] { "DocumentID" });
            DropIndex("dbo.LogsRequestWorkflowActivityChange", new[] { "TaskID" });
            DropColumn("dbo.LogsRequestWorkflowActivityChange", "TaskID");
            DropTable("dbo.LogsDocumentChange");
            RenameTable(name: "dbo.LogsRequestWorkflowActivityChange", newName: "LogsRequestWorkflowActivityChanged");
            RenameTable(name: "dbo.LogsTaskChange", newName: "LogsTaskChangedLog");
        }
    }
}
