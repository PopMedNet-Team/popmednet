namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SummaryWorkflowProcess : DbMigration
    {
        public override void Up()
        {

            Sql(@"INSERT INTO Workflows (ID, Name, Description) VALUES ('7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Summary Query', 'Workflow for Summary Queries.')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [End]) VALUES ('B34013F5-88C5-4D79-997B-6525D740E0CB', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Terminate Request', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description, [Start]) VALUES ('197AF4BA-F079-48DD-9E7C-C7BE7F8DC896', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Draft Request', '', 1)
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('CC1BCADD-4487-47C7-BDCA-1010F2C68FE0', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Review Request', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('752B83D7-2190-49DF-9BAE-983A7880A899', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Distribute Request', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('303C1C1B-A330-41DB-B3B6-4D7C02D02C8C', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Respond to Request', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('9CDD7176-9361-4585-B79C-438645DA45BE', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Review DataMart Responses', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Review Responses', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Review Draft Report', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('9173A8E7-27C4-469D-853D-69A78501A522', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Submit Draft Report', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('2E7A3263-C87E-47BA-AC35-A78ABF8FE606', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Review Final Report', '')
INSERT INTO WorkflowActivities (ID, WorkflowID, Name, Description) VALUES ('F888C5D6-B8EB-417C-9DE2-4A96D75F3208', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Submit Final Report', '')

-- CREATE REQUEST TASK
-- Delete a request that has not been submitted: create request => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('10AC80A5-850B-49D1-9E13-AE6AE2D63701', 'Delete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('10AC80A5-850B-49D1-9E13-AE6AE2D63701','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Submit a request for approval: create request => request approval
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('F14B4432-804A-4052-A8EE-64260CE5DCB7', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('F14B4432-804A-4052-A8EE-64260CE5DCB7','197AF4BA-F079-48DD-9E7C-C7BE7F8DC896','CC1BCADD-4487-47C7-BDCA-1010F2C68FE0')

-- REVIEW REQUEST FORM TASK
-- Terminate a submitted request: review request form => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('61939A65-5E6B-4B8F-8F0F-4D76DFAD2854', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('61939A65-5E6B-4B8F-8F0F-4D76DFAD2854','CC1BCADD-4487-47C7-BDCA-1010F2C68FE0','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Approve a submitted request: review request form => distribution
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('4AE61A78-CE31-4A01-807F-DB18A535E4E0', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('4AE61A78-CE31-4A01-807F-DB18A535E4E0','CC1BCADD-4487-47C7-BDCA-1010F2C68FE0','752B83D7-2190-49DF-9BAE-983A7880A899')

-- DISTRIBUTE REQUEST TASK
-- Terminate a submitted request: distribution => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('53579F36-9D20-47D9-AC33-643D9130080B', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('53579F36-9D20-47D9-AC33-643D9130080B','752B83D7-2190-49DF-9BAE-983A7880A899','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Distribute a submitted request: distribution => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('80FD6F76-2E32-4D35-9797-0B541507CB56', 'Submit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('80FD6F76-2E32-4D35-9797-0B541507CB56','752B83D7-2190-49DF-9BAE-983A7880A899','752B83D7-2190-49DF-9BAE-983A7880A899')
-- Modify a submitted request: distribution => modify
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('94513F48-4C4A-4449-BA95-5B0CD81DB642', 'Modify Request', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('94513F48-4C4A-4449-BA95-5B0CD81DB642','752B83D7-2190-49DF-9BAE-983A7880A899','CC1BCADD-4487-47C7-BDCA-1010F2C68FE0')

-- DATA PARTNER RESPOND TO A REQUEST TASK
-- Upload a response to a submited request: respond => upload
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('668EE9C7-4930-423E-AA9E-150B646121F4', 'Upload Response', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('668EE9C7-4930-423E-AA9E-150B646121F4','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')
-- Reject a response to a submited request: respond => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('D0A0924F-F4B5-43BF-89A6-C7F32E764735', 'Reject Response', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('D0A0924F-F4B5-43BF-89A6-C7F32E764735','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')

-- DATA PARTNER RESPONSE REVIEW TASK
-- Approve an uploaded response: response request => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('B240D900-8BE6-4907-8F08-590864A1EA1A', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('B240D900-8BE6-4907-8F08-590864A1EA1A','9CDD7176-9361-4585-B79C-438645DA45BE','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Reject an uploaded response: response request => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('634D54E5-74C5-46BC-A0DF-33F488AA584B', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('634D54E5-74C5-46BC-A0DF-33F488AA584B','9CDD7176-9361-4585-B79C-438645DA45BE','752B83D7-2190-49DF-9BAE-983A7880A899')
-- Resubmit an uploaded response: response request => resubmit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('C494B6D6-622F-4BD8-BEA3-2716FE34D5AD', 'Resubmit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('C494B6D6-622F-4BD8-BEA3-2716FE34D5AD','9CDD7176-9361-4585-B79C-438645DA45BE','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')

-- REVIEW RESPONSES TASK
-- Terminate a distributed request: review responses => terminate
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('BC2C554C-97CD-405B-AA55-91C01006C93C', 'Terminate', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('BC2C554C-97CD-405B-AA55-91C01006C93C','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Complete a distributed request: review responses => complete
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('9CC66B2D-F813-4C6B-82C7-EE0893D72257', 'Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('9CC66B2D-F813-4C6B-82C7-EE0893D72257','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Complete a distributed request: review responses => complete pending report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('3A069D57-4658-4DFE-BA57-70EE84292D64', 'Create Report', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('9CC66B2D-F813-4C6B-82C7-EE0893D72257','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','9173A8E7-27C4-469D-853D-69A78501A522')
-- Resubmit an uploaded response: review responses => resubmit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('3FB86142-D6A1-45A5-A988-EF45B10D5C83', 'Resubmit', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('3FB86142-D6A1-45A5-A988-EF45B10D5C83','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','303C1C1B-A330-41DB-B3B6-4D7C02D02C8C')
-- Cancel an pending response: review responses => cancel
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('7828CAD1-6547-4605-A361-6E76A796326B', 'Cancel', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('7828CAD1-6547-4605-A361-6E76A796326B','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')

-- PREPARE DRAFT REPORT TASK
-- Submit a draft report for review: draft report => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('7385973B-1C4F-4224-A13C-F148685F0217', 'Submit Report', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('7385973B-1C4F-4224-A13C-F148685F0217','9173A8E7-27C4-469D-853D-69A78501A522','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81')
-- Complete without a draft report: draft report => complete with no report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A', 'Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A','9173A8E7-27C4-469D-853D-69A78501A522','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Redistribute request: draft report => redistribute
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA', 'Redistribute', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA','9173A8E7-27C4-469D-853D-69A78501A522','6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B')

-- REVIEW DRAFT REPORT TASK 
-- Approve a draft report: draft report review => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('36F8F9BA-849A-493F-A9FA-B443370EF5AD', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('36F8F9BA-849A-493F-A9FA-B443370EF5AD','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81','F888C5D6-B8EB-417C-9DE2-4A96D75F3208')
-- Reject a draft report: draft report review => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('687360E2-8389-48E3-A3FE-71248F0D6192', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('687360E2-8389-48E3-A3FE-71248F0D6192','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81','9173A8E7-27C4-469D-853D-69A78501A522')

-- PREPARE FINAL REPORT TASK 
-- Submit a final report for review: final report => submit
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('0CF45F91-6F2C-4283-BDC2-0896B552694A', 'Submit Report', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('0CF45F91-6F2C-4283-BDC2-0896B552694A','F888C5D6-B8EB-417C-9DE2-4A96D75F3208','2E7A3263-C87E-47BA-AC35-A78ABF8FE606')
-- Complete without a draft report: draft report => complete with no report
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('B38C1515-BF25-4179-BA09-9F811E0053D8', 'Complete', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('B38C1515-BF25-4179-BA09-9F811E0053D8','F888C5D6-B8EB-417C-9DE2-4A96D75F3208','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Redistribute request: draft report => redistribute
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('ECCBF404-B3BA-4C5E-BB6E-388725938DC3', 'Draft Review', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('ECCBF404-B3BA-4C5E-BB6E-388725938DC3','F888C5D6-B8EB-417C-9DE2-4A96D75F3208','2E6B1E60-8E26-45A2-9DAE-EFE6F4760A81')

-- REVIEW FINAL REPORT TASK 
-- Approve a final report: final report review => approve
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('0811D461-626F-4CCF-B1FA-5B495858C67D', 'Approve', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('0811D461-626F-4CCF-B1FA-5B495858C67D','2E7A3263-C87E-47BA-AC35-A78ABF8FE606','B34013F5-88C5-4D79-997B-6525D740E0CB')
-- Reject a final report: final report review => reject
INSERT INTO WorkflowActivityResults (ID, Name, Description) VALUES ('2AFFB9A9-3BC1-4039-ADD9-FE809C81C800', 'Reject', '')
INSERT INTO WorkflowActivityCompletionMaps(WorkflowActivityResultID, SourceWorkflowActivityID, DestinationWorkflowActivityID) VALUES ('2AFFB9A9-3BC1-4039-ADD9-FE809C81C800','2E7A3263-C87E-47BA-AC35-A78ABF8FE606','F888C5D6-B8EB-417C-9DE2-4A96D75F3208')
");
        }
        
        public override void Down()
        {
            Sql(@"DELETE WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID IN ('10AC80A5-850B-49D1-9E13-AE6AE2D63701', 'F14B4432-804A-4052-A8EE-64260CE5DCB7', '61939A65-5E6B-4B8F-8F0F-4D76DFAD2854', '4AE61A78-CE31-4A01-807F-DB18A535E4E0', '53579F36-9D20-47D9-AC33-643D9130080B', '80FD6F76-2E32-4D35-9797-0B541507CB56', '94513F48-4C4A-4449-BA95-5B0CD81DB642', '668EE9C7-4930-423E-AA9E-150B646121F4', 'D0A0924F-F4B5-43BF-89A6-C7F32E764735', 'B240D900-8BE6-4907-8F08-590864A1EA1A', '634D54E5-74C5-46BC-A0DF-33F488AA584B', 'C494B6D6-622F-4BD8-BEA3-2716FE34D5AD', 'BC2C554C-97CD-405B-AA55-91C01006C93C', '9CC66B2D-F813-4C6B-82C7-EE0893D72257', '3A069D57-4658-4DFE-BA57-70EE84292D64', '3FB86142-D6A1-45A5-A988-EF45B10D5C83', '7828CAD1-6547-4605-A361-6E76A796326B', '7385973B-1C4F-4224-A13C-F148685F0217', '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A', 'B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA', '36F8F9BA-849A-493F-A9FA-B443370EF5AD', '687360E2-8389-48E3-A3FE-71248F0D6192', '0CF45F91-6F2C-4283-BDC2-0896B552694A', 'B38C1515-BF25-4179-BA09-9F811E0053D8', 'ECCBF404-B3BA-4C5E-BB6E-388725938DC3', '0811D461-626F-4CCF-B1FA-5B495858C67D', '2AFFB9A9-3BC1-4039-ADD9-FE809C81C800')
DELETE FROM WorkflowActivityResults WHERE ID IN ('10AC80A5-850B-49D1-9E13-AE6AE2D63701', 'F14B4432-804A-4052-A8EE-64260CE5DCB7', '61939A65-5E6B-4B8F-8F0F-4D76DFAD2854', '4AE61A78-CE31-4A01-807F-DB18A535E4E0', '53579F36-9D20-47D9-AC33-643D9130080B', '80FD6F76-2E32-4D35-9797-0B541507CB56', '94513F48-4C4A-4449-BA95-5B0CD81DB642', '668EE9C7-4930-423E-AA9E-150B646121F4', 'D0A0924F-F4B5-43BF-89A6-C7F32E764735', 'B240D900-8BE6-4907-8F08-590864A1EA1A', '634D54E5-74C5-46BC-A0DF-33F488AA584B', 'C494B6D6-622F-4BD8-BEA3-2716FE34D5AD', 'BC2C554C-97CD-405B-AA55-91C01006C93C', '9CC66B2D-F813-4C6B-82C7-EE0893D72257', '3A069D57-4658-4DFE-BA57-70EE84292D64', '3FB86142-D6A1-45A5-A988-EF45B10D5C83', '7828CAD1-6547-4605-A361-6E76A796326B', '7385973B-1C4F-4224-A13C-F148685F0217', '066B79BD-ECB4-43C6-8DF9-1FDB76DD2C7A', 'B3C959D1-E5C6-4FCD-8812-DD2EBEA468DA', '36F8F9BA-849A-493F-A9FA-B443370EF5AD', '687360E2-8389-48E3-A3FE-71248F0D6192', '0CF45F91-6F2C-4283-BDC2-0896B552694A', 'B38C1515-BF25-4179-BA09-9F811E0053D8', 'ECCBF404-B3BA-4C5E-BB6E-388725938DC3', '0811D461-626F-4CCF-B1FA-5B495858C67D', '2AFFB9A9-3BC1-4039-ADD9-FE809C81C800')
DELETE FROM WorkflowActivities WHERE WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'
DELETE FROM Workflows WHERE ID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'
");
        }
    }
}
