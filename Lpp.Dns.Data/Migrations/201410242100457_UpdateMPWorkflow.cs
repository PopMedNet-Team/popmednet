namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMPWorkflow : DbMigration
    {
        public override void Up()
        {
            Sql(@"-- Copy summary complete maps that are shared by MP
--
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('D0A0924F-F4B5-43BF-89A6-C7F32E764735', '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('668EE9C7-4930-423E-AA9E-150B646121F4', '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('36F8F9BA-849A-493F-A9FA-B443370EF5AD', '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81', 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('687360E2-8389-48E3-A3FE-71248F0D6192', '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81', '9173A8E7-27C4-469D-853D-69A78501A522', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('0811D461-626F-4CCF-B1FA-5B495858C67D', '2E7A3263-C87E-47BA-AC35-A78ABF8FE606', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('2AFFB9A9-3BC1-4039-ADD9-FE809C81C800', '2E7A3263-C87E-47BA-AC35-A78ABF8FE606', 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A', '9173A8E7-27C4-469D-853D-69A78501A522', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA', '9173A8E7-27C4-469D-853D-69A78501A522', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('7385973B-1C4F-4224-A13C-F148685F0217', '9173A8E7-27C4-469D-853D-69A78501A522', '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('B38C1515-BF25-4179-BA09-9F811E0053D8', 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('ECCBF404-B3BA-4C5E-BB6E-388725938DC3', 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208', '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('0CF45F91-6F2C-4283-BDC2-0896B552694A', 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208', '2E7A3263-C87E-47BA-AC35-A78ABF8FE606', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('4186A06D-D5CC-439D-8B7B-D2A1A97D3ADE', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('B240D900-8BE6-4907-8F08-590864A1EA1A', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('9CC66B2D-F813-4C6B-82C7-EE0893D72257', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '9173A8E7-27C4-469D-853D-69A78501A522', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('61D4C5E0-07AC-4FDF-9F60-FC073D7BECDA', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('634D54E5-74C5-46BC-A0DF-33F488AA584B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('7828CAD1-6547-4605-A361-6E76A796326B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('3FB86142-D6A1-45A5-A988-EF45B10D5C83', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('53579F36-9D20-47D9-AC33-643D9130080B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('E361B3BB-DE1F-40AF-893B-51EE1FF59E41', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('CD6B90AB-91B8-42E7-A3F7-8795AB405C48', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('1A9EBEAC-09CB-4BBC-952C-52A1DEB31094', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')
");
        }
        
        public override void Down()
        {
        }
    }
}
