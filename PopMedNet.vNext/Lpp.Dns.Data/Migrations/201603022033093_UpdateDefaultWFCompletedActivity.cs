namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDefaultWFCompletedActivity : DbMigration
    {
        public override void Up()
        {

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivities WHERE ID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC') 
                INSERT INTO WorkflowActivities(ID, Name, Description, Start, [End])
                VALUES ('9392ACEF-1AF3-407C-B19C-BAE88C389BFC', 'Completed', '', 0, 1) ");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' ");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' ) 
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('E1C90001-B582-4180-9A71-A3B600EA0C27', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC','F64E0001-4F9A-49F0-BF75-A3B501396946' )");

        }
        
        public override void Down()
        {

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' ");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946')
                INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('E1C90001-B582-4180-9A71-A3B600EA0C27', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696','F64E0001-4F9A-49F0-BF75-A3B501396946' )");

            Sql(@"DELETE FROM WorkflowActivities WHERE ID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC'");

        }
    }
}
