namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkflow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workflows",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.WorkflowActivities",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        WorkflowID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Workflows", t => t.WorkflowID, cascadeDelete: true)
                .Index(t => t.WorkflowID)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.WorkflowActivityCompletionMaps",
                c => new
                    {
                        WorkflowActivityResultID = c.Guid(nullable: false),
                        SourceWorkflowActivityID = c.Guid(nullable: false),
                        DestinationWorkflowActivityID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkflowActivityResultID, t.SourceWorkflowActivityID, t.DestinationWorkflowActivityID })
                .ForeignKey("dbo.WorkflowActivityResults", t => t.WorkflowActivityResultID, cascadeDelete: true)
                .ForeignKey("dbo.WorkflowActivities", t => t.DestinationWorkflowActivityID)
                .ForeignKey("dbo.WorkflowActivities", t => t.SourceWorkflowActivityID, cascadeDelete: true)
                .Index(t => t.WorkflowActivityResultID)
                .Index(t => t.SourceWorkflowActivityID)
                .Index(t => t.DestinationWorkflowActivityID);
            
            CreateTable(
                "dbo.WorkflowActivityResults",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID);

            Sql(@"  CREATE TRIGGER [dbo].[WorkflowActivitiesDelete] 
		ON  [dbo].[WorkflowActivities]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM WorkflowActivityCompletionMaps WHERE DestinationWorkflowActivityID IN (SELECT ID FROM deleted)
	END");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkflowActivities", "WorkflowID", "dbo.Workflows");
            DropForeignKey("dbo.WorkflowActivityCompletionMaps", "SourceWorkflowActivityID", "dbo.WorkflowActivities");
            DropForeignKey("dbo.WorkflowActivityCompletionMaps", "DestinationWorkflowActivityID", "dbo.WorkflowActivities");
            DropForeignKey("dbo.WorkflowActivityCompletionMaps", "WorkflowActivityResultID", "dbo.WorkflowActivityResults");
            DropIndex("dbo.WorkflowActivityCompletionMaps", new[] { "DestinationWorkflowActivityID" });
            DropIndex("dbo.WorkflowActivityCompletionMaps", new[] { "SourceWorkflowActivityID" });
            DropIndex("dbo.WorkflowActivityCompletionMaps", new[] { "WorkflowActivityResultID" });
            DropIndex("dbo.WorkflowActivities", new[] { "Name" });
            DropIndex("dbo.WorkflowActivities", new[] { "WorkflowID" });
            DropIndex("dbo.Workflows", new[] { "Name" });
            DropTable("dbo.WorkflowActivityResults");
            DropTable("dbo.WorkflowActivityCompletionMaps");
            DropTable("dbo.WorkflowActivities");
            DropTable("dbo.Workflows");
        }
    }
}
