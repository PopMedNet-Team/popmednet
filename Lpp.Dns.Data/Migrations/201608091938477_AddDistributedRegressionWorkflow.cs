namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDistributedRegressionWorkflow : DbMigration
    {
        public override void Up()
        {
            //--Create Distributed Regression Workflow
            Sql(@"INSERT INTO Workflows([ID], [Name], [Description]) VALUES ('E9656288-33FF-4D1F-BA77-C82EB0BF0192','Distributed Regression','Workflow for Distributed Regression')");

            //--Create Conduct Analysis Activity
            Sql(@"INSERT INTO WorkflowActivities([ID], [Name], [Description], [Start], [End]) VALUES ('370646FC-7A47-43B5-A4B3-659F90A188A9','Conduct Analysis','Conduct Analysis Activity for Distributed Regression',0,0)");

            //--Create Copy Activity Result
            Sql(@"Insert INTO WorkflowActivityResults([ID],[Name],[Description])VALUES('47538F13-9257-4161-BCD0-AA7DEA897AE5','Copy','Copying of Distributed Regression Requests')");

            //--Create Complete Routings Activity Result
            Sql(@"Insert INTO WorkflowActivityResults([ID],[Name],[Description])VALUES('8A68399F-D562-4A98-87C9-195D3D83A103','Complete Routings','Complete Routings of Distributed Regression Requests')");

            //--DISTIBUTION Activity SAVE
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', '94E90001-A620-4624-9003-A64F0121D0D7', '94E90001-A620-4624-9003-A64F0121D0D7', 'DFF3000B-B076-4D07-8D83-05EDE3636F4D')");

            //--DISTIBUTION Activity Copy
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
			VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', '94E90001-A620-4624-9003-A64F0121D0D7', '94E90001-A620-4624-9003-A64F0121D0D7', '47538F13-9257-4161-BCD0-AA7DEA897AE5')");

            //--DISTIBUTION Activity SUBMIT
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', '94E90001-A620-4624-9003-A64F0121D0D7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5445DC6E-72DC-4A6B-95B6-338F0359F89E')");

            //--DISTIBUTION Activity TERMINATE
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', '94E90001-A620-4624-9003-A64F0121D0D7', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '53579F36-9D20-47D9-AC33-643D9130080B')");

            //--Complete Distribution Activtiy Redistribute
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5C5E0001-10A6-4992-A8BE-A3F4012D5FEB')");

            //--Complete Distribution Activity Bulk Edit
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '4F7E1762-E453-4D12-8037-BAE8A95523F7')");

            //--Complete Distribution Activity Routings Add DataMarts
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '15BDEF13-6E86-4E0F-8790-C07AE5B798A8')");

            //--Complete Distribution Activity Remove DataMarts
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5E010001-1353-44E9-9204-A3B600E263E9')");

            //--Complete Distribution Activity Complete Routings
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
			VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '8A68399F-D562-4A98-87C9-195D3D83A103')");

            //--Complete Distribution Activity Save
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'DFF3000B-B076-4D07-8D83-05EDE3636F4D')");

            //--Complete Distribution Activity Terminate
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '53579F36-9D20-47D9-AC33-643D9130080B')");

            //--Conduct Analysis Activity Save
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
			VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', '370646FC-7A47-43B5-A4B3-659F90A188A9', '370646FC-7A47-43B5-A4B3-659F90A188A9', 'DFF3000B-B076-4D07-8D83-05EDE3636F4D')");

            //--Conduct Analysis Activity Terminate
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
			VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', '370646FC-7A47-43B5-A4B3-659F90A188A9', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '53579F36-9D20-47D9-AC33-643D9130080B')");

            //--Completed Activity Save
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('E9656288-33FF-4D1F-BA77-C82EB0BF0192', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', 'DFF3000B-B076-4D07-8D83-05EDE3636F4D')");

            //--Add Requestor Role
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('2F60E04D-8289-4901-B4FA-E0FD378BA275','E9656288-33FF-4D1F-BA77-C82EB0BF0192','Request Creator','The Request Creator for the workflow.',1)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('A90F98F2-E5EA-40CD-8120-154B859B441D','E9656288-33FF-4D1F-BA77-C82EB0BF0192','Primary Analyst','The Primary Analyst of the workflow.',0)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('CC1736F7-DFF9-43E1-904A-0871F2CAE1C7','E9656288-33FF-4D1F-BA77-C82EB0BF0192','Requestor','The Requestor that initiated the request.',0)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('4626EBA4-B093-4F59-9F59-5D1E9A458507','E9656288-33FF-4D1F-BA77-C82EB0BF0192','Secondary Analyst','The Secondary Analyst of the workflow.',0)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('A839F0E3-7267-45F5-AEEC-77712FAA212D','E9656288-33FF-4D1F-BA77-C82EB0BF0192','Scientific Lead','The Scientific Lead of the workflow.',0)");

        }

        public override void Down()
        {
            Sql(@"Delete from WorkflowRoles where WorkflowID = 'E9656288-33FF-4D1F-BA77-C82EB0BF0192'");
            Sql(@"Delete from WorkflowActivityResults where ID = '8A68399F-D562-4A98-87C9-195D3D83A103'");
            Sql(@"Delete from WorkflowActivityResults where ID = '47538F13-9257-4161-BCD0-AA7DEA897AE5'");
            Sql(@"Delete from WorkflowActivities where ID = '370646FC-7A47-43B5-A4B3-659F90A188A9'");
            Sql(@"Delete from WorkflowActivityCompletionMaps where WorkflowID = 'E9656288-33FF-4D1F-BA77-C82EB0BF0192'");
            Sql(@"Delete from Workflows where ID = 'E9656288-33FF-4D1F-BA77-C82EB0BF0192'");
        }
    }
}
