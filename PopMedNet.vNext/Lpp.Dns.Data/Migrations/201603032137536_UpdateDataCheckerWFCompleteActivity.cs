namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataCheckerWFCompleteActivity : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
                INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('E1C90001-B582-4180-9A71-A3B600EA0C27', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663' )");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
                INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC','9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663' )");

        }
        
        public override void Down()
        {

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')
                INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) 
                VALUES ('E1C90001-B582-4180-9A71-A3B600EA0C27','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' , 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663' )
                ");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E1C90001-B582-4180-9A71-A3B600EA0C27' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

        }
    }
}
