namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDefaultWorkflowMappings : DbMigration
    {
        public override void Up()
        {
            //remove the autoroute mapping
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' AND WorkflowActivityResultID ='A9B30001-9A57-4268-9FB2-A3B600E26A53' AND SourceWorkflowActivityID ='ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'");
            //change the view response mapping to go to the response details step
            Sql(@"UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' WHERE WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' AND WorkflowActivityResultID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'");
            //add approve result to the view response activity
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F', 'F64E0001-4F9A-49F0-BF75-A3B501396946', '675F0001-6B44-4910-AD89-A3B600E98CE9', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344')");
            //add reject result to the view response activity
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49', 'F64E0001-4F9A-49F0-BF75-A3B501396946', '675F0001-6B44-4910-AD89-A3B600E98CE9', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344')");
        }
        
        public override void Down()
        {
        }
    }
}
