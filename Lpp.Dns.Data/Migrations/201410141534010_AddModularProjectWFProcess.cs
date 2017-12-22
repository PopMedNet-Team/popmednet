namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddModularProjectWFProcess : DbMigration
    {
        public override void Up()
        {

            Sql(@"INSERT INTO Workflows (ID, Name, Description) VALUES ('5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Modular Program', 'Workflow for Modular Program Queries.')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [End]) VALUES ('5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Terminate Request', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [Start]) VALUES ('0321E17F-AA1F-4B23-A145-85B159E74F0F', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Draft Request', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('A96FBAD0-8FD8-4D10-8891-D749A71912F8', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Review Request', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('31C60BB1-2F6A-423B-A7B7-B52626FD9E97', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Working Specification', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('C8891CFD-80BF-4F71-90DE-6748BF71566C', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Working Specification Review', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('C3B13067-3B9D-41E4-8D4A-7114A6E81930', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Specification', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('948B60F0-8CE5-4B14-9AD6-C50EC37DFC77', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Specification Review', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Pre-distribution Testing', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('EA69E5ED-6029-47E8-9B45-F0F00B07FDC2', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Pre-distribution Testing Review', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('E6CCD61B-81C4-4217-A958-ADAFB5EE5554', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Distribute Request', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [Start]) VALUES ('D51D0D4F-41F7-4208-8722-6D71B23DE2F9', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Respond to Request', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [End]) VALUES ('7B4EB88B-1295-45B9-AE19-5BC45E98C985', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Review DataMart Responses', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'View Status and Results', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('43AB48FD-A400-4C0D-92C9-DD2415A5D5B6', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Prepare Draft Report', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('C80810A3-CF10-4941-854A-A7E2052A5EBA', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Review Draft Report', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Prepare Final Report', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('4CCE18C8-CABF-4D22-88AB-611CD560DBF8', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Review Final Report', '')

-- CREATE REQUEST TASK
-- Delete a request that has not been submitted: create request => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('7E8661F2-E540-4E91-A3CF-982DB52EF965', 'Delete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('7E8661F2-E540-4E91-A3CF-982DB52EF965','0321E17F-AA1F-4B23-A145-85B159E74F0F','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Submit a request for approval: create request => request approval
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('6248E8B1-7C7C-4959-993F-352C722821A6', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('6248E8B1-7C7C-4959-993F-352C722821A6','0321E17F-AA1F-4B23-A145-85B159E74F0F','A96FBAD0-8FD8-4D10-8891-D749A71912F8')

-- REVIEW REQUEST FORM TASK
-- Terminate a submitted request: review request form => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('546164B4-0D6D-4C26-868B-07280F627818', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('546164B4-0D6D-4C26-868B-07280F627818','A96FBAD0-8FD8-4D10-8891-D749A71912F8','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Approve a submitted request: review request form => distribution
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('DEB04531-1635-4B32-AB0F-14C1CCF9BAFB', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('DEB04531-1635-4B32-AB0F-14C1CCF9BAFB','A96FBAD0-8FD8-4D10-8891-D749A71912F8','31C60BB1-2F6A-423B-A7B7-B52626FD9E97')

-- WORKING SPECIFICATION TASK
-- Terminate a working specification: working specification => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('0704481B-A42C-4335-B17C-743BF41B03FB', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('0704481B-A42C-4335-B17C-743BF41B03FB','31C60BB1-2F6A-423B-A7B7-B52626FD9E97','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Submit a working specification for internal review: working specification => submit for internal review
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('2BEF97A9-1A3A-46F8-B1D1-7E9E6F6F902A', 'Working Specification Review', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('2BEF97A9-1A3A-46F8-B1D1-7E9E6F6F902A','31C60BB1-2F6A-423B-A7B7-B52626FD9E97','C8891CFD-80BF-4F71-90DE-6748BF71566C')
-- Submit a working specification for final review: working specification => submit for final review
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('14B7E8CF-4CF2-4C3D-A97E-E69C5D090FC0', 'Specification Review', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('14B7E8CF-4CF2-4C3D-A97E-E69C5D090FC0','31C60BB1-2F6A-423B-A7B7-B52626FD9E97','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77')

-- WORKING SPECIFICATION REVIEW TASK
--  Approve a working specification: working specification => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('982C4DCC-AB1C-4F87-83BB-E09FA8270C17', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('982C4DCC-AB1C-4F87-83BB-E09FA8270C17','C8891CFD-80BF-4F71-90DE-6748BF71566C','C3B13067-3B9D-41E4-8D4A-7114A6E81930')
-- Reject a working specification: working specification => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('A95899AC-F4F6-41AB-AD4B-D41E05563486', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('A95899AC-F4F6-41AB-AD4B-D41E05563486','C8891CFD-80BF-4F71-90DE-6748BF71566C','C8891CFD-80BF-4F71-90DE-6748BF71566C')

-- SPECIFICATION TASK
-- Terminate a specification: specification => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('93473E52-0AB2-41FC-A71A-461A3D9B0D61', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('93473E52-0AB2-41FC-A71A-461A3D9B0D61','C3B13067-3B9D-41E4-8D4A-7114A6E81930','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Submit a specification : specification => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('C8260E90-2C8B-435A-85C8-372B021C3E9F', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('C8260E90-2C8B-435A-85C8-372B021C3E9F','C3B13067-3B9D-41E4-8D4A-7114A6E81930','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77')
-- Modify specification: specification => modify working specifications
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('E57E6B65-140F-452B-95FF-04BDB16BCD2D', 'Modify', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('E57E6B65-140F-452B-95FF-04BDB16BCD2D','C3B13067-3B9D-41E4-8D4A-7114A6E81930','31C60BB1-2F6A-423B-A7B7-B52626FD9E97')

-- SPECIFICATION REVIEW TASK
-- Approve a specification: specification => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('74294C54-05C2-4F97-BA35-FAC7C7E6F61A', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('74294C54-05C2-4F97-BA35-FAC7C7E6F61A','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77','49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7')
-- Reject a specification: specification => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('AD6A9E17-936F-431A-A5D6-97B37B7C0796', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('AD6A9E17-936F-431A-A5D6-97B37B7C0796','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77','31C60BB1-2F6A-423B-A7B7-B52626FD9E97')


-- PRE-DISTRIBUTION TESTING TASK
-- Terminate a specification: specification => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('7905678A-3CC1-4FFA-85F1-3514B232B694', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('7905678A-3CC1-4FFA-85F1-3514B232B694','49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Submit a specification : specification => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('8D035265-44EF-40AE-A1CD-30C9EF9871DB', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('8D035265-44EF-40AE-A1CD-30C9EF9871DB','49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7','948B60F0-8CE5-4B14-9AD6-C50EC37DFC77')
-- Modify specification: specification => modify working specifications
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('15F10745-0AAE-4EBB-8D8C-D38E85534EC3', 'Modify', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('15F10745-0AAE-4EBB-8D8C-D38E85534EC3','49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7','31C60BB1-2F6A-423B-A7B7-B52626FD9E97')

-- PRE-DISTRIBUTION TESTING REVIEW TASK
-- Approve a specification: specification => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('71AC013A-CE8E-419E-A1F0-67BD3B0777EF', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('71AC013A-CE8E-419E-A1F0-67BD3B0777EF','EA69E5ED-6029-47E8-9B45-F0F00B07FDC2','E6CCD61B-81C4-4217-A958-ADAFB5EE5554')
-- Reject a specification: specification => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('C3AE5DF8-DD50-4587-91E1-DD769237F34B', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('C3AE5DF8-DD50-4587-91E1-DD769237F34B','EA69E5ED-6029-47E8-9B45-F0F00B07FDC2','49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7')

-- DISTRIBUTE REQUEST TASK
-- Terminate a submitted request: distribution => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('79F29AF1-A0B8-469D-B7B8-7ED63EF4E55B', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('79F29AF1-A0B8-469D-B7B8-7ED63EF4E55B','E6CCD61B-81C4-4217-A958-ADAFB5EE5554','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Distribute a submitted request: distribution => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('5445DC6E-72DC-4A6B-95B6-338F0359F89E', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('5445DC6E-72DC-4A6B-95B6-338F0359F89E','E6CCD61B-81C4-4217-A958-ADAFB5EE5554','D51D0D4F-41F7-4208-8722-6D71B23DE2F9')
-- Modify a submitted request: distribution => modify
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('E970F48E-A072-41C1-B8C3-CC34C5826A46', 'Modify Specification', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('E970F48E-A072-41C1-B8C3-CC34C5826A46','E6CCD61B-81C4-4217-A958-ADAFB5EE5554','31C60BB1-2F6A-423B-A7B7-B52626FD9E97')

--
-- SUB WORKFLOW
--
-- DATA PARTNER RESPOND TO A REQUEST TASK
-- Upload a response to a submited request: respond => upload
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('FE131FA0-8F8F-4B17-9594-1F0C14D44429', 'Upload Response', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('FE131FA0-8F8F-4B17-9594-1F0C14D44429','D51D0D4F-41F7-4208-8722-6D71B23DE2F9','7B4EB88B-1295-45B9-AE19-5BC45E98C985')
-- Reject a response to a submited request: respond => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('463E51A5-20F9-4FC3-AE5D-D83D07B30C74', 'Reject Response', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('463E51A5-20F9-4FC3-AE5D-D83D07B30C74','D51D0D4F-41F7-4208-8722-6D71B23DE2F9','7B4EB88B-1295-45B9-AE19-5BC45E98C985')

-- DATA PARTNER RESPONSE REVIEW TASK
-- Approve an uploaded response: response request => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('77B57048-68BD-47A9-B280-E8A9C7418BD1', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('77B57048-68BD-47A9-B280-E8A9C7418BD1','7B4EB88B-1295-45B9-AE19-5BC45E98C985','D0E659B8-1155-4F44-9728-B4B6EA4D4D55')
-- Reject an uploaded response: response request => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('44B2B703-5F0B-411E-A623-4045E5023A51', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('44B2B703-5F0B-411E-A623-4045E5023A51','7B4EB88B-1295-45B9-AE19-5BC45E98C985','D0E659B8-1155-4F44-9728-B4B6EA4D4D55')
-- Resubmit an uploaded response: response request => resubmit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('44303715-B033-4038-805D-17208BC3E601', 'Resubmit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('44303715-B033-4038-805D-17208BC3E601','7B4EB88B-1295-45B9-AE19-5BC45E98C985','D51D0D4F-41F7-4208-8722-6D71B23DE2F9')


-- VIEW STATUS AND RESULTS TASK
-- Terminate a distributed request: review responses => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('B7CB6D9D-1663-4B42-A709-1C6822543C9F', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('B7CB6D9D-1663-4B42-A709-1C6822543C9F','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Complete a distributed request: review responses => complete
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('D17F726C-E0BB-4789-9601-879AB4ADF7CD', 'Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('D17F726C-E0BB-4789-9601-879AB4ADF7CD','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Complete a distributed request: review responses => complete pending report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('F44FAA77-F96F-49FA-BFA8-B83274ED3278', 'Create Report', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('F44FAA77-F96F-49FA-BFA8-B83274ED3278','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6')
-- Resubmit an uploaded response: review responses => resubmit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('A307B8AD-DB43-4512-89CB-8B824DDD1E1F', 'Resubmit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('A307B8AD-DB43-4512-89CB-8B824DDD1E1F','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','D51D0D4F-41F7-4208-8722-6D71B23DE2F9')
-- Cancel an pending response: review responses => cancel
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('7CBBB522-0A99-483B-A287-CE35CACD0531', 'Cancel', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('7CBBB522-0A99-483B-A287-CE35CACD0531','D0E659B8-1155-4F44-9728-B4B6EA4D4D55','D0E659B8-1155-4F44-9728-B4B6EA4D4D55')

-- PREPARE DRAFT REPORT TASK
-- Submit a draft report for review: draft report => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('BC82095A-E285-47A4-B18F-E9BD8B0BCAB1', 'Submit Report', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('BC82095A-E285-47A4-B18F-E9BD8B0BCAB1','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6','C80810A3-CF10-4941-854A-A7E2052A5EBA')
-- Complete without a draft report: draft report => complete with no report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('00200409-014A-45F3-84E5-E4BF3A911421', 'Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('00200409-014A-45F3-84E5-E4BF3A911421','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Redistribute request: draft report => redistribute
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('EB198BFF-8781-42C5-9027-0784E858410D', 'Redistribute', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('EB198BFF-8781-42C5-9027-0784E858410D','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6','D0E659B8-1155-4F44-9728-B4B6EA4D4D55')

-- REVIEW DRAFT REPORT TASK 
-- Approve a draft report: draft report review => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('0650CADA-03D2-415C-A089-0C9D00E55742', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('0650CADA-03D2-415C-A089-0C9D00E55742','C80810A3-CF10-4941-854A-A7E2052A5EBA','1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9')
-- Reject a draft report: draft report review => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('5FBD22D9-85E6-4D32-B38D-2A5426AFD53F', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('5FBD22D9-85E6-4D32-B38D-2A5426AFD53F','C80810A3-CF10-4941-854A-A7E2052A5EBA','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6')

-- PREPARE FINAL REPORT TASK 
-- Submit a final report for review: final report => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('FA86B57C-6281-48FC-A6A5-5649390B98C0', 'Submit Final Report', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('FA86B57C-6281-48FC-A6A5-5649390B98C0','1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9','4CCE18C8-CABF-4D22-88AB-611CD560DBF8')
-- Complete without a final report: draft report => complete with no report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('8BED40A0-ACC2-44BF-B381-9FF9231AECAD', 'Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('8BED40A0-ACC2-44BF-B381-9FF9231AECAD','1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Redistribute request: final report => redistribute
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('1D28FF84-A121-4F59-AB0C-E077AF452B19', 'Reopen Draft Report', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('1D28FF84-A121-4F59-AB0C-E077AF452B19','1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6')

-- REVIEW FINAL REPORT TASK 
-- Approve a final report: final report review => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('9A05D22E-40C6-46CD-BF27-BBCA7F385D25', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('9A05D22E-40C6-46CD-BF27-BBCA7F385D25','4CCE18C8-CABF-4D22-88AB-611CD560DBF8','5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4')
-- Reject a final report: final report review => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('54DD89B5-0129-4A28-BC0F-9F1405C88E54', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('54DD89B5-0129-4A28-BC0F-9F1405C88E54','4CCE18C8-CABF-4D22-88AB-611CD560DBF8','43AB48FD-A400-4C0D-92C9-DD2415A5D5B6')
");
        }

        public override void Down()
        {
            Sql(@"DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID IN ('7E8661F2-E540-4E91-A3CF-982DB52EF965', '6248E8B1-7C7C-4959-993F-352C722821A6', '546164B4-0D6D-4C26-868B-07280F627818', 'DEB04531-1635-4B32-AB0F-14C1CCF9BAFB', '0704481B-A42C-4335-B17C-743BF41B03FB', '2BEF97A9-1A3A-46F8-B1D1-7E9E6F6F902A', '14B7E8CF-4CF2-4C3D-A97E-E69C5D090FC0', '982C4DCC-AB1C-4F87-83BB-E09FA8270C17', 'A95899AC-F4F6-41AB-AD4B-D41E05563486', '93473E52-0AB2-41FC-A71A-461A3D9B0D61', 'C8260E90-2C8B-435A-85C8-372B021C3E9F', 'E57E6B65-140F-452B-95FF-04BDB16BCD2D', '74294C54-05C2-4F97-BA35-FAC7C7E6F61A', 'AD6A9E17-936F-431A-A5D6-97B37B7C0796', '7905678A-3CC1-4FFA-85F1-3514B232B694', '8D035265-44EF-40AE-A1CD-30C9EF9871DB', '15F10745-0AAE-4EBB-8D8C-D38E85534EC3', '71AC013A-CE8E-419E-A1F0-67BD3B0777EF', 'C3AE5DF8-DD50-4587-91E1-DD769237F34B', '79F29AF1-A0B8-469D-B7B8-7ED63EF4E55B', '5445DC6E-72DC-4A6B-95B6-338F0359F89E', 'E970F48E-A072-41C1-B8C3-CC34C5826A46', 'FE131FA0-8F8F-4B17-9594-1F0C14D44429', '463E51A5-20F9-4FC3-AE5D-D83D07B30C74', '77B57048-68BD-47A9-B280-E8A9C7418BD1', '44B2B703-5F0B-411E-A623-4045E5023A51', '44303715-B033-4038-805D-17208BC3E601', 'B7CB6D9D-1663-4B42-A709-1C6822543C9F', 'D17F726C-E0BB-4789-9601-879AB4ADF7CD', 'F44FAA77-F96F-49FA-BFA8-B83274ED3278', 'A307B8AD-DB43-4512-89CB-8B824DDD1E1F', '7CBBB522-0A99-483B-A287-CE35CACD0531', 'BC82095A-E285-47A4-B18F-E9BD8B0BCAB1', '00200409-014A-45F3-84E5-E4BF3A911421', 'EB198BFF-8781-42C5-9027-0784E858410D', '0650CADA-03D2-415C-A089-0C9D00E55742', '5FBD22D9-85E6-4D32-B38D-2A5426AFD53F', 'FA86B57C-6281-48FC-A6A5-5649390B98C0', '8BED40A0-ACC2-44BF-B381-9FF9231AECAD', '1D28FF84-A121-4F59-AB0C-E077AF452B19', '9A05D22E-40C6-46CD-BF27-BBCA7F385D25', '54DD89B5-0129-4A28-BC0F-9F1405C88E54')
DELETE FROM WorkflowActivityResults WHERE ID IN ('7e8661f2-e540-4e91-a3cf-982db52ef965', '6248E8B1-7C7C-4959-993F-352C722821A6', '546164B4-0D6D-4C26-868B-07280F627818', 'DEB04531-1635-4B32-AB0F-14C1CCF9BAFB', '0704481B-A42C-4335-B17C-743BF41B03FB', '2BEF97A9-1A3A-46F8-B1D1-7E9E6F6F902A', '14B7E8CF-4CF2-4C3D-A97E-E69C5D090FC0', '5E37CA02-53DA-4DE0-A11B-692DD5A1FFF4', '0321E17F-AA1F-4B23-A145-85B159E74F0F', 'A96FBAD0-8FD8-4D10-8891-D749A71912F8', '31C60BB1-2F6A-423B-A7B7-B52626FD9E97', 'C8891CFD-80BF-4F71-90DE-6748BF71566C', 'C3B13067-3B9D-41E4-8D4A-7114A6E81930', '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77', '49D9E9E8-B1A6-41C8-A36B-4D5FDED3A9B7', 'EA69E5ED-6029-47E8-9B45-F0F00B07FDC2', 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554', 'D51D0D4F-41F7-4208-8722-6D71B23DE2F9', '7B4EB88B-1295-45B9-AE19-5BC45E98C985', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '43AB48FD-A400-4C0D-92C9-DD2415A5D5B6', 'C80810A3-CF10-4941-854A-A7E2052A5EBA', '1C6CCB68-6B62-4F10-A605-2FB6EC64B8C9', '4CCE18C8-CABF-4D22-88AB-611CD560DBF8', '982c4dcc-ab1c-4f87-83bb-e09fa8270c17', 'a95899ac-f4f6-41ab-ad4b-d41e05563486', '93473e52-0ab2-41fc-a71a-461a3d9b0d61', 'c8260e90-2c8b-435a-85c8-372b021c3e9f', 'e57e6b65-140f-452b-95ff-04bdb16bcd2d', '74294c54-05c2-4f97-ba35-fac7c7e6f61a', 'ad6a9e17-936f-431a-a5d6-97b37b7c0796', '7905678a-3cc1-4ffa-85f1-3514b232b694', '8d035265-44ef-40ae-a1cd-30c9ef9871db', '15f10745-0aae-4ebb-8d8c-d38e85534ec3', '71ac013a-ce8e-419e-a1f0-67bd3b0777ef', 'c3ae5df8-dd50-4587-91e1-dd769237f34b',
'79f29af1-a0b8-469d-b7b8-7ed63ef4e55b', '5445dc6e-72dc-4a6b-95b6-338f0359f89e', 'e970f48e-a072-41c1-b8c3-cc34c5826a46', 'fe131fa0-8f8f-4b17-9594-1f0c14d44429', '463e51a5-20f9-4fc3-ae5d-d83d07b30c74', '77b57048-68bd-47a9-b280-e8a9c7418bd1',
'44b2b703-5f0b-411e-a623-4045e5023a51', '44303715-b033-4038-805d-17208bc3e601', 'b7cb6d9d-1663-4b42-a709-1c6822543c9f', 'd17f726c-e0bb-4789-9601-879ab4adf7cd', 'f44faa77-f96f-49fa-bfa8-b83274ed3278', 'a307b8ad-db43-4512-89cb-8b824ddd1e1f', 
'7cbbb522-0a99-483b-a287-ce35cacd0531', 'bc82095a-e285-47a4-b18f-e9bd8b0bcab1', '00200409-014a-45f3-84e5-e4bf3a911421', 'eb198bff-8781-42c5-9027-0784e858410d', '0650cada-03d2-415c-a089-0c9d00e55742', '5fbd22d9-85e6-4d32-b38d-2a5426afd53f', 
'fa86b57c-6281-48fc-a6a5-5649390b98c0', '8bed40a0-acc2-44bf-b381-9ff9231aecad', '1d28ff84-a121-4f59-ab0c-e077af452b19', '9a05d22e-40c6-46cd-bf27-bbca7f385d25', '54dd89b5-0129-4a28-bc0f-9f1405c88e54')
DELETE FROM WorkflowActivities WHERE WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'
DELETE FROM Workflows WHERE ID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");
        }
    }
}
