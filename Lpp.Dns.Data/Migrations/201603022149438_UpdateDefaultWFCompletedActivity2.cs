namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDefaultWFCompletedActivity2 : DbMigration
    {
        public override void Up()
        {

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE (WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946'))
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', 'F64E0001-4F9A-49F0-BF75-A3B501396946')
                ");

        }
        
        public override void Down()
        {

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE (WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946')");

        }
    }
}
