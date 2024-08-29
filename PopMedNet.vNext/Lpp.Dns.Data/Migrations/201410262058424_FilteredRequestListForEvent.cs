namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilteredRequestListForEvent : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[FilteredRequestListForEvent]
(	
	@UserID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	--declare @userid uniqueidentifier
	--set @userid='56B50001-6A71-4CCF-8E42-A22300C91367'
SELECT 
    ID,
	TimeStamp,
	Identifier,
	Name,
	Description,
	AdditionalInstructions,
	IsScheduled as Scheduled,
	IsTemplate as Template,
	IsDeleted as Deleted,
	Priority,
	OrganizationID,
	PurposeOfUse,
	PhiDisclosureLevel,
	Schedule,
	ScheduleCount,
	ProjectID,
	RequestTypeID,
	AdapterPackageVersion,
	IRBApprovalNo,
	DueDate,
	ActivityDescription,
	RequesterCenterID,
	WorkplanTypeID,
	ActivityID,
	CreatedOn,
	CreatedByID,
	UpdatedOn,
	UpdatedByID,
	SubmittedOn,
	SubmittedByID,
	ApprovedForDraftOn,
	ApprovedForDraftByID,
	RejectedOn,
	RejectedByID,
	CancelledOn,
	CancelledByID,
	WorkFlowActivityID,
	Private,
	Status
FROM Requests

WHERE
    --Rule #1: Subscriber has any of these rights: New Request Submitted, Routing Status Changed, Submitted Request Awaits a Response, Uploaded Results Need Approval
	(
	    (
			--Any Request sent to any DM of Org
			EXISTS(SELECT NULL FROM OrganizationEvents WHERE OrganizationEvents.OrganizationID IN
								(SELECT OrganizationID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID)
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = OrganizationEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('06E30001-ED86-4427-9936-A22200CC74F0','5AB90001-8072-42CD-940F-A22200CC24A2','688B0001-1572-41CA-8298-A22200CBD542','F31C0001-6900-4BDB-A03A-A22200CC019C'))
			OR
			--Any Request sent to specified DM in Proj
			EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND ProjectDataMartEvents.DataMartID IN
			                    (SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID)
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectDataMartEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('06E30001-ED86-4427-9936-A22200CC74F0','5AB90001-8072-42CD-940F-A22200CC24A2','688B0001-1572-41CA-8298-A22200CBD542','F31C0001-6900-4BDB-A03A-A22200CC019C'))
			OR
			--Any Request sent to any DM in Proj
			EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('06E30001-ED86-4427-9936-A22200CC74F0','5AB90001-8072-42CD-940F-A22200CC24A2','688B0001-1572-41CA-8298-A22200CBD542','F31C0001-6900-4BDB-A03A-A22200CC019C'))
		)
		AND
		--No denies anywhere
		(
			--Any Request sent to any DM of Org
			NOT EXISTS(SELECT NULL FROM OrganizationEvents WHERE OrganizationEvents.OrganizationID IN
								(SELECT OrganizationID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID)
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = OrganizationEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('06E30001-ED86-4427-9936-A22200CC74F0','5AB90001-8072-42CD-940F-A22200CC24A2','688B0001-1572-41CA-8298-A22200CBD542','F31C0001-6900-4BDB-A03A-A22200CC019C')
							AND Allowed=0)
			AND
			--Any Request sent to specified DM in Proj
			NOT EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND ProjectDataMartEvents.DataMartID IN
			                    (SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID)
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectDataMartEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('06E30001-ED86-4427-9936-A22200CC74F0','5AB90001-8072-42CD-940F-A22200CC24A2','688B0001-1572-41CA-8298-A22200CBD542','F31C0001-6900-4BDB-A03A-A22200CC019C')
							AND Allowed=0)
			AND
			--Any Request sent to any DM in Proj
			NOT EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('06E30001-ED86-4427-9936-A22200CC74F0','5AB90001-8072-42CD-940F-A22200CC24A2','688B0001-1572-41CA-8298-A22200CBD542','F31C0001-6900-4BDB-A03A-A22200CC019C')
							AND Allowed=0)
 	    )
	)
	OR

	--Rule #2: Subscriber has any of these rights: Request Status Changed, Results Viewed, Submitted Request Needs Approval, Results Reminder
	(
		(
     		--User who is the request creator/submitter has to be handled outside this function

			--Any Request submitted by member of Org
			EXISTS(SELECT NULL FROM OrganizationEvents WHERE OrganizationEvents.OrganizationID=Requests.OrganizationID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = OrganizationEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08'))
			OR
			--Any Request submitted in Proj
			EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08'))
		)
		AND
		(
			--No denies anywhere

			--Any Request submitted by member of Org
			NOT EXISTS(SELECT NULL FROM OrganizationEvents WHERE OrganizationEvents.OrganizationID=Requests.OrganizationID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = OrganizationEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08')
							AND Allowed=0)
			OR
			--Any Request submitted in Proj
			NOT EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08')
							AND Allowed=0)
		)
	)
)");
        }
        
        public override void Down()
        {
            Sql(@"drop function dbo.FilteredRequestListForEvent");
        }
    }
}
