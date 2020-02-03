namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRejectWorkflowCompletionStepForSummarywF : DbMigration
    {
        public override void Up()
        {
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0', '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896', 'EA120001-7A35-4829-9F2D-A3B600E25013')");

        }

        public override void Down()
        {
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', SourceWorkflowActivityID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0', DestinationWorkflowActivityID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896', WorkflowActivityResultID = 'EA120001-7A35-4829-9F2D-A3B600E25013'");
        }
    }
}
