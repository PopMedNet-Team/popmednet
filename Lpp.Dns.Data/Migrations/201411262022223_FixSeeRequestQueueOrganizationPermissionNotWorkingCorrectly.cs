namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSeeRequestQueueOrganizationPermissionNotWorkingCorrectly : DbMigration
    {
        public override void Up()
        {
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
	Query
FROM Requests

WHERE
    --Rule #1: User who creates a request or submits it can always see the request
	(Requests.CreatedByID = @UserID OR Requests.SubmittedByID = @UserID)
	OR
	(
		Requests.Status >= 300
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
						EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791')) AND EXISTS(SELECT NULL FROM RequestSharedFolderRequests WHERE RequestSharedFolderRequests.RequestID = Requests.ID)
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
						EXISTS(
							SELECT NULL FROM RequestDataMarts rdm 
							JOIN DataMarts ON rdm.DataMartID = DataMarts.ID
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId, rdm.DataMartID))
								OR
								EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', DataMarts.OrganizationID))
							)
						)
					)
					AND
					(
						-- check for denies
						NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20') a WHERE a.Allowed = 0)
						AND
						NOT EXISTS(
							-- make sure at least one routing has permission
							SELECT NULL FROM RequestDataMarts rdm 
							JOIN DataMarts ON rdm.DataMartID = DataMarts.ID
							WHERE rdm.RequestID = Requests.ID
							AND
							(
								EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', rdm.DataMartID) a WHERE a.Allowed = 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectId, rdm.DataMartID) a WHERE a.Allowed = 0)
								OR
								EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', DataMarts.OrganizationID) a WHERE a.Allowed = 0)
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
        }
    }
}
