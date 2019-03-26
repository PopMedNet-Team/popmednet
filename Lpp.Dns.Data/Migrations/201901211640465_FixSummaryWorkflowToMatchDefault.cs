namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSummaryWorkflowToMatchDefault : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM WorkflowActivityCompletionMaps Where SourceWorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' OR DestinationWorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'");
            Sql("DELETE FROM LogsNewRequestSubmitted WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsRequestDataMartAddedRemoved WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsRequestDataMartMetadataChange WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsRequestMetadataChange WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsRequestDocumentChange WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsRequestStatusChange WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsRoutingStatusChange WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsSubmittedRequestAwaitsResponse WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsSubmittedRequestNeedsApproval WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsTaskChange WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsTaskReminder WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM LogsUploadedResultNeedsApproval WHERE TaskID IN(SELECT ID FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')");
            Sql("DELETE FROM Tasks WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'");
            Sql("DELETE FROM Requests WHERE WorkflowActivityID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'");
            Sql("DELETE FROM WorkflowActivities Where ID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C'");

            Sql("UPDATE WorkflowActivities SET Name = 'New Request' WHERE ID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896'");
            Sql("UPDATE WorkflowActivities SET Name = 'Request Review' WHERE ID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0'");

            Sql("INSERT INTO WorkflowActivityResults (ID,Name,Description,Uri) VALUES ('34285004-D933-4B27-AC7C-52F65B01891F', 'Submit to Distribution', NULL, NULL)");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID, WorkflowID)
                VALUES('34285004-D933-4B27-AC7C-52F65B01891F', '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896', '752B83D7-2190-49DF-9BAE-983A7880A899', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql("UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896' WHERE WorkflowActivityResultID = '94513F48-4C4A-4449-BA95-5B0CD81DB642' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '752B83D7-2190-49DF-9BAE-983A7880A899'");
        }
        
        public override void Down()
        {
            Sql(@"INSERT INTO WorkflowActivities (ID,Name,Description,Start,[End]) VALUES ('303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','Respond to Request','<p>Review the request and your DataMart routing status(es). Respond to the request using your DataMart Client.</p>
               <p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>',0,0)");

            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID,SourceWorkflowActivityID,DestinationWorkflowActivityID,WorkflowID) 
                VALUES ('DFF3000B-B076-4D07-8D83-05EDE3636F4D','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID,SourceWorkflowActivityID,DestinationWorkflowActivityID,WorkflowID) 
                VALUES ('668EE9C7-4930-423E-AA9E-150B646121F4','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowActivityResultID,SourceWorkflowActivityID,DestinationWorkflowActivityID,WorkflowID) 
                VALUES ('D0A0924F-F4B5-43BF-89A6-C7F32E764735','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql("UPDATE WorkflowActivities SET Name = 'Request Form' WHERE ID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896'");
            Sql("UPDATE WorkflowActivities SET Name = 'Request Form Review' WHERE ID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0'");

            Sql("DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '34285004-D933-4B27-AC7C-52F65B01891F'");
            Sql("DELETE FROM WorkflowActivityResults WHERE ID = '34285004-D933-4B27-AC7C-52F65B01891F'");

            Sql("UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0' WHERE WorkflowActivityResultID = '94513F48-4C4A-4449-BA95-5B0CD81DB642' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8' AND SourceWorkflowActivityID = '752B83D7-2190-49DF-9BAE-983A7880A899'");
        }
    }
}
