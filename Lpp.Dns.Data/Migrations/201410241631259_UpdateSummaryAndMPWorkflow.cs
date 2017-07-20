namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSummaryAndMPWorkflow : DbMigration
    {
        public override void Up()
        {
            Sql(@"--
-- Modify Summary WF
--
-- Modify Distribute to directly to response to request
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET [DestinationWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' WHERE [WorkflowActivityResultID] = '80FD6F76-2E32-4D35-9797-0B541507CB56' AND [SourceWorkflowActivityID] = '752B83D7-2190-49DF-9BAE-983A7880A899' AND [DestinationWorkflowActivityID] = '752B83D7-2190-49DF-9BAE-983A7880A899' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'

-- Replace Complete result that goes to Terminate with a Complete with no reports result and map 
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('E361B3BB-DE1F-40AF-893B-51EE1FF59E41', 'Terminate Request')
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET [WorkflowActivityResultID] = 'E361B3BB-DE1F-40AF-893B-51EE1FF59E41' WHERE [WorkflowActivityResultID] = '9CC66B2D-F813-4C6B-82C7-EE0893D72257' AND [SourceWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND [DestinationWorkflowActivityID] = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'


-- Update view status and results activity name to not confict with DM admin activity
UPDATE [dbo].WorkflowActivities SET [Name] = 'View Responses' WHERE [ID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'


-- Change Cancel DM route result name to be Remove DataMart
UPDATE [dbo].[WorkflowActivityResults] SET [Name] = 'Remove DataMart' WHERE [ID] = '7828CAD1-6547-4605-A361-6E76A796326B'
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET [DestinationWorkflowActivityID] = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' WHERE [WorkflowActivityResultID] = '7828CAD1-6547-4605-A361-6E76A796326B' AND [SourceWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND [DestinationWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'

-- Add Add DataMart result and map
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('4186A06D-D5CC-439D-8B7B-D2A1A97D3ADE', 'Add DataMart')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('4186A06D-D5CC-439D-8B7B-D2A1A97D3ADE', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')

-- Add Group and map
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('61D4C5E0-07AC-4FDF-9F60-FC073D7BECDA', 'Group')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('61D4C5E0-07AC-4FDF-9F60-FC073D7BECDA', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')
-- Add UnGroup and map
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('CD6B90AB-91B8-42E7-A3F7-8795AB405C48', 'Ungroup')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('CD6B90AB-91B8-42E7-A3F7-8795AB405C48', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')

-- Remove one of the DM Upload, have the other one just to View Respones
DELETE [dbo].[WorkflowActivityCompletionMaps] WHERE [WorkflowActivityResultID] = '668EE9C7-4930-423E-AA9E-150B646121F4' AND [SourceWorkflowActivityID] = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND [DestinationWorkflowActivityID] = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'
DELETE [dbo].[WorkflowActivityCompletionMaps] WHERE [WorkflowActivityResultID] = 'D0A0924F-F4B5-43BF-89A6-C7F32E764735' AND [SourceWorkflowActivityID] = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND [DestinationWorkflowActivityID] = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'

-- Move approve / reject to View results activity
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET [SourceWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', [DestinationWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' WHERE [WorkflowActivityResultID] = 'B240D900-8BE6-4907-8F08-590864A1EA1A' AND [SourceWorkflowActivityID] = '9CDD7176-9361-4585-B79C-438645DA45BE' AND [DestinationWorkflowActivityID] = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET [SourceWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', [DestinationWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' WHERE [WorkflowActivityResultID] = '634D54E5-74C5-46BC-A0DF-33F488AA584B' AND [SourceWorkflowActivityID] = '9CDD7176-9361-4585-B79C-438645DA45BE' AND [DestinationWorkflowActivityID] = '752B83D7-2190-49DF-9BAE-983A7880A899' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'
DELETE [dbo].[WorkflowActivityCompletionMaps] WHERE [WorkflowActivityResultID] = 'C494B6D6-622F-4BD8-BEA3-2716FE34D5AD' AND [SourceWorkflowActivityID] = '9CDD7176-9361-4585-B79C-438645DA45BE' AND [DestinationWorkflowActivityID] = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' AND [WorkflowID] = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'

--
-- MP WF Update 
--
-- Reuse View Responses and after activities from Summary WF
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET [DestinationWorkflowActivityID] = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' WHERE [WorkflowActivityResultID] = '5445DC6E-72DC-4A6B-95B6-338F0359F89E' AND [SourceWorkflowActivityID] = 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554' AND [DestinationWorkflowActivityID] = 'D51D0D4F-41F7-4208-8722-6D71B23DE2F9' AND [WorkflowID] = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'
DELETE [dbo].[WorkflowActivityCompletionMaps] WHERE WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' AND SourceWorkflowActivityID in ('D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D51D0D4F-41F7-4208-8722-6D71B23DE2F9', 'C80810A3-CF10-4941-854A-A7E2052A5EBA', '4CCE18C8-CABF-4D22-88AB-611CD560DBF8', '7B4EB88B-1295-45B9-AE19-5BC45E98C985', '43AB48FD-A400-4C0D-92C9-DD2415A5D5B6', '1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9')
DELETE [dbo].[WorkflowActivities] WHERE ID in ('D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D51D0D4F-41F7-4208-8722-6D71B23DE2F9', 'C80810A3-CF10-4941-854A-A7E2052A5EBA', '4CCE18C8-CABF-4D22-88AB-611CD560DBF8', '7B4EB88B-1295-45B9-AE19-5BC45E98C985', '43AB48FD-A400-4C0D-92C9-DD2415A5D5B6', '1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9')

--
-- Delete any unused results
DELETE [dbo].[WorkflowActivityResults] WHERE ID not in (SELECT [WorkflowActivityResultID] FROM [dbo].[WorkflowActivityCompletionMaps])
-- Delete any unsed activities
DELETE [dbo].[WorkflowActivities] WHERE ID not in (SELECT [SourceWorkflowActivityID] FROM [dbo].[WorkflowActivityCompletionMaps] UNION SELECT [DestinationWorkflowActivityID] FROM [dbo].[WorkflowActivityCompletionMaps])");
        }
        
        public override void Down()
        {
        }
    }
}
