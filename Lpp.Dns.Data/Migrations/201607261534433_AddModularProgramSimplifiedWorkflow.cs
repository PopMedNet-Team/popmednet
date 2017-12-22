namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModularProgramSimplifiedWorkflow : DbMigration
    {
        public override void Up()
        {
            //--Adding Modular Program Simplified Workflow
            Sql(@"INSERT INTO Workflows(ID, Name, Description)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'Simple Modular Program', 'This is a simplified Version of the Modular Program Workflow')");

            //--Add New Activity DISTIBUTION with Start
            Sql(@"Insert INTO WorkflowActivities(ID,Name,Description,[Start],[End])
                    VALUES('94E90001-A620-4624-9003-A64F0121D0D7',
                            'Distribution',
                            'This is for Submitting Requests to DataMarts',
                            1,
                            0)");
            
            //--DISTIBUTION Activity SAVE
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', '94E90001-A620-4624-9003-A64F0121D0D7', '94E90001-A620-4624-9003-A64F0121D0D7', 'DFF3000B-B076-4D07-8D83-05EDE3636F4D')");

            //--DISTIBUTION Activity SUBMIT
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', '94E90001-A620-4624-9003-A64F0121D0D7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5445DC6E-72DC-4A6B-95B6-338F0359F89E')");

            //--DISTIBUTION Activity TERMINATE
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', '94E90001-A620-4624-9003-A64F0121D0D7', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '53579F36-9D20-47D9-AC33-643D9130080B')");



            //--Complete Distribution Activity Save
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'DFF3000B-B076-4D07-8D83-05EDE3636F4D')");
            
            //--Complete Distribution Activity Complete Workflow
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', 'E93CED3B-4B55-4991-AF84-07058ABE315C')");

            //--Complete Distribution Activity Group
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '49F9C682-9FAD-4AE5-A2C5-19157E227186')");

            //--Complete Distribution Activity Terminate
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696', '53579F36-9D20-47D9-AC33-643D9130080B')");

            //--Complete Distribution Activity Edit Routing Status
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C')");

            //--Complete Distribution Activity Remove DataMarts
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5E010001-1353-44E9-9204-A3B600E263E9')");

            //--Complete Distribution ACtivtiy Redistribute
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5C5E0001-10A6-4992-A8BE-A3F4012D5FEB')");

            //--Complete Distribution Activity Ungroup
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '7821FC45-9FD5-4597-A405-B021E5ED14FA')");

            //--Complete Distribution Activity Bulk Edit
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '4F7E1762-E453-4D12-8037-BAE8A95523F7')");

            //--Complete Distribution Activity Routings Add DataMarts
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '15BDEF13-6E86-4E0F-8790-C07AE5B798A8')");

            //--Completed Activity Save
            Sql(@"Insert INTO WorkflowActivityCompletionMaps(WorkflowID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowActivityResultID)
            VALUES('931C0001-787C-464D-A90F-A64F00FB23E7', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', '9392ACEF-1AF3-407C-B19C-BAE88C389BFC', 'DFF3000B-B076-4D07-8D83-05EDE3636F4D')");

            //Add Requestor Role
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('B4380001-CA22-48C4-AFE0-A64F0108ED56','931C0001-787C-464D-A90F-A64F00FB23E7','Request Creator','The Request Creator for the workflow.',1)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('EBA40001-97E0-4FAB-B40D-A64F01098754','931C0001-787C-464D-A90F-A64F00FB23E7','Primary Analyst','The Primary Analyst of the workflow.',0)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('A3560001-4368-47D3-B146-A64F0109AAA1','931C0001-787C-464D-A90F-A64F00FB23E7','Requestor','The Requestor that initiated the request.',0)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('65860001-DD6E-42A1-819C-A64F0109DB69','931C0001-787C-464D-A90F-A64F00FB23E7','Secondary Analyst','The Secondary Analyst of the workflow.',0)");
            Sql(@"Insert INTO WorkflowRoles(ID,WorkflowID,Name,Description,IsRequestCreator)
                VALUES('095F0001-9C24-423D-B242-A64F010A0202','931C0001-787C-464D-A90F-A64F00FB23E7','Scientific Lead','The Scientific Lead of the workflow.',0)");

        }

        public override void Down()
        {
            Sql("DELETE FROM WorkflowRoles Where WorkflowID = '931C0001-787C-464D-A90F-A64F00FB23E7'");
            Sql("DELETE FROM WorkflowActivityCompletionMaps Where WorkflowID = '931C0001-787C-464D-A90F-A64F00FB23E7'");
            Sql("DELETE FROM WorkflowActivities WHERE ID = '94E90001-A620-4624-9003-A64F0121D0D7'");
            Sql("DELETE FROM Workflows Where ID = '931C0001-787C-464D-A90F-A64F00FB23E7'");
        }
    }
}
