namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTaskNameDecription : DbMigration
    {
        public override void Up()
        {
            Sql(@"-- Default Workflow Tasks and Descriptions
-- Create request
UPDATE WorkflowActivities SET Name = 'Create Request', [Description] = 'Create a draft request'  WHERE ID = 'C1380001-4524-49BA-B4B6-A3B5013A3343' 
-- Review request
UPDATE WorkflowActivities SET Name = 'Review Request', [Description] = 'Review request and click Approve to distribute the request to the selected DataMarts or click Reject to terminate the request.' 
-- View Status and Results
UPDATE WorkflowActivities SET Name = 'View Responses', [Description] = ''
-- Upload Response
UPDATE WorkflowActivities SET Name = 'Respond to Request', [Description] = ''
-- ReviewResponse
UPDATE WorkflowActivities SET Name = 'Review Responses', [Description] = ''

--
-- Summary Workflow Tasks and Descriptions
--
-- Create request
UPDATE WorkflowActivities SET Name = 'Request Form', [Description] = '<p>Complete the request form below. Additional information and instructions may be included by attaching one or more files to this page.</p> 

<p>Click <strong>Submit</strong> to send the request form for review. Upon submission, you may optionally include a comment for the reviewer.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896' 

-- Review Request Form
UPDATE WorkflowActivities SET Name = 'Review Request Form', [Description] = '<p>Review the request and any additional documents below. Enter request metadata in the fields provided and modify the request parameters as needed.</p>

<p>Use the Comments and Documents tabs to view comments from the requester and communicate and share documents with other users participating in and viewing this stage.</p>

<p>Click <strong>Approve</strong> to approve the request form and advance to the Distribution stage in the workflow. Upon approval, you may optionally include a comment that will be transferred to the next stage.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0' 

-- Distribute Request
UPDATE WorkflowActivities SET Name = 'Distribute Request', [Description] = '<p>Select the DataMart(s) that the request will be distributed to.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Request Form Review stage and to communicate and share additional documents with other users participating in and viewing this stage.</p>

<p>Click <strong>Submit</strong> to distribute the request to the specified DataMarts or <strong>Modify Request</strong> to return to the Request Form Review stage in the workflow. Upon submission or selecting modify request, you may optionally include a comment that will be transferred to the next stage.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = '752B83D7-2190-49DF-9BAE-983A7880A899' 

-- Respond to Request
UPDATE WorkflowActivities SET Name = 'Respond to Request', [Description] = '<p>Review the request and your DataMart routing status(es). Respond to the request using your DataMart Client.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = '303C1C1B-A330-41DB-B3B6-4D7C02D02C8C' 

-- Review Response
UPDATE WorkflowActivities SET Name = 'Review Results', [Description] = '<p>Review the results from the completed DataMart routing(s).</p> 

<p>Click <strong>Group</strong> to aggregate results from multiple selected DataMarts or <strong>Ungroup</strong> to disaggregate selected grouped results, if applicable.</p> 

<p>Click <strong>Approve</strong> to approve the results from the selected DataMart(s) and release them to the next stage in the workflow or <strong>Reject</strong> to reject the results and not release them. Upon approval or rejection, you may optionally include a comment that will be transfered to the next stage.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = '9CDD7176-9361-4585-B79C-438645DA45BE' 

-- View Responses
UPDATE WorkflowActivities SET Name = 'Respond to Request', [Description] = '<p>Review the DataMart routing statuses and request results.</p> 

<p>Click <strong>Add DataMarts</strong> to route the request to a new DataMart or <strong>Remove DataMarts</strong> to cancel the request at the selected DataMarts. Click <strong>Resubmit</strong> to resubmit the request to the selected DataMarts.</p>

<p>Use the Comments and Documents tabs to view comments included upon completion of the Distribution stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Complete Distribution</strong> to close distribution for this request and cancel all outstanding DataMart routings. Select a report status using the dropdown menu. If no reports will be uploaded for this request, select <strong>Not Applicable</strong> to complete the request workflow. Select <strong>Pending</strong> to advance to the Draft Report stage in the workflow. Upon indicating that a report is pending, you may optionally include a comment that will be transferred to the next stage.</p> 
 
<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B' 

-- Prepate Draft Report
UPDATE WorkflowActivities SET Name = 'Submit Draft Report', [Description] = '<p>Attach draft report documents below. Only files attached to this page will be submitted for review.</p>

<p>Use the Comments and Documents tabs to view comments included upon completion of the Complete Distribution stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Submit</strong> to submit the draft report documents for review or <strong>Redistribute</strong> to return to the Distribution stage in the workflow. Upon submission or selecting redistribution, you may optionally include a comment that will be transferred to the next stage.</p>

<p>To end the workflow for this request, select <strong>Not Applicable</strong> in the Report Status dropdown menu.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = '9173A8E7-27C4-469D-853D-69A78501A522' 

-- Review Draft Report
UPDATE WorkflowActivities SET Name = 'Review Draft Report', [Description] = '<p>Review the draft report documents below.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Draft Report stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Approve</strong> to approve the draft report documents and advance to the Final Report stage in the workflow or <strong>Return for Revision</strong> to return the request to the Draft Report stage. Upon approval or return for revision, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>To end the workflow for this request, select <strong>Not Applicable</strong> in the Report Status dropdown menu.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = '2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81' 

-- Prepare Final Report
UPDATE WorkflowActivities SET Name = 'Submit Final Report', [Description] = '<p>Attach final report documents below. Only files attached to this page will be submitted for review.</p>

<p>Use the Comments and Documents tabs to view comments included upon completion of the Draft Report Review stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p/>Click <strong>Submit</strong> to submit the final report documents for review or <strong>Reopen Draft Report Review</strong> to return to the Draft Review stage in the workflow. Upon submission or selecting to reopen draft review, you may optionally include a comment that will be transferred to the next stage.</p>

<p>To end the workflow for this request, select <strong>Not Applicable</strong> in the Report Status dropdown menu.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a>.</p>'  WHERE ID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208' 

-- Review Final Report
UPDATE WorkflowActivities SET Name = 'Review Final Report', [Description] = '<p>Review the final report documents below.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Final Report stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Approve</strong> to approve the final report documents and complete the workflow for this request or <strong>Return for Revision</strong> to return the request to the Draft Report stage. Upon return for revision, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '2E7A3263-C87E-47BA-AC35-A78ABF8FE606' 

--
-- Modular Program Workflow Tasks and Descriptions
--
-- Create request
UPDATE WorkflowActivities SET Name = 'Request Form', [Description] = '<p>Complete the request form below. Additional information and instructions may be included by attaching one or more files to this page.</p> 

<p>Click <strong>Submit</strong> to send the request form for review. Upon submission, you may optionally include a comment for the reviewer.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '0321E17F-AA1F-4B23-A145-85B159E74F0F' 

-- Review request
UPDATE WorkflowActivities SET Name = 'Review Request Form', [Description] = '<p>Review the request form and any additional documents below.</p> 

<p>Use the Comments and Documents tabs to view comments from the requester and communicate and share documents with other users participating in and viewing this stage.</p> 

<p>Enter request metadata in the fields provided.</p>

<p>Click <strong>Approve</strong> to approve the request form and advance to the Working Specifications stage in the workflow. Upon approval, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'A96FBAD0-8FD8-4D10-8891-D749A71912F8' 

-- Working Specification
UPDATE WorkflowActivities SET Name = 'Submit Working Specifications', [Description] = '<p>Attach working specification documents below. Only files attached to this page will be submitted for review.</p>

<p>Use the Comments and Documents tabs to view comments included upon completion of the Request Form Review stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Submit for Internal Review</strong> or <strong>Submit for Final Review</strong> to advance the request to that stage in the workflow. Upon submission, you may optionally include a comment that will be transferred to the next stage.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '31C60BB1-2F6A-423B-A7B7-B52626FD9E97' 

-- Working Specification Review
UPDATE WorkflowActivities SET Name = 'Review Working Specifications', [Description] = '<p>Review the working specifications documents below.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Working Specifications stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Approve</strong> to approve the working specification documents and advance the request to the Specifications stage in the workflow or <strong>Return for Revision</strong> to return the request to the Working Specifications stage. Upon approval or return for revision, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'C8891CFD-80BF-4F71-90DE-6748BF71566C' 

-- Specification
UPDATE WorkflowActivities SET Name = 'Submit Specifications', [Description] = '<p>Attach specification documents below. Only files attached to this page will be submitted for review.</p>

<p>Use the Comments and Documents tabs to view comments included upon completion of Working Specifications Review stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Submit</strong> to submit the specifications for review. Upon submission, you may optionally include a comment that will be transferred to the next stage.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'C3B13067-3B9D-41E4-8D4A-7114A6E81930' 

-- Specification Review
UPDATE WorkflowActivities SET Name = 'Review Specifications', [Description] = '<p>Review the specifications documents below.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Specifications stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Approve</strong> to approve the specification documents and advance to the Pre-Distribution stage in the workflow or <strong>Return for Revision</strong> to return the request to the Specifications stage. Upon approval or return for revision, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77' 

-- Pre-distribution Testing
UPDATE WorkflowActivities SET Name = 'Perform Pre-Distribution Testing', [Description] = '<p>Attach files related to pre-distribution testing below. Only files attached to this page will be submitted for review.</p>

<p>Use the Comments and Documents tabs to view comments included upon completion of the Specifications Review stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Submit</strong> to submit these documents for review or <strong>Modify Specifications</strong> to return to Specifications stage in the workflow. Upon submission or returning to Specifications, you may optionally include a comment that will be transferred to the next stage.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7' 

-- Pre-distribution Testing Review
UPDATE WorkflowActivities SET Name = 'Review Pre-Distribution Testing', [Description] = '<p>Review the pre-distribution testing files below.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Pre-Distribution Testing stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Approve</strong> to approve the testing and advance to the Distribution stage in the workflow or <strong>Reject</strong> to return to the Pre-Distribution Testing stage. Upon approval or rejection, you may optionally include a comment that will be transferred to the next stage.</p>  

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'EA69E5ED-6029-47E8-9B45-F0F00B07FDC2' 

-- Distribute Request
UPDATE WorkflowActivities SET Name = 'Distribute Request', [Description] = '<p>Complete request metadata in the fields provided and complete the request parameters and/or upload documents for distribution. Select the DataMart(s) that the request will be distributed to. Only files attached to this page will be distributed to the specified DataMart(s).</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Pre-Distribution Testing Review stage and to communicate and share additional documents with other users participating in and viewing this stage.</p>  

<p>Click <strong>Submit</strong> to distribute the request to the specified DataMarts or <strong>Retest</strong> to return to the Pre-Distribution Testing stage in the workflow. Upon submission or selecting retest, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554' 

-- Respond to Request
UPDATE WorkflowActivities SET Name = 'Respond to Request', [Description] = '<p>Review the request and your DataMart(s) routing status(es). Respond to the request using your DataMart Client.</p>

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'D51D0D4F-41F7-4208-8722-6D71B23DE2F9' 

-- Review Responses
UPDATE WorkflowActivities SET Name = 'Review Results', [Description] = '<p>Review the results from the completed DataMart routing(s).</p> 

<p>Click <strong>Group</strong> to aggregate results from multiple selected DataMarts or <strong>Ungroup</strong> to disaggregate selected grouped results, if applicable.</p> 

<p>Click <strong>Approve</strong> to approve the results from the selected DataMart(s) and release them to the next stage in the workflow or <strong>Reject</strong> to reject the results and not release them. Upon approval or rejection, you may optionally include a comment that will be transfered to the next stage.

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '7B4EB88B-1295-45B9-AE19-5BC45E98C985' 

-- View Status and Results
UPDATE WorkflowActivities SET Name = 'Respond to Request', [Description] = '<a>Review the DataMart routing statuses and request results.</a> 

<a>Click <strong>Add DataMarts</strong> to route the request to a new DataMart or <strong>Remove DataMarts</strong> to cancel the request at the selected DataMarts. Click <strong>Resubmit</strong> to resubmit the request to the selected DataMarts.</a>

<a>Use the Comments and Documents tabs to view comments included upon completion of the Distribution stage and to communicate and share additional documents with other users participating in and viewing this stage.</a> 

<a>Click <strong>Complete Distribution</strong> to close distribution for this request and cancel all outstanding DataMart routings. Select a report status using the dropdown menu. If no reports will be uploaded for this request, select <strong>Not Applicable</strong> to complete the request workflow. Select Pending to advance to the Draft Report stage in the workflow. Upon indicating that a report is pending, you may optionally include a comment that will be transferred to the next stage.</a> 
 
<a>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' 

-- Prepare Draft Report
UPDATE WorkflowActivities SET Name = 'Submit Draft Report', [Description] = '<p>Attach draft report documents below. Only files attached to this page will be submitted for review.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Complete Distribution stage and to communicate and share additional documents with other users participating in and viewing this stage.</p>  

<p>Click <strong>Submit</strong> to submit the draft report documents for review or <strong>Redistribute</strong> to return to the Distribution stage in the workflow. Upon submission or selecting redistribution, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>To end the workflow for this request, select <strong>Not Applicable</strong> in the Report Status dropdown menu.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '43AB48FD-A400-4C0D-92C9-DD2415A5D5B6' 

-- Draft Report Review
UPDATE WorkflowActivities SET Name = 'Review Draft Report', [Description] = '<p>Review the draft report documents below.</p>  

<p>Use the Comments and Documents tabs to view comments included upon completion of the Draft Report stage and to communicate and share additional documents with other users participating in and viewing this stage.</p>  

<p>Click <strong>Approve</strong> to approve the draft report documents and advance to the Final Report stage in the workflow or <strong>Return for Revision</strong> to return the request to the Draft Report stage. Upon approval or return for revision, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = 'C80810A3-CF10-4941-854A-A7E2052A5EBA' 

-- Prepare Final Report
UPDATE WorkflowActivities SET Name = 'Submit Final Report', [Description] = '<p>Attach final report documents below. Only files attached to this page will be submitted for review.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Draft Report Review stage and to communicate and share additional documents with other users participating in and viewing this stage.</p>  

<p>Click <strong>Submit</strong> to submit the final report documents for review or <strong>Reopen Draft Report Review</strong> to return to the Draft Review stage in the workflow. Upon submission or selecting to reopen draft review, you may optionally include a comment that will be transferred to the next stage.</p> 

<p>To end the workflow for this request, select <strong>Not Applicable</strong> in the Report Status dropdown menu.</p> 

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9' 

-- Final Report Review
UPDATE WorkflowActivities SET Name = 'Review Final Report', [Description] = '<p>Review the final report documents below.</p> 

<p>Use the Comments and Documents tabs to view comments included upon completion of the Final Report stage and to communicate and share additional documents with other users participating in and viewing this stage.</p> 

<p>Click <strong>Approve</strong> to approve the final report documents and complete the workflow for this request or <strong>Return for Revision</strong> to return the request to the Draft Report stage. Upon return for revision, you may optionally include a comment that will be transferred to the next stage.</p>  

<p>For more information, please see <a href=""https://popmednet.atlassian.net/wiki/display/DOC/Request+Workflow"">PopMedNet User''s Guide: Request Workflow</a></p>'  WHERE ID = '4CCE18C8-CABF-4D22-88AB-611CD560DBF8' 

-- Use common Terminate activity for Default, Summary, and MP workflows
UPDATE [dbo].[WorkflowActivityCompletionMaps]
   SET [DestinationWorkflowActivityID] = 'CC2E0001-9B99-4C67-8DED-A3B600E1C696'
 WHERE [DestinationWorkflowActivityID] in ('5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4', 'B34013F5-88C5-4D79-997B-6525D740E0CB')

 DELETE [dbo].[WorkflowActivities] WHERE [ID] in ('5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4', 'B34013F5-88C5-4D79-997B-6525D740E0CB') 

 UPDATE [dbo].[WorkflowActivityCompletionMaps]
   SET [WorkflowActivityResultID] = 'DFF3000B-B076-4D07-8D83-05EDE3636F4D'
 WHERE [WorkflowActivityResultID] in ('6410D92A-2AC8-43F8-A06F-D4EE3274BC81', '1E6E14B1-0D41-4E2B-B6E3-289F742296DA', 'ADCA656A-8052-4F32-9AC5-5617870C53CE', '3512F473-3E1B-460E-9EAF-12B8DA986ACD', 'A225E59A-656C-4A63-B5E4-82416D6351D8', '04338B5D-9507-4E04-9701-37F55ACB2966', 'A54277C0-F08D-4401-AE0D-9F18D2642025', 'FB23A7FF-0A38-4F5E-9457-CCCD74170219', '535E94E5-4D2E-438C-B9F1-37E5B6E2EF02', '495288DD-8B92-4240-85B6-25A9144A8192') 

 DELETE [dbo].[WorkflowActivityResults] WHERE [ID] in ('6410D92A-2AC8-43F8-A06F-D4EE3274BC81', '1E6E14B1-0D41-4E2B-B6E3-289F742296DA', 'ADCA656A-8052-4F32-9AC5-5617870C53CE', '3512F473-3E1B-460E-9EAF-12B8DA986ACD', 'A225E59A-656C-4A63-B5E4-82416D6351D8', '04338B5D-9507-4E04-9701-37F55ACB2966', 'A54277C0-F08D-4401-AE0D-9F18D2642025', 'FB23A7FF-0A38-4F5E-9457-CCCD74170219', '535E94E5-4D2E-438C-B9F1-37E5B6E2EF02', '495288DD-8B92-4240-85B6-25A9144A8192') 

 UPDATE [dbo].[WorkflowActivityCompletionMaps]
   SET [WorkflowActivityResultID] = '53579F36-9D20-47D9-AC33-643D9130080B'
 WHERE [WorkflowActivityResultID] in ('61939A65-5E6B-4B8F-8F0F-4D76DFAD2854', 'BC2C554C-97CD-405B-AA55-91C01006C93C', '79F29AF1-A0B8-469D-B7B8-7ED63EF4E55B', '93473E52-0AB2-41FC-A71A-461A3D9B0D61', '0704481B-A42C-4335-B17C-743BF41B03FB', '7905678A-3CC1-4FFA-85F1-3514B232B694', 'B7CB6D9D-1663-4B42-A709-1C6822543C9F') 

 DELETE [dbo].[WorkflowActivityResults] WHERE [ID] in ('61939A65-5E6B-4B8F-8F0F-4D76DFAD2854', 'BC2C554C-97CD-405B-AA55-91C01006C93C', '79F29AF1-A0B8-469D-B7B8-7ED63EF4E55B', '93473E52-0AB2-41FC-A71A-461A3D9B0D61', '0704481B-A42C-4335-B17C-743BF41B03FB', '7905678A-3CC1-4FFA-85F1-3514B232B694', 'B7CB6D9D-1663-4B42-A709-1C6822543C9F')
");
        }
        
        public override void Down()
        {
            Sql(@"");
        }
    }
}
