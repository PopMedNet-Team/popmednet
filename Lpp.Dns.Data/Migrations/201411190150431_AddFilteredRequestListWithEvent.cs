namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilteredRequestListWithEvent : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[FilteredRequestListWithEvent]
(	
	@UserID uniqueidentifier,
	@DataMartID uniqueidentifier,
	@EventID uniqueidentifier
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
	Status,
	CompletedOn,
	UserIdentifier,
	WorkflowID,
	Query
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
							AND EventID=@EventID)
			OR
			--Any Request sent to specified DM in Proj
			EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND 
						    ((@DataMartID IS NOT NULL AND ProjectDataMartEvents.DataMartID=@DataMartID) OR
							 (@DataMartID IS NULL AND ProjectDataMartEvents.DataMartID IN (
							     SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID
							 ))
							)
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectDataMartEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID=@EventID)
			OR
			--Any Request sent to any DM in Proj
			EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID=@EventID)
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
							AND EventID=@EventID
							AND Allowed=0)
			AND
			--Any Request sent to specified DM in Proj
			NOT EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND 
						   ((@DataMartID IS NOT NULL AND ProjectDataMartEvents.DataMartID=@DataMartID) OR
							 (@DataMartID IS NULL AND ProjectDataMartEvents.DataMartID IN (
							     SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID
							 ))
							)
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectDataMartEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID=@EventID
							AND Allowed=0)
			AND
			--Any Request sent to any DM in Proj
			NOT EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID=@EventID
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
							AND EventID=@EventID)
			OR
			--Any Request submitted in Proj
			EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID=@EventID)
		)
		AND
		(
			--No denies anywhere

			--Any Request submitted by member of Org
			NOT EXISTS(SELECT NULL FROM OrganizationEvents WHERE OrganizationEvents.OrganizationID=Requests.OrganizationID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = OrganizationEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID=@EventID
							AND Allowed=0)
			OR
			--Any Request submitted in Proj
			NOT EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID=@EventID
							AND Allowed=0)
		)
	))");

            Sql(@"ALTER FUNCTION [dbo].[ResponseRelatedNotificationRecipients]
(	
	@EventID uniqueidentifier,
	@RequestDataMartID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
SELECT DISTINCT dbo.Users.*
FROM            dbo.UserEventSubscriptions INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID INNER JOIN
                         dbo.LogsRoutingStatusChange ON dbo.UserEventSubscriptions.EventID = @EventID INNER JOIN
                         dbo.RequestDataMarts ON dbo.LogsRoutingStatusChange.RequestDataMartID = dbo.RequestDataMarts.ID
WHERE dbo.UserEventSubscriptions.EventID = @EventID AND dbo.Users.isDeleted=0 AND dbo.Users.isActive=1 AND EXISTS(
  SELECT NULL FROM dbo.FilteredRequestListWithEvent(UserEventSubscriptions.UserID, RequestDataMarts.DataMartID, @EventID) r 
  JOIN dbo.RequestDataMarts rdm ON rdm.RequestID=r.ID
  WHERE
  rdm.ID=@RequestDataMartID
)
AND RequestDataMartID=@RequestDataMartID
)");
        }
        
        public override void Down()
        {
            Sql(@"DROP FUNCTION dbo.FilteredRequestListWithEvent");
            Sql(@"ALTER FUNCTION [dbo].[ResponseRelatedNotificationRecipients]
(	
	@EventID uniqueidentifier,
	@RequestDataMartID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
SELECT DISTINCT dbo.Users.*
FROM            dbo.UserEventSubscriptions INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID INNER JOIN
                         dbo.LogsRoutingStatusChange ON dbo.UserEventSubscriptions.EventID = @EventID INNER JOIN
                         dbo.RequestDataMarts ON dbo.LogsRoutingStatusChange.RequestDataMartID = dbo.RequestDataMarts.ID
WHERE dbo.UserEventSubscriptions.EventID = @EventID AND dbo.Users.isDeleted=0 AND dbo.Users.isActive=1 AND EXISTS(
  SELECT NULL FROM dbo.FilteredRequestListForEvent(UserEventSubscriptions.UserID, RequestDataMarts.DataMartID) r 
  JOIN dbo.RequestDataMarts rdm ON rdm.RequestID=r.ID
  WHERE
  rdm.ID=@RequestDataMartID
)
AND RequestDataMartID=@RequestDataMartID
)");
        }
    }
}
