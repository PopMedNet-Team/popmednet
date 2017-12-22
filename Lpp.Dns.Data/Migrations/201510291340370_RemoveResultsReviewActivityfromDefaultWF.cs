namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveResultsReviewActivityfromDefaultWF : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '0FEE0001-ED08-48D8-8C0B-A3B600EEF30F' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND workflowid = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'F1B10001-B0B3-45A9-AAFF-A3B600EEFC49' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND workflowid = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND DestinationWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND workflowid = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");

            Sql(@"DELETE FROM WorkflowActivities where id = '6CE50001-A2B7-4721-890D-A3B600EDF917'");

        }
        
        public override void Down()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '6CE50001-A2B7-4721-890D-A3B600EDF917') INSERT INTO WorkflowActivities (ID, Name) VALUES ('6CE50001-A2B7-4721-890D-A3B600EDF917', 'Results Review')");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '0FEE0001-ED08-48D8-8C0B-A3B600EEF30F' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946') INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F', '6CE50001-A2B7-4721-890D-A3B600EDF917',  'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','F64E0001-4F9A-49F0-BF75-A3B501396946')");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'F1B10001-B0B3-45A9-AAFF-A3B600EEFC49' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' ) INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49', '6CE50001-A2B7-4721-890D-A3B600EDF917',  'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','F64E0001-4F9A-49F0-BF75-A3B501396946')");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917' AND DestinationWorkflowActivityID =  '6CE50001-A2B7-4721-890D-A3B600EDF917' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' ) INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D' , '6CE50001-A2B7-4721-890D-A3B600EDF917',  '6CE50001-A2B7-4721-890D-A3B600EDF917','F64E0001-4F9A-49F0-BF75-A3B501396946')");

        }
    }
}
