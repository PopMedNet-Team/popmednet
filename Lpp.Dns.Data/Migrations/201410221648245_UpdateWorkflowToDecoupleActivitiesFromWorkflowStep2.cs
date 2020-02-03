namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowToDecoupleActivitiesFromWorkflowStep2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkflowActivityCompletionMaps", "WorkflowID", c => c.Guid());

            Sql(@"UPDATE WorkflowActivityCompletionMaps SET WorkflowID = (SELECT WorkflowID FROM WorkflowActivities WHERE WorkflowActivities.ID = WorkflowActivityCompletionMaps.SourceWorkflowActivityID) WHERE WorkflowID IS NULL");
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkflowActivityCompletionMaps", "WorkflowID");
        }
    }
}
