namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixGetRequestStatusDisplayFunction : DbMigration
    {
        public override void Up()
        {

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
			WHEN @statusValue = '9120' THEN 'Draft Report Pending Review'
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
    }
}
