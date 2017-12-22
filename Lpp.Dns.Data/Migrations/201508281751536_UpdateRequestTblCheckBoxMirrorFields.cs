namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestTblCheckBoxMirrorFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "MirrorBudgetFields", c => c.Boolean(nullable: false));
            DropColumn("dbo.Requests", "isChecked");

            Sql(@"ALTER FUNCTION [dbo].[FilteredRequestListForEvent]
(	
	@UserID uniqueidentifier,
	@DataMartID uniqueidentifier
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
	MSRequestID,
	SourceActivityID,
	SourceActivityProjectID,
	SourceTaskOrderID,
	MirrorBudgetFields
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
			--Any Request sent to specified DM in Proj - this subquery may be unused; investigate.
			EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND 
						    ((@DataMartID IS NOT NULL AND ProjectDataMartEvents.DataMartID=@DataMartID) OR
							 (@DataMartID IS NULL AND ProjectDataMartEvents.DataMartID IN (
							     SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID
							 ))
							)
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
			--Any Request sent to specified DM in Proj - this subquery may be unused; investigate.
			NOT EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND 
						   ((@DataMartID IS NOT NULL AND ProjectDataMartEvents.DataMartID=@DataMartID) OR
							 (@DataMartID IS NULL AND ProjectDataMartEvents.DataMartID IN (
							     SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID
							 ))
							)
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
			OR
			--Any Request sent to any Org in Proj
			EXISTS(SELECT NULL FROM ProjectOrganizationEvents WHERE ProjectOrganizationEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectOrganizationEvents.SecurityGroupID AND UserID = @UserID)
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
			AND
			--Any Request submitted in Proj
			NOT EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08')
							AND Allowed=0)
			AND
			--Any Request sent to any Org in Proj
			NOT EXISTS(SELECT NULL FROM ProjectOrganizationEvents WHERE ProjectOrganizationEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectOrganizationEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08')
							AND Allowed=0)
		)
	))");
            Sql(@"ALTER FUNCTION [dbo].[FilteredRequestListWithEvent]
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
	Query,
	MSRequestID,
    SourceActivityID,
    SourceActivityProjectID,
    SourceTaskOrderID,
	MirrorBudgetFields
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
            Sql(@"ALTER FUNCTION [dbo].[FilteredRequestList]
(	
	@UserID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
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
	Query,
	MSRequestID,
	SourceActivityID,
	SourceActivityProjectID,
	SourceTaskOrderID,
	MirrorBudgetFields
FROM Requests

WHERE
    --Rule #1: User who creates a request or submits it can always see the request
	(Requests.CreatedByID = @UserID OR Requests.SubmittedByID = @UserID)
	OR
	(
		Requests.Status >= 250
		AND
		(

			--Rule #2: User has any of these Rights: See Requests, Reject Request, Request Shared Folder View, View Request, Approve Reject Request
			(
				-- Reject Request: 0CABF382-93D3-4DAC-AA80-2DE500A5F945
				-- global, projects, datamarts, projectdatamarts, orgs
				(
					(
						--check permissions
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId))
						--Project List Request
						OR
						EXISTS
						(
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId, rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							)
						)
					)
					AND
					(
						-- make sure no denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND						
						EXISTS
						(	
							-- make sure at least one routing has permission						
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId, rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE a.Allowed > 0)
							)
						)
					)
				)
				OR
				-- Request Shared Folder View: 5CCB0EC2-006D-4345-895E-5DD2C6C8C791
				-- global, RequestSharedFolders
				(
					(
						--check for permission
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791'))
						OR
						EXISTS
						(
							SELECT NULL FROM RequestSharedFolderRequests rsfr
							WHERE rsfr.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclRequestSharedFoldersFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791', rsfr.RequestSharedFolderID))
							)
						)
					)
					AND
					(
						-- make sure no denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS
						(
							SELECT NULL FROM RequestSharedFolderRequests rsfr
							WHERE rsfr.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclRequestSharedFoldersFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791', rsfr.RequestSharedFolderID) a WHERE a.Allowed = 0)
							)
						)
					)
				)
				OR
				-- View Request: 0549F5C8-6C0E-4491-BE90-EE0F29652422
				-- global, projects, orgs, projectorgs
				(
					(
						--check permissions
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID))
						OR
						EXISTS
						(
							SELECT NULL FROM RequestDataMarts rdm
							WHERE rdm.RequestID = Requests.ID
							AND 
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId, Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							)
						)
					)
					AND
					(
						--make sure no denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS
						(
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm
							WHERE rdm.RequestID = Requests.ID
							AND 
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId, Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE a.Allowed > 0)
							)
						)
					)
				)
			)
			OR
			--Rule #3: User who can see requests because of a datamart can see the request so long as it's submitted: 5D6DD388-7842-40A1-A27A-B9782A445E20
			(
				Requests.Status >= 500 AND NOT Requests.SubmittedByID IS NULL
				AND
				(					
					(
						--check permission, datamarts, projects, organizations, projectdatamarts, global 
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.OrganizationID))
						OR
						EXISTS(
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId, rdm.DataMartID))
							)
						)
					)
					AND
					(
						-- check for denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId, rdm.DataMartID) a WHERE a.Allowed > 0)
							)
						)
					)
				)-- end SeeRequests permission checking
			)
			OR
			--Rule #4: User can approve or reject submission of the request: 40DB7DE2-EEFA-4D31-B400-7E72AB34DE99
			(
				-- global, projects, orgs, projectorgs
				Requests.Status >= 300 AND
				(
					(
						-- check for permission
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.OrganizationID))
						OR
						EXISTS (
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID, Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							)
						)
					)
					AND
					(
						-- check for denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS (
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID, Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE a.Allowed > 0)
							)
						)
					)
				)
			)
			OR
			--Rule #5: User can approve responses for the request
			(
				NOT Requests.SubmittedByID IS NULL AND Status >= 500
				AND
				--get the ones the user has the rights to approve regardless of if they need approval or not: A58791B5-E8AF-48D0-B9CD-ED0B54E564E6
				(
					(
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID))
						OR
						EXISTS (
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', rdm.DataMartID))
							)
						)
					)
					AND
					(
						-- check high level denies, and make sure that at least one routing is approved
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS (
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', rdm.DataMartID) a WHERE a.Allowed > 0)
							)
						)
					)
				)
				-- check if there are any datamarts that need to be approved
				AND EXISTS
				(
					SELECT NULL FROM RequestDataMarts rdm 
					WHERE rdm.RequestID = Requests.ID
					AND rdm.QueryStatusTypeId <> 1 --not in draft state
					AND NOT 
					(-- look to see if the submitter can skip approval for the datamart: A0F5B621-277A-417C-A862-801D7B9030A2
						(
							EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', rdm.DataMartID))
							OR
							EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID, rdm.DataMartID))
							OR
							EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID))
							OR
							EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							OR
							EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2'))
						)
						AND
						(
							NOT EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', rdm.DataMartID) a WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID, rdm.DataMartID) a WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID) a WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2') a WHERE  a.Allowed = 0)
						)
					)--end skip approval check

				)-- end check if datamart needs approval

			)--end rule #5

		)--end of greater than draft status clause
	)--end of clause for requests user didn't create
)");
        }
        
        public override void Down()
        {
            Sql(@"ALTER FUNCTION [dbo].[FilteredRequestListForEvent]
(	
	@UserID uniqueidentifier,
	@DataMartID uniqueidentifier
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
	MSRequestID,
	SourceActivityID,
	SourceActivityProjectID,
	SourceTaskOrderID
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
			--Any Request sent to specified DM in Proj - this subquery may be unused; investigate.
			EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND 
						    ((@DataMartID IS NOT NULL AND ProjectDataMartEvents.DataMartID=@DataMartID) OR
							 (@DataMartID IS NULL AND ProjectDataMartEvents.DataMartID IN (
							     SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID
							 ))
							)
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
			--Any Request sent to specified DM in Proj - this subquery may be unused; investigate.
			NOT EXISTS(SELECT NULL FROM ProjectDataMartEvents WHERE ProjectDataMartEvents.ProjectID=Requests.ProjectId AND 
						   ((@DataMartID IS NOT NULL AND ProjectDataMartEvents.DataMartID=@DataMartID) OR
							 (@DataMartID IS NULL AND ProjectDataMartEvents.DataMartID IN (
							     SELECT DataMartID FROM DataMarts dm
								 JOIN RequestDataMarts rdm ON dm.ID=rdm.DataMartID
								 WHERE rdm.RequestID=Requests.ID
							 ))
							)
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
			OR
			--Any Request sent to any Org in Proj
			EXISTS(SELECT NULL FROM ProjectOrganizationEvents WHERE ProjectOrganizationEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectOrganizationEvents.SecurityGroupID AND UserID = @UserID)
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
			AND
			--Any Request submitted in Proj
			NOT EXISTS(SELECT NULL FROM ProjectEvents WHERE ProjectEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08')
							AND Allowed=0)
			AND
			--Any Request sent to any Org in Proj
			NOT EXISTS(SELECT NULL FROM ProjectOrganizationEvents WHERE ProjectOrganizationEvents.ProjectID=Requests.ProjectID
							AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = ProjectOrganizationEvents.SecurityGroupID AND UserID = @UserID)
							AND EventID IN ('0A850001-FC8A-4DE2-9AA5-A22200E82398','25EC0001-3AC0-45FB-AF72-A22200CC334C','B7B30001-2704-4A57-A71A-A22200CC1736','E39A0001-A4CA-46B8-B7EF-A22200E72B08')
							AND Allowed=0)
		)
	))");
            Sql(@"ALTER FUNCTION [dbo].[FilteredRequestListWithEvent]
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
	Query,
	MSRequestID,
    SourceActivityID,
    SourceActivityProjectID,
    SourceTaskOrderID
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
            Sql(@"ALTER FUNCTION [dbo].[FilteredRequestList]
(	
	@UserID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
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
	Query,
	MSRequestID,
	SourceActivityID,
	SourceActivityProjectID,
	SourceTaskOrderID
FROM Requests

WHERE
    --Rule #1: User who creates a request or submits it can always see the request
	(Requests.CreatedByID = @UserID OR Requests.SubmittedByID = @UserID)
	OR
	(
		Requests.Status >= 250
		AND
		(

			--Rule #2: User has any of these Rights: See Requests, Reject Request, Request Shared Folder View, View Request, Approve Reject Request
			(
				-- Reject Request: 0CABF382-93D3-4DAC-AA80-2DE500A5F945
				-- global, projects, datamarts, projectdatamarts, orgs
				(
					(
						--check permissions
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId))
						--Project List Request
						OR
						EXISTS
						(
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId, rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							)
						)
					)
					AND
					(
						-- make sure no denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND						
						EXISTS
						(	
							-- make sure at least one routing has permission						
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectId, rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE a.Allowed > 0)
							)
						)
					)
				)
				OR
				-- Request Shared Folder View: 5CCB0EC2-006D-4345-895E-5DD2C6C8C791
				-- global, RequestSharedFolders
				(
					(
						--check for permission
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791'))
						OR
						EXISTS
						(
							SELECT NULL FROM RequestSharedFolderRequests rsfr
							WHERE rsfr.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclRequestSharedFoldersFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791', rsfr.RequestSharedFolderID))
							)
						)
					)
					AND
					(
						-- make sure no denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS
						(
							SELECT NULL FROM RequestSharedFolderRequests rsfr
							WHERE rsfr.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclRequestSharedFoldersFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791', rsfr.RequestSharedFolderID) a WHERE a.Allowed = 0)
							)
						)
					)
				)
				OR
				-- View Request: 0549F5C8-6C0E-4491-BE90-EE0F29652422
				-- global, projects, orgs, projectorgs
				(
					(
						--check permissions
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID))
						OR
						EXISTS
						(
							SELECT NULL FROM RequestDataMarts rdm
							WHERE rdm.RequestID = Requests.ID
							AND 
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId, Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							)
						)
					)
					AND
					(
						--make sure no denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS
						(
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm
							WHERE rdm.RequestID = Requests.ID
							AND 
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectId, Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE a.Allowed > 0)
							)
						)
					)
				)
			)
			OR
			--Rule #3: User who can see requests because of a datamart can see the request so long as it's submitted: 5D6DD388-7842-40A1-A27A-B9782A445E20
			(
				Requests.Status >= 500 AND NOT Requests.SubmittedByID IS NULL
				AND
				(					
					(
						--check permission, datamarts, projects, organizations, projectdatamarts, global 
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.OrganizationID))
						OR
						EXISTS(
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId, rdm.DataMartID))
							)
						)
					)
					AND
					(
						-- check for denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId, rdm.DataMartID) a WHERE a.Allowed > 0)
							)
						)
					)
				)-- end SeeRequests permission checking
			)
			OR
			--Rule #4: User can approve or reject submission of the request: 40DB7DE2-EEFA-4D31-B400-7E72AB34DE99
			(
				-- global, projects, orgs, projectorgs
				Requests.Status >= 300 AND
				(
					(
						-- check for permission
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.OrganizationID))
						OR
						EXISTS (
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID, Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							)
						)
					)
					AND
					(
						-- check for denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS (
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID, Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE a.Allowed > 0)
							)
						)
					)
				)
			)
			OR
			--Rule #5: User can approve responses for the request
			(
				NOT Requests.SubmittedByID IS NULL AND Status >= 500
				AND
				--get the ones the user has the rights to approve regardless of if they need approval or not: A58791B5-E8AF-48D0-B9CD-ED0B54E564E6
				(
					(
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'))
						OR
						EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId))
						OR
						EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID))
						OR
						EXISTS (
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', rdm.DataMartID))
							)
						)
					)
					AND
					(
						-- check high level denies, and make sure that at least one routing is approved
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID) a WHERE a.Allowed = 0)
						AND
						NOT EXISTS (
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm 
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, rdm.DataMartID) a WHERE a.Allowed > 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', rdm.DataMartID) a WHERE a.Allowed > 0)
							)
						)
					)
				)
				-- check if there are any datamarts that need to be approved
				AND EXISTS
				(
					SELECT NULL FROM RequestDataMarts rdm 
					WHERE rdm.RequestID = Requests.ID
					AND rdm.QueryStatusTypeId <> 1 --not in draft state
					AND NOT 
					(-- look to see if the submitter can skip approval for the datamart: A0F5B621-277A-417C-A862-801D7B9030A2
						(
							EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', rdm.DataMartID))
							OR
							EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID, rdm.DataMartID))
							OR
							EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID))
							OR
							EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID)
							OR
							EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2'))
						)
						AND
						(
							NOT EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', rdm.DataMartID) a WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID, rdm.DataMartID) a WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID) a WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = rdm.DataMartID WHERE  a.Allowed = 0)
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(Requests.SubmittedByID, 'A0F5B621-277A-417C-A862-801D7B9030A2') a WHERE  a.Allowed = 0)
						)
					)--end skip approval check

				)-- end check if datamart needs approval

			)--end rule #5

		)--end of greater than draft status clause
	)--end of clause for requests user didn't create
)");

            AddColumn("dbo.Requests", "isChecked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Requests", "MirrorBudgetFields");
        }
    }
}
