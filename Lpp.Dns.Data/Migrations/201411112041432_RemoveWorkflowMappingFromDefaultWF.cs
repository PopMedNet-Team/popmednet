namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveWorkflowMappingFromDefaultWF : DbMigration
    {
        public override void Up()
        {
            //remove mapping that is not needed
            Sql("DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946' AND WorkflowActivityResultID = 'FEB90001-19C4-48DB-A8A4-A3B600EE60C7' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND DestinationWorkflowActivityID = '6CE50001-A2B7-4721-890D-A3B600EDF917'");
        }
        
        public override void Down()
        {
            Sql("INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) VALUES ('FEB90001-19C4-48DB-A8A4-A3B600EE60C7','675F0001-6B44-4910-AD89-A3B600E98CE9','6CE50001-A2B7-4721-890D-A3B600EDF917','F64E0001-4F9A-49F0-BF75-A3B501396946')");
        }
    }
}
