namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompleteWorkflowSummaryQueryWFCompletionMaps : DbMigration
    {
        public override void Up()
        {
            Sql(@"--View Status And Results -> Complete
                    IF Not Exists( Select NULL From WorkflowActivityCompletionMaps Where WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' And SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                    BEGIN Insert Into WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) Values( 'E93CED3B-4B55-4991-AF84-07058ABE315C', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') END

                    --Sumbit Draft Report -> Complete
                    IF Not Exists( Select NULL From WorkflowActivityCompletionMaps Where WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' And SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                    BEGIN Insert Into WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) Values( 'E93CED3B-4B55-4991-AF84-07058ABE315C', '9173A8E7-27C4-469D-853D-69A78501A522', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') END

                    --Submit Final Report -> Complete
                    IF Not Exists( Select NULL From WorkflowActivityCompletionMaps Where WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' And SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' And WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                    BEGIN Insert Into WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) Values( 'E93CED3B-4B55-4991-AF84-07058ABE315C', 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8') END
                    ");
        }
        
        public override void Down()
        {
        }
    }
}
