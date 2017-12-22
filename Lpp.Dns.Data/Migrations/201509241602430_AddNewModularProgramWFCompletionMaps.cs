namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewModularProgramWFCompletionMaps : DbMigration
    {
        public override void Up()
        {
            Sql(@"--View Status And Results -> Complete
                    IF Not Exists( Select NULL From WorkflowActivityCompletionMaps Where WorkflowActivityResultID = '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A' And SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' And WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
                    BEGIN Insert Into WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) Values( '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') END

                    --Sumbit Draft Report -> Complete
                    IF Not Exists( Select NULL From WorkflowActivityCompletionMaps Where WorkflowActivityResultID = '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A' And SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' And WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
                    BEGIN Insert Into WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) Values( '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A', '9173A8E7-27C4-469D-853D-69A78501A522', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') END

                    --Submit Final Report -> Complete
                    IF Not Exists( Select NULL From WorkflowActivityCompletionMaps Where WorkflowActivityResultID = '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A' And SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' And WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
                    BEGIN Insert Into WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID) Values( '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A', 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D') END
                    ");
        }
        
        public override void Down()
        {
            Sql(@"--View Status And Results -> Complete
                    Delete from WorkflowActivityCompletionMaps 
                    Where WorkflowActivityResultID = '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A'
                    And SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' 
                    AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696'
                    And WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'

                    --Sumbit Draft Report -> Complete
                    Delete from WorkflowActivityCompletionMaps 
                    Where WorkflowActivityResultID = '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A'
                    And SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' 
                    AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696'
                    And WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'

                    --Submit Final Report -> Complete
                    Delete from WorkflowActivityCompletionMaps 
                    Where WorkflowActivityResultID = '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A'
                    And SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' 
                    AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696'
                    And WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'
                    ");
        }
    }
}
