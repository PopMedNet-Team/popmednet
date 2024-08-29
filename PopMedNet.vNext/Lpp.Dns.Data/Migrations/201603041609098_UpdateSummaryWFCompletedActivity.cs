namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSummaryWFCompletedActivity : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('E93CED3B-4B55-4991-AF84-07058ABE315C', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )
                ");


            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('E93CED3B-4B55-4991-AF84-07058ABE315C', '9173A8E7-27C4-469D-853D-69A78501A522', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                ");


            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('E93CED3B-4B55-4991-AF84-07058ABE315C','F888C5D6-B8EB-417C-9DE2-4A96D75F3208', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )
                ");


            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '0811D461-626F-4CCF-B1FA-5B495858C67D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE606' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '0811D461-626F-4CCF-B1FA-5B495858C67D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE606' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('0811D461-626F-4CCF-B1FA-5B495858C67D', '2E7A3263-C87E-47BA-AC35-A78ABF8FE606', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                ");


            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                ");

        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('E93CED3B-4B55-4991-AF84-07058ABE315C', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )");


            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('E93CED3B-4B55-4991-AF84-07058ABE315C', '9173A8E7-27C4-469D-853D-69A78501A522', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");


            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'E93CED3B-4B55-4991-AF84-07058ABE315C' AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('E93CED3B-4B55-4991-AF84-07058ABE315C','F888C5D6-B8EB-417C-9DE2-4A96D75F3208', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' )");


            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '0811D461-626F-4CCF-B1FA-5B495858C67D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE606' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '0811D461-626F-4CCF-B1FA-5B495858C67D' AND SourceWorkflowActivityID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE606' AND DestinationWorkflowActivityID = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
                INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES ('0811D461-626F-4CCF-B1FA-5B495858C67D', '2E7A3263-C87E-47BA-AC35-A78ABF8FE606', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");


            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D' AND SourceWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND DestinationWorkflowActivityID = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

        }
    }
}
