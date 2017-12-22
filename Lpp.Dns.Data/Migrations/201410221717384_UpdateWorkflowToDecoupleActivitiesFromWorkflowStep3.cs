namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowToDecoupleActivitiesFromWorkflowStep3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkflowActivities", "WorkflowID", "dbo.Workflows");
            DropIndex("dbo.WorkflowActivities", new[] { "WorkflowID" });
            DropPrimaryKey("dbo.WorkflowActivityCompletionMaps");
            AlterColumn("dbo.WorkflowActivityCompletionMaps", "WorkflowID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.WorkflowActivityCompletionMaps", new[] { "WorkflowID", "WorkflowActivityResultID", "SourceWorkflowActivityID", "DestinationWorkflowActivityID" });
            DropColumn("dbo.WorkflowActivities", "WorkflowID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkflowActivities", "WorkflowID", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.WorkflowActivityCompletionMaps");
            AlterColumn("dbo.WorkflowActivityCompletionMaps", "WorkflowID", c => c.Guid());
            AddPrimaryKey("dbo.WorkflowActivityCompletionMaps", new[] { "WorkflowActivityResultID", "SourceWorkflowActivityID", "DestinationWorkflowActivityID" });
            CreateIndex("dbo.WorkflowActivities", "WorkflowID");
            AddForeignKey("dbo.WorkflowActivities", "WorkflowID", "dbo.Workflows", "ID", cascadeDelete: true);
        }
    }
}
