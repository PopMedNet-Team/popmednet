namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModularProgramWorkflowMappings : DbMigration
    {
        public override void Up()
        {
            //make sure the View Status activity exists
            Sql(@"IF NOT EXISTS(SELECT ID FROM WorkflowActivities WHERE ID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55')
	INSERT INTO WorkflowActivities (ID, [Name], [Description], [Start], [End]) VALUES ('D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'View Status And Results', '<p>Review the DataMart routing statuses and request results.</p>
<p>Click <strong>Add DataMarts</strong> to route the request to a new DataMart or <strong>Remove DataMarts</strong> to cancel the request at the selected DataMarts. Click <strong>Resubmit</strong> to resubmit the request to the selected DataMarts.</p>
<p>Use the Comments and Documents tabs to view comments included upon completion of the Distribution stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 
<p>Click <strong>Complete Distribution</strong> to close distribution for this request and cancel all outstanding DataMart routings. Select a report status using the dropdown menu. If no reports will be uploaded for this request, select <strong>Not Applicable</strong> to complete the request workflow. Select <strong>Pending</strong> to advance to the Draft Report stage in the workflow. Upon indicating that a report is pending, you may optionally include a comment that will be transferred to the next stage.</p>
<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>', 0, 0)");
            
            //update the submit activity mapping
            Sql(@"UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' -- View Status And Results
WHERE WorkflowActivityResultID = '5445DC6E-72DC-4A6B-95B6-338F0359F89E' --Submit
AND SourceWorkflowActivityID = 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554' -- Distribute Request
AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' -- Modular Program");

            //add the mappings for available activities
            //Terminate workflow
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', '53579F36-9D20-47D9-AC33-643D9130080B', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'CC2E0001-9B99-4C67-8DED-A3B600E1C696')");
            //complete distribution, go to Prepare Draft
            Sql(@"INSERT INTO WorkflowActivityResults (ID, [Name], [Description]) VALUES ('EBEF0001-09B6-4BDE-89A2-A3F4012D282C', 'Complete Distribution', '')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'EBEF0001-09B6-4BDE-89A2-A3F4012D282C', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '9173A8E7-27C4-469D-853D-69A78501A522')");
            //resubmit (goes back to to self but with routings resubmitted)
            Sql(@"INSERT INTO WorkflowActivityResults (ID, [Name], [Description]) VALUES ('5C5E0001-10A6-4992-A8BE-A3F4012D5FEB', 'Redistribute', '')");
            Sql(@"INSERT INTO WorkflowActivityCompletionMaps (WorkflowID, WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', '5C5E0001-10A6-4992-A8BE-A3F4012D5FEB', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55')");
            //update the Redistribute activity mapping for submit draft report activity
            Sql(@"UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' -- View Status And Results
WHERE WorkflowActivityResultID = 'B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA' -- Redistribute
AND SourceWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' -- Submit Draft Report
AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' -- Modular Program");
            //update submit final report to be able to go back to draft report step
            Sql(@"UPDATE WorkflowActivityCompletionMaps SET DestinationWorkflowActivityID = '9173A8E7-27C4-469D-853D-69A78501A522' -- Prepare Draft Report
WHERE WorkflowActivityResultID = 'ECCBF404-B3BA-4C5E-BB6E-388725938DC3' -- Draft Review
AND SourceWorkflowActivityID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' -- Submit Final Report
AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' -- Modular Program");
            //remove all mappings for Modular Workflow mapping to View Responses source activity
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D' AND SourceWorkflowActivityID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'");
            //remove unused result items
            Sql(@"DELETE FROM WorkflowActivityResults WHERE NOT EXISTS(SELECT NULL FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = WorkflowActivityResults.ID)");
        }
        
        public override void Down()
        {
        }
    }
}
