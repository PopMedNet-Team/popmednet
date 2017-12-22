namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowActivityNames : DbMigration
    {
        public override void Up()
        {
            //fix task subjects set to start with workflow name then activity
            Sql("UPDATE Tasks SET Subject = RTRIM(LTRIM(SUBSTRING([Subject], CHARINDEX(':', [Subject]) + 1, LEN([Subject]) - CHARINDEX(':', [Subject])))) WHERE WorkflowActivityID IS NOT NULL");
            
            //list routings for default and summary wf
            Sql("UPDATE WorkflowActivities SET [Name] = 'Complete Distribution' WHERE ID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'");

            //update summary wf
            Sql("UPDATE WorkflowActivities SET [Name] = 'Request Form' WHERE ID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Request Form Review' WHERE ID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Distribution' WHERE ID = '752B83D7-2190-49DF-9BAE-983A7880A899'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Complete Distribution' WHERE ID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Draft Report' WHERE ID = '9173A8E7-27C4-469D-853D-69A78501A522'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Final Report' WHERE ID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208'");

            //update for modular program wf
            Sql("UPDATE WorkflowActivities SET [Name] = 'Request Form' WHERE ID = '0321E17F-AA1F-4B23-A145-85B159E74F0F'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Specifications Review' WHERE ID = '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Distribution' WHERE ID = 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Complete Distribution' WHERE ID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55'");

            //fix draft review enum display text
            Sql(@"ALTER FUNCTION [dbo].[GetRequestStatusDisplayText](@RequestID uniqueidentifier) RETURNS nvarchar(50) 
AS
BEGIN
	DECLARE @statusText nvarchar(50),
		    @statusValue nvarchar(10),
			@IsScheduled bit,
			@workflowActivityID uniqueidentifier,
			@workflowActivityName nvarchar(255),
			@incompleteRoutings int,
			@completeRoutings int;

	SELECT @statusValue = CAST(r.Status as nvarchar(10)), @workflowActivityID = r.WorkFlowActivityID, @IsScheduled = r.IsScheduled, @workflowActivityName = a.Name,
	@incompleteRoutings = (SELECT COUNT(*) FROM RequestDataMarts rdm WHERE rdm.RequestID = @RequestID AND rdm.QueryStatusTypeId IN (3, 5, 12, 13, 16)),
	@completeRoutings = (SELECT COUNT(*) FROM RequestDataMarts rdm WHERE rdm.RequestID = @RequestID AND rdm.QueryStatusTypeId <> 6)
	FROM Requests r 
	LEFT OUTER JOIN WorkflowActivities a ON r.WorkFlowActivityID = a.ID
	WHERE r.ID = @RequestID;

	SET @statusText = 
		CASE 
			WHEN @statusValue = '300' THEN 'Awaiting Request Approval'
			WHEN @statusValue = '1100' THEN 'Awaiting Response Approval'
			WHEN @statusValue = '9999' THEN 'Canceled'
			WHEN @statusValue = '390' THEN 'Terminated'
			WHEN @statusValue = '10000' THEN 'Complete'
			WHEN @statusValue = '10100' THEN 'Complete, with Report'
			WHEN @statusValue = '200' AND @IsScheduled = 1 THEN 'Scheduled'
			WHEN @statusValue = '200' AND @IsScheduled = 0 THEN 'Draft'
			WHEN @statusValue = '250' THEN 'Draft Pending Review'
			WHEN @statusValue = '1000' THEN 'Examined By Investigator'
			WHEN @statusValue = '9998' THEN 'Failed'
			WHEN @statusValue = '9997' THEN 'Hold'
			--'Partially Complete'
			WHEN @statusValue = '9000' THEN (CAST(@incompleteRoutings as nvarchar(10)) + ' / ' + CAST(@completeRoutings as nvarchar(10)) + ' Responses Received') 
			WHEN @statusValue = '700' THEN 'Pending Upload'
			WHEN @statusValue = '400' THEN 'Request Rejected'
			WHEN @statusValue = '900' THEN 'Response Rejected After Upload'
			WHEN @statusValue = '800' THEN 'Response Rejected Before Upload'
			WHEN @statusValue = '600' THEN 'Resubmitted'
			WHEN @statusValue = '500' THEN 'Submitted'
			WHEN @statusValue = '100' THEN '3rd Party Draft'
			WHEN @statusValue = '310' THEN 'Pending Working Specifications'
			WHEN @statusValue = '320' THEN 'Working Specification Pending Review'
			WHEN @statusValue = '330' THEN 'Specifications Pending Review'
			WHEN @statusValue = '340' THEN 'Pending Specifications'
			WHEN @statusValue = '350' THEN 'Pending Pre-Distribution Testing'
			WHEN @statusValue = '360' THEN 'Pre-Distribution Testing Pending Review'
			WHEN @statusValue = '370' THEN 'Request Pending Distribution'
			WHEN @statusValue = '9110' THEN 'Pending Draft Report'
			WHEN @statusValue = '9120' THEN 'Draft Report Pending Approval'
			WHEN @statusValue = '9130' THEN 'Pending Final Report'
			WHEN @statusValue = '9140' THEN 'Final Report Pending Review'			
			WHEN @workflowActivityID IS NOT NULL AND ISNULL(@workflowActivityName, '') <> '' THEN @workflowActivityName
			ELSE 'Unknown'
		END;

	return @statusText
END");
        }
        
        public override void Down()
        {
            //list routings for default and summary wf
            Sql("UPDATE WorkflowActivities SET [Name] = 'View Status and Results' WHERE ID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344'");

            //update summary wf
            Sql("UPDATE WorkflowActivities SET [Name] = 'New Request' WHERE ID = '197AF4BA-F079-48DD-9E7C-C7BE7F8DC896'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Request Review' WHERE ID = 'CC1BCADD-4487-47C7-BDCA-1010F2C68FE0'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Distribute Request' WHERE ID = '752B83D7-2190-49DF-9BAE-983A7880A899'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'View Status and Results' WHERE ID = '6BD20AD7-502C-4D8E-A0BB-A9A5CE388C4B'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Submit Draft Report' WHERE ID = '9173A8E7-27C4-469D-853D-69A78501A522'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Submit Final Report' WHERE ID = 'F888C5D6-B8EB-417C-9DE2-4A96D75F3208'");

            //update for modular program wf
            Sql("UPDATE WorkflowActivities SET [Name] = 'New Request' WHERE ID = '0321E17F-AA1F-4B23-A145-85B159E74F0F'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Specification Review' WHERE ID = '948B60F0-8CE5-4B14-9AD6-C50EC37DFC77'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'Request Distribution' WHERE ID = 'E6CCD61B-81C4-4217-A958-ADAFB5EE5554'");
            Sql("UPDATE WorkflowActivities SET [Name] = 'View Status and Results' WHERE ID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55'");

            //fix draft review enum display text
            Sql(@"ALTER FUNCTION [dbo].[GetRequestStatusDisplayText](@RequestID uniqueidentifier) RETURNS nvarchar(50) 
AS
BEGIN
	DECLARE @statusText nvarchar(50),
		    @statusValue nvarchar(10),
			@IsScheduled bit,
			@workflowActivityID uniqueidentifier,
			@workflowActivityName nvarchar(255),
			@incompleteRoutings int,
			@completeRoutings int;

	SELECT @statusValue = CAST(r.Status as nvarchar(10)), @workflowActivityID = r.WorkFlowActivityID, @IsScheduled = r.IsScheduled, @workflowActivityName = a.Name,
	@incompleteRoutings = (SELECT COUNT(*) FROM RequestDataMarts rdm WHERE rdm.RequestID = @RequestID AND rdm.QueryStatusTypeId IN (3, 5, 12, 13, 16)),
	@completeRoutings = (SELECT COUNT(*) FROM RequestDataMarts rdm WHERE rdm.RequestID = @RequestID AND rdm.QueryStatusTypeId <> 6)
	FROM Requests r 
	LEFT OUTER JOIN WorkflowActivities a ON r.WorkFlowActivityID = a.ID
	WHERE r.ID = @RequestID;

	SET @statusText = 
		CASE 
			WHEN @statusValue = '300' THEN 'Awaiting Request Approval'
			WHEN @statusValue = '1100' THEN 'Awaiting Response Approval'
			WHEN @statusValue = '9999' THEN 'Canceled'
			WHEN @statusValue = '390' THEN 'Terminated'
			WHEN @statusValue = '10000' THEN 'Complete'
			WHEN @statusValue = '10100' THEN 'Complete, with Report'
			WHEN @statusValue = '200' AND @IsScheduled = 1 THEN 'Scheduled'
			WHEN @statusValue = '200' AND @IsScheduled = 0 THEN 'Draft'
			WHEN @statusValue = '250' THEN 'Draft Review'
			WHEN @statusValue = '1000' THEN 'Examined By Investigator'
			WHEN @statusValue = '9998' THEN 'Failed'
			WHEN @statusValue = '9997' THEN 'Hold'
			--'Partially Complete'
			WHEN @statusValue = '9000' THEN (CAST(@incompleteRoutings as nvarchar(10)) + ' / ' + CAST(@completeRoutings as nvarchar(10)) + ' Responses Received') 
			WHEN @statusValue = '700' THEN 'Pending Upload'
			WHEN @statusValue = '400' THEN 'Request Rejected'
			WHEN @statusValue = '900' THEN 'Response Rejected After Upload'
			WHEN @statusValue = '800' THEN 'Response Rejected Before Upload'
			WHEN @statusValue = '600' THEN 'Resubmitted'
			WHEN @statusValue = '500' THEN 'Submitted'
			WHEN @statusValue = '100' THEN '3rd Party Draft'
			WHEN @statusValue = '310' THEN 'Pending Working Specifications'
			WHEN @statusValue = '320' THEN 'Working Specification Pending Review'
			WHEN @statusValue = '330' THEN 'Specifications Pending Review'
			WHEN @statusValue = '340' THEN 'Pending Specifications'
			WHEN @statusValue = '350' THEN 'Pending Pre-Distribution Testing'
			WHEN @statusValue = '360' THEN 'Pre-Distribution Testing Pending Review'
			WHEN @statusValue = '370' THEN 'Request Pending Distribution'
			WHEN @statusValue = '9110' THEN 'Pending Draft Report'
			WHEN @statusValue = '9120' THEN 'Draft Report Pending Approval'
			WHEN @statusValue = '9130' THEN 'Pending Final Report'
			WHEN @statusValue = '9140' THEN 'Final Report Pending Review'			
			WHEN @workflowActivityID IS NOT NULL AND ISNULL(@workflowActivityName, '') <> '' THEN @workflowActivityName
			ELSE 'Unknown'
		END;

	return @statusText
END");
        }
    }
}
