namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSummaryWFMappings1 : DbMigration
    {
        public override void Up()
        {
            // Updates for Summary Workflow Mapping
            Sql(@"-- setup mapping for viewing response detail
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F' AND DestinationWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9')
	INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, WorkflowActivityResultID, DestinationWorkflowActivityID) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','1C1D0001-65F4-4E02-9BB7-A3B600E27A2F','675F0001-6B44-4910-AD89-A3B600E98CE9')

--update appove response mapping
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = 'B240D900-8BE6-4907-8F08-590864A1EA1A' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowActivityResultID = '0FEE0001-ED08-48D8-8C0B-A3B600EEF30F' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
	INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, WorkflowActivityResultID, DestinationWorkflowActivityID) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','675F0001-6B44-4910-AD89-A3B600E98CE9','0FEE0001-ED08-48D8-8C0B-A3B600EEF30F','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')

--update reject response mapping
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = '634D54E5-74C5-46BC-A0DF-33F488AA584B' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowActivityResultID = 'F1B10001-B0B3-45A9-AAFF-A3B600EEFC49' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
	INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, WorkflowActivityResultID, DestinationWorkflowActivityID) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','675F0001-6B44-4910-AD89-A3B600E98CE9','F1B10001-B0B3-45A9-AAFF-A3B600EEFC49','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')

-- return to view routings from response detail
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowActivityResultID = 'FEB90001-19C4-48DB-A8A4-A3B600EE60C7' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
	INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, WorkflowActivityResultID, DestinationWorkflowActivityID) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','675F0001-6B44-4910-AD89-A3B600E98CE9','FEB90001-19C4-48DB-A8A4-A3B600EE60C7','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
");
        }
        
        public override void Down()
        {
            // Reverse the changes to the Summary WF mappings.
            Sql(@"-- setup mapping for viewing response detail
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F' AND DestinationWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9'
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
	INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, WorkflowActivityResultID, DestinationWorkflowActivityID) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','1C1D0001-65F4-4E02-9BB7-A3B600E27A2F','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')

--update approve response mapping
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowActivityResultID = '0FEE0001-ED08-48D8-8C0B-A3B600EEF30F' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = 'B240D900-8BE6-4907-8F08-590864A1EA1A' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
	INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, WorkflowActivityResultID, DestinationWorkflowActivityID) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','B240D900-8BE6-4907-8F08-590864A1EA1A','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')

--update reject response mapping
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowActivityResultID = 'F1B10001-B0B3-45A9-AAFF-A3B600EEFC49' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'
IF NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND WorkflowActivityResultID = '634D54E5-74C5-46BC-A0DF-33F488AA584B' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')
	INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, SourceWorkflowActivityID, WorkflowActivityResultID, DestinationWorkflowActivityID) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','634D54E5-74C5-46BC-A0DF-33F488AA584B','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')

-- delete the return to view routings from response detail result
DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND WorkflowActivityResultID = 'FEB90001-19C4-48DB-A8A4-A3B600EE60C7' AND DestinationWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'");
        }
    }
}
