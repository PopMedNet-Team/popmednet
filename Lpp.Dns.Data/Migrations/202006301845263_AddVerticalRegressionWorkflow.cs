namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVerticalRegressionWorkflow : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF EXISTS (SELECT NULL FROM sys.triggers WHERE Name = 'RequestDataMartResponsesInsert') BEGIN DROP TRIGGER [dbo].[RequestDataMartResponsesInsert] END ");
            Sql("UPDATE Workflows SET Name = 'Horizontal Distributed Regression', Description = 'Workflow for Horizontal Distributed Regression' WHERE ID = 'E9656288-33FF-4D1F-BA77-C82EB0BF0192'");
            Sql(@"IF NOT EXISTS (SELECT NULL FROM Workflows WHERE ID = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A')
                BEGIN
                    INSERT INTO Workflows([ID], [Name], [Description]) VALUES ('047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A','Vertical Distributed Regression','Workflow for Vertical Distributed Regression')
                END");

            //--DISTIBUTION Activity SAVE
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = '94E90001-A620-4624-9003-A64F0121D0D7';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = '94E90001-A620-4624-9003-A64F0121D0D7';
                DECLARE @workflowActivityResultID uniqueidentifier = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--DISTIBUTION Activity Copy
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = '94E90001-A620-4624-9003-A64F0121D0D7';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = '94E90001-A620-4624-9003-A64F0121D0D7';
                DECLARE @workflowActivityResultID uniqueidentifier = '47538F13-9257-4161-BCD0-AA7DEA897AE5';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--DISTIBUTION Activity SUBMIT
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = '94E90001-A620-4624-9003-A64F0121D0D7';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @workflowActivityResultID uniqueidentifier = '5445DC6E-72DC-4A6B-95B6-338F0359F89E';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--DISTIBUTION Activity TERMINATE
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = '94E90001-A620-4624-9003-A64F0121D0D7';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696';
                DECLARE @workflowActivityResultID uniqueidentifier = '53579F36-9D20-47D9-AC33-643D9130080B';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--Complete Distribution Activity Bulk Edit
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @workflowActivityResultID uniqueidentifier = '4F7E1762-E453-4D12-8037-BAE8A95523F7';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--Complete Distribution Activity Complete Routings
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @workflowActivityResultID uniqueidentifier = '8A68399F-D562-4A98-87C9-195D3D83A103';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--Complete Distribution Activity Save
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @workflowActivityResultID uniqueidentifier = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--Complete Distribution Activity Terminate
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696';
                DECLARE @workflowActivityResultID uniqueidentifier = '53579F36-9D20-47D9-AC33-643D9130080B';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            //--Completed Activity Save
            Sql(@"DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';
                DECLARE @sourceWorkflowActivityID uniqueidentifier = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC';
                DECLARE @destinationWorkflowActivityID uniqueidentifier = '9392ACEF-1AF3-407C-B19C-BAE88C389BFC';
                DECLARE @workflowActivityResultID uniqueidentifier = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D';

                IF NOT EXISTS (SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowID = @workflowID AND SourceWorkflowActivityID =  @sourceWorkflowActivityID AND DestinationWorkflowActivityID = @destinationWorkflowActivityID AND WorkflowActivityResultID = @workflowActivityResultID)
                BEGIN
                    Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
                            VALUES(@workflowID,  @sourceWorkflowActivityID, @destinationWorkflowActivityID, @workflowActivityResultID)
                END");

            Sql(@"DECLARE @ID uniqueidentifier = '40C16658-1157-4056-8FB3-D68D1FACFD87';
                DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';

                IF NOT EXISTS (SELECT NULL FROM WorkflowRoles WHERE ID = @ID AND WorkflowID = @workflowID)
                BEGIN
                    Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                    VALUES(@ID,@workflowID,'Request Creator','The Request Creator for the workflow.',1)
                END");

            Sql(@"DECLARE @ID uniqueidentifier = 'FBB7C47A-78CF-4171-9E9D-D50C96DED6B8';
                DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';

                IF NOT EXISTS (SELECT NULL FROM WorkflowRoles WHERE ID = @ID AND WorkflowID = @workflowID)
                BEGIN
                    Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                    VALUES(@ID,@workflowID,'Primary Analyst','The Primary Analyst of the workflow.',0)
                END");
            Sql(@"DECLARE @ID uniqueidentifier = '81959ADE-9C98-435E-92DA-2A63ABC246C9';
                DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';

                IF NOT EXISTS (SELECT NULL FROM WorkflowRoles WHERE ID = @ID AND WorkflowID = @workflowID)
                BEGIN
                    Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                    VALUES(@ID,@workflowID,'Requestor','The Requestor that initiated the request.',0)
                END");
            Sql(@"DECLARE @ID uniqueidentifier = '4934FBA5-6EC0-4297-8273-B13B282F216D';
                DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';

                IF NOT EXISTS (SELECT NULL FROM WorkflowRoles WHERE ID = @ID AND WorkflowID = @workflowID)
                BEGIN
                    Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                    VALUES(@ID,@workflowID,'Secondary Analyst','The Secondary Analyst of the workflow.',0)
                END");
            Sql(@"DECLARE @ID uniqueidentifier = '90767C4A-2C42-43AA-AE74-8EB913016E27';
                DECLARE @workflowID uniqueidentifier = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A';

                IF NOT EXISTS (SELECT NULL FROM WorkflowRoles WHERE ID = @ID AND WorkflowID = @workflowID)
                BEGIN
                    Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                    VALUES(@ID,@workflowID,'Scientific Lead','The Scientific Lead of the workflow.',0)
                END");
        }
        
        public override void Down()
        {
            Sql("Delete from WorkflowRoles WHERE WorkflowID = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A'");
            Sql("Delete from WorkflowActivityCompletionMaps WHERE WorkflowID = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A'");
            Sql("Delete from Workflows WHERE ID = '047AC2E4-3F74-40DB-BA9F-2DFA0FB3123A'");
            Sql("UPDATE Workflows SET Name = 'Distributed Regression', Description = 'Workflow for Distributed Regression' WHERE ID = 'E9656288-33FF-4D1F-BA77-C82EB0BF0192'");
            Sql(@"CREATE TRIGGER [dbo].[RequestDataMartResponsesInsert] 
	                ON  [dbo].[RequestDataMartResponses]
	                AFTER INSERT
                    AS 
                    BEGIN
	                    UPDATE RequestDataMartResponses SET Count = (SELECT COUNT(*) FROM RequestDataMartResponses r WHERE r.RequestDataMartID = RequestDataMartResponses.RequestDataMartID AND r.SubmittedOn < RequestDataMartResponses.SubmittedOn) + 1 WHERE RequestDataMartResponses.ID IN (SELECT ID FROM inserted)
                    END");
        }
    }
}
