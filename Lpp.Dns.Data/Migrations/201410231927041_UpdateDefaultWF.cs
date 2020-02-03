namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDefaultWF : DbMigration
    {
        public override void Up()
        {
            Sql(@"-- Update default workflow task names
UPDATE [dbo].WorkflowActivities SET [Name] = 'Request Form' WHERE [ID] = 'C1380001-4524-49BA-B4B6-A3B5013A3343'
UPDATE [dbo].WorkflowActivities SET [Name] = 'View Responses' WHERE [ID] = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'
UPDATE [dbo].WorkflowActivities SET [Name] = 'Terminate Request' WHERE [ID] = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696'
UPDATE [dbo].WorkflowActivities SET [Name] = 'Review Request Form' WHERE [ID] = '73740001-A942-47B0-BF6E-A3B600E7D9EC'
UPDATE [dbo].WorkflowActivities SET [Name] = 'View Response' WHERE [ID] = '675F0001-6B44-4910-AD89-A3B600E98CE9'
UPDATE [dbo].WorkflowActivities SET [Name] = 'Results Review' WHERE [ID] = '6CE50001-A2B7-4721-890D-A3B600EDF917'

UPDATE [dbo].[WorkflowActivityResults] SET [Name] = 'Complete' WHERE [ID] = 'E1C90001-B582-4180-9A71-A3B600EA0C27'
UPDATE [dbo].[WorkflowActivityResults] SET [Name] = 'View Result' WHERE [ID] = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F'

-- Add new submit for approval button for Default WF
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('C4FB25F8-8521-427E-8FB1-78A84311BF1C', 'Submit for Approval')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('C4FB25F8-8521-427E-8FB1-78A84311BF1C', 'C1380001-4524-49BA-B4B6-A3B5013A3343', '73740001-A942-47B0-BF6E-A3B600E7D9EC', 'F64E0001-4F9A-49F0-BF75-A3B501396946')
DELETE [dbo].[WorkflowActivityCompletionMaps] WHERE [WorkflowActivityResultID] = '48B20001-BD0B-425D-8D49-A3B5015A2258' AND [SourceWorkflowActivityID] = 'C1380001-4524-49BA-B4B6-A3B5013A3343' AND [DestinationWorkflowActivityID] = '73740001-A942-47B0-BF6E-A3B600E7D9EC' AND [WorkflowID] = 'F64E0001-4F9A-49F0-BF75-A3B501396946'

UPDATE [dbo].[WorkflowActivityCompletionMaps] SET SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', [DestinationWorkflowActivityID] = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'  WHERE [WorkflowActivityResultID] = '22AE0001-0B5A-4BA9-BB55-A3B600E2728C' AND [SourceWorkflowActivityID] = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND [DestinationWorkflowActivityID] = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND [WorkflowID] = 'F64E0001-4F9A-49F0-BF75-A3B501396946'
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', [DestinationWorkflowActivityID] = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'  WHERE [WorkflowActivityResultID] = '5E010001-1353-44E9-9204-A3B600E263E9' AND [SourceWorkflowActivityID] = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND [DestinationWorkflowActivityID] = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND [WorkflowID] = 'F64E0001-4F9A-49F0-BF75-A3B501396946'

-- Add DataMarts
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('15BDEF13-6E86-4E0F-8790-C07AE5B798A8', 'Add DataMarts')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('15BDEF13-6E86-4E0F-8790-C07AE5B798A8', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'F64E0001-4F9A-49F0-BF75-A3B501396946')

-- Remove DataMarts
UPDATE [dbo].[WorkflowActivityResults] SET [Name] = 'Remove DataMarts' WHERE [ID] = '5E010001-1353-44E9-9204-A3B600E263E9'

-- View Result
UPDATE [dbo].[WorkflowActivityCompletionMaps] SET [DestinationWorkflowActivityID] = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'  WHERE [WorkflowActivityResultID] = '1C1D0001-65F4-4E02-9BB7-A3B600E27A2F' AND [SourceWorkflowActivityID] = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND [DestinationWorkflowActivityID] = '675F0001-6B44-4910-AD89-A3B600E98CE9' AND [WorkflowID] = 'F64E0001-4F9A-49F0-BF75-A3B501396946'

-- Group results
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('49F9C682-9FAD-4AE5-A2C5-19157E227186', 'Group')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('49F9C682-9FAD-4AE5-A2C5-19157E227186', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'F64E0001-4F9A-49F0-BF75-A3B501396946')
INSERT INTO [dbo].[WorkflowActivityResults] ([ID] ,[Name]) VALUES ('7821FC45-9FD5-4597-A405-B021E5ED14FA', 'Ungroup')
INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID] , [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('7821FC45-9FD5-4597-A405-B021E5ED14FA', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'F64E0001-4F9A-49F0-BF75-A3B501396946')
");
        }
        
        public override void Down()
        {
            
        }
    }
}
