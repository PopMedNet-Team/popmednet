namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSummaryQueryCompletionMapForModifyDraftReport : DbMigration
    {
        public override void Up()
        {
            Sql(@"Update WorkflowActivityCompletionMaps Set DestinationWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' Where WorkflowActivityResultID = 'ECCBF404-B3BA-4C5E-BB6E-388725938DC3' And SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' And DestinationWorkflowActivityID = '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81' and WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");
        }
        
        public override void Down()
        {
        }
    }
}
