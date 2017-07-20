namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogTablesForTaskSupport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsTaskChangedLog",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        TaskID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.UserID, t.TimeStamp, t.TaskID })
                .ForeignKey("dbo.Tasks", t => t.TaskID, cascadeDelete: true)
                .Index(t => t.TaskID)
                .Index(t => t.EventID);
            
            CreateTable(
                "dbo.LogsRequestWorkflowActivityChanged",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestID = c.Guid(nullable: false),
                        OriginalWorkflowActivityID = c.Guid(),
                        NewWorkflowActivityID = c.Guid(),
                        Reason = c.Int(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.UserID, t.TimeStamp, t.RequestID })
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID)
                .Index(t => t.EventID);
            
            AddColumn("dbo.LogsRoutingStatusChange", "TaskID", c => c.Guid());
            AddColumn("dbo.LogsUploadedResultNeedsApproval", "TaskID", c => c.Guid());
            AddColumn("dbo.LogsNewRequestSubmitted", "TaskID", c => c.Guid());
            AddColumn("dbo.LogsRequestStatusChange", "TaskID", c => c.Guid());
            AddColumn("dbo.LogsSubmittedRequestAwaitsResponse", "TaskID", c => c.Guid());
            AddColumn("dbo.LogsSubmittedRequestNeedsApproval", "TaskID", c => c.Guid());
            CreateIndex("dbo.LogsRoutingStatusChange", "TaskID");
            CreateIndex("dbo.LogsUploadedResultNeedsApproval", "TaskID");
            CreateIndex("dbo.LogsNewRequestSubmitted", "TaskID");
            CreateIndex("dbo.LogsRequestStatusChange", "TaskID");
            CreateIndex("dbo.LogsSubmittedRequestAwaitsResponse", "TaskID");
            CreateIndex("dbo.LogsSubmittedRequestNeedsApproval", "TaskID");
            AddForeignKey("dbo.LogsRoutingStatusChange", "TaskID", "dbo.Tasks", "ID");
            AddForeignKey("dbo.LogsUploadedResultNeedsApproval", "TaskID", "dbo.Tasks", "ID");
            AddForeignKey("dbo.LogsNewRequestSubmitted", "TaskID", "dbo.Tasks", "ID");
            AddForeignKey("dbo.LogsRequestStatusChange", "TaskID", "dbo.Tasks", "ID");
            AddForeignKey("dbo.LogsSubmittedRequestAwaitsResponse", "TaskID", "dbo.Tasks", "ID");
            AddForeignKey("dbo.LogsSubmittedRequestNeedsApproval", "TaskID", "dbo.Tasks", "ID");

            //Add event identifier for Task.Change
            Sql("INSERT INTO [Events] (ID, Name, Description) VALUES ('2DFE0001-B98D-461D-A705-A3BE01411396', 'Task Change', 'Users will be notified whenever a change occurs to a Task they have permission to see.')");
            //Add event identifier for Request.WorkflowActivityChanged
            Sql("INSERT INTO [Events] (ID, Name, Description) VALUES ('D2DC0001-43E9-477E-B60D-A3BE01550FA4', 'Request Workflow Activity Change', 'Users will be notified whenever the Workflow Activity for a Request changes if they have permission to view the Request.')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM [Events] WHERE ID = '2DFE0001-B98D-461D-A705-A3BE01411396' OR ID = 'D2DC0001-43E9-477E-B60D-A3BE01550FA4'");

            DropForeignKey("dbo.LogsRequestWorkflowActivityChanged", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.LogsSubmittedRequestNeedsApproval", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsSubmittedRequestAwaitsResponse", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsRequestStatusChange", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsNewRequestSubmitted", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsUploadedResultNeedsApproval", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsRoutingStatusChange", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsTaskChangedLog", "TaskID", "dbo.Tasks");
            DropIndex("dbo.LogsRequestWorkflowActivityChanged", new[] { "EventID" });
            DropIndex("dbo.LogsRequestWorkflowActivityChanged", new[] { "RequestID" });
            DropIndex("dbo.LogsSubmittedRequestNeedsApproval", new[] { "TaskID" });
            DropIndex("dbo.LogsSubmittedRequestAwaitsResponse", new[] { "TaskID" });
            DropIndex("dbo.LogsRequestStatusChange", new[] { "TaskID" });
            DropIndex("dbo.LogsNewRequestSubmitted", new[] { "TaskID" });
            DropIndex("dbo.LogsUploadedResultNeedsApproval", new[] { "TaskID" });
            DropIndex("dbo.LogsRoutingStatusChange", new[] { "TaskID" });
            DropIndex("dbo.LogsTaskChangedLog", new[] { "EventID" });
            DropIndex("dbo.LogsTaskChangedLog", new[] { "TaskID" });
            DropColumn("dbo.LogsSubmittedRequestNeedsApproval", "TaskID");
            DropColumn("dbo.LogsSubmittedRequestAwaitsResponse", "TaskID");
            DropColumn("dbo.LogsRequestStatusChange", "TaskID");
            DropColumn("dbo.LogsNewRequestSubmitted", "TaskID");
            DropColumn("dbo.LogsUploadedResultNeedsApproval", "TaskID");
            DropColumn("dbo.LogsRoutingStatusChange", "TaskID");
            DropTable("dbo.LogsRequestWorkflowActivityChanged");
            DropTable("dbo.LogsTaskChangedLog");
        }
    }
}
