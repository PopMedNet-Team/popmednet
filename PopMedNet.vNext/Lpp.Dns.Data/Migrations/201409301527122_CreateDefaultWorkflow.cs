namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDefaultWorkflow : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Workflows (ID, Name, Description) VALUES ('F64E0001-4F9A-49F0-BF75-A3B501396946', 'Default', 'A default request workflow.')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [Start]) VALUES ('C1380001-4524-49BA-B4B6-A3B5013A3343', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Create Request', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [End]) VALUES ('CC2E0001-9B99-4C67-8DED-A3B600E1C696', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Terminate Request', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('73740001-A942-47B0-BF6E-A3B600E7D9EC', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Request Review', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'View Request', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('675F0001-6B44-4910-AD89-A3B600E98CE9', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'View Response', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('6CE50001-A2B7-4721-890D-A3B600EDF917', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Approve Response', '')

-- delete a request that has not been submitted: create request => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('61110001-1708-4869-BDCF-A3B600E24AA3', 'Delete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('61110001-1708-4869-BDCF-A3B600E24AA3','C1380001-4524-49BA-B4B6-A3B5013A3343','CC2E0001-9B99-4C67-8DED-A3B600E1C696')

-- submit a request for approval: create request => request approval
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('48B20001-BD0B-425D-8D49-A3B5015A2258', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('48B20001-BD0B-425D-8D49-A3B5015A2258','C1380001-4524-49BA-B4B6-A3B5013A3343','73740001-A942-47B0-BF6E-A3B600E7D9EC')

-- review a request and either approve or reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('EA120001-7A35-4829-9F2D-A3B600E25013', 'Reject', '')
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('50C60001-891F-40E6-B95F-A3B600E25C2B', 'Approve', '')
-- reject request: request approval => terminate
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('EA120001-7A35-4829-9F2D-A3B600E25013','73740001-A942-47B0-BF6E-A3B600E7D9EC','CC2E0001-9B99-4C67-8DED-A3B600E1C696')
-- approve request: request approval => view request
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('50C60001-891F-40E6-B95F-A3B600E25C2B','73740001-A942-47B0-BF6E-A3B600E7D9EC','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344')

-- request autocompletes: view request => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('E1C90001-B582-4180-9A71-A3B600EA0C27', 'Auto Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('E1C90001-B582-4180-9A71-A3B600EA0C27','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','CC2E0001-9B99-4C67-8DED-A3B600E1C696')

-- submission of routings, auto route: view request => view response
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('A9B30001-9A57-4268-9FB2-A3B600E26A53', 'Auto Route', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('A9B30001-9A57-4268-9FB2-A3B600E26A53','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','675F0001-6B44-4910-AD89-A3B600E98CE9')

-- cancel response: view response => view request
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('5E010001-1353-44E9-9204-A3B600E263E9', 'Cancel', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('5E010001-1353-44E9-9204-A3B600E263E9','675F0001-6B44-4910-AD89-A3B600E98CE9','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344')
-- resubmit: view response => view request
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('22AE0001-0B5A-4BA9-BB55-A3B600E2728C', 'Resubmit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('22AE0001-0B5A-4BA9-BB55-A3B600E2728C','675F0001-6B44-4910-AD89-A3B600E98CE9','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344')

-- view response: view request => view response
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('1C1D0001-65F4-4E02-9BB7-A3B600E27A2F', 'View', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('1C1D0001-65F4-4E02-9BB7-A3B600E27A2F','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344','675F0001-6B44-4910-AD89-A3B600E98CE9')

-- upload response: view response => approve response?
INSERT INTO WorkflowActivityResults(ID, Name, Description) VALUES ('FEB90001-19C4-48DB-A8A4-A3B600EE60C7', 'Upload', 'Upload response.')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('FEB90001-19C4-48DB-A8A4-A3B600EE60C7','675F0001-6B44-4910-AD89-A3B600E98CE9','6CE50001-A2B7-4721-890D-A3B600EDF917')

-- reject response: approve response => view request
INSERT INTO WorkflowActivityResults(ID, Name, Description) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49', 'Reject', 'Reject response.')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('F1B10001-B0B3-45A9-AAFF-A3B600EEFC49','6CE50001-A2B7-4721-890D-A3B600EDF917','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344')

-- approve response: approve response => view request
INSERT INTO WorkflowActivityResults(ID, Name, Description) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F', 'Approve', 'Approve response.')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('0FEE0001-ED08-48D8-8C0B-A3B600EEF30F','6CE50001-A2B7-4721-890D-A3B600EDF917','ACBA0001-0CE4-4C00-8DD3-A3B5013A3344')
");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps
DELETE FROM WorkflowActivities
DELETE FROM WorkflowActivityResults
DELETE FROM Workflows");
        }
    }
}
