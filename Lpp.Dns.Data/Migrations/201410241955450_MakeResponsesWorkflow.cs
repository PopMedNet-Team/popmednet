namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeResponsesWorkflow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestDataMartResponses", "WorkflowID", c => c.Guid());
            AddColumn("dbo.RequestDataMartResponses", "WorkFlowActivityID", c => c.Guid());
            CreateIndex("dbo.RequestDataMartResponses", "WorkflowID");
            CreateIndex("dbo.RequestDataMartResponses", "WorkFlowActivityID");
            AddForeignKey("dbo.RequestDataMartResponses", "WorkflowID", "dbo.Workflows", "ID");
            AddForeignKey("dbo.RequestDataMartResponses", "WorkFlowActivityID", "dbo.WorkflowActivities", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestDataMartResponses", "WorkFlowActivityID", "dbo.WorkflowActivities");
            DropForeignKey("dbo.RequestDataMartResponses", "WorkflowID", "dbo.Workflows");
            DropIndex("dbo.RequestDataMartResponses", new[] { "WorkFlowActivityID" });
            DropIndex("dbo.RequestDataMartResponses", new[] { "WorkflowID" });
            DropColumn("dbo.RequestDataMartResponses", "WorkFlowActivityID");
            DropColumn("dbo.RequestDataMartResponses", "WorkflowID");
        }
    }
}
