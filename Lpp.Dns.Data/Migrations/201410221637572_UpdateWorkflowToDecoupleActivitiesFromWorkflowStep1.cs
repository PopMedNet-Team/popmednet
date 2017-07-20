namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowToDecoupleActivitiesFromWorkflowStep1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "WorkflowID", c => c.Guid());
            CreateIndex("dbo.Requests", "WorkflowID");
            CreateIndex("dbo.Requests", "WorkFlowActivityID");
            AddForeignKey("dbo.Requests", "WorkflowID", "dbo.Workflows", "ID");
            AddForeignKey("dbo.Requests", "WorkFlowActivityID", "dbo.WorkflowActivities", "ID");

            Sql("UPDATE Requests SET WorkflowID = (SELECT TOP 1 WorkflowID FROM Workflowactivities WHERE ID = Requests.WorkFlowActivityID) WHERE NOT Requests.WorkflowActivityID IS NULL");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "WorkFlowActivityID", "dbo.WorkflowActivities");
            DropForeignKey("dbo.Requests", "WorkflowID", "dbo.Workflows");
            DropIndex("dbo.Requests", new[] { "WorkFlowActivityID" });
            DropIndex("dbo.Requests", new[] { "WorkflowID" });
            DropColumn("dbo.Requests", "WorkflowID");
        }
    }
}
