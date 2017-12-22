namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddTVFsForSecurityFiltering : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[AclDataMartsFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier, @DataMartID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
 SELECT SecurityGroupID, PermissionID, Allowed FROM AclDataMarts WHERE PermissionID = @PermissionID AND DataMartID = @DataMartID AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclDataMarts.SecurityGroupID AND UserID = @UserID)
)");

            Sql(@"CREATE FUNCTION [dbo].[AclGlobalFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT SecurityGroupID, PermissionID, Allowed FROM AclGlobal WHERE PermissionID = @PermissionID  AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclGlobal.SecurityGroupID AND UserID = @UserID)
)
");
            Sql(@"CREATE FUNCTION [dbo].[AclOrganizationsFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier, @OrganizationID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT SecurityGroupID, PermissionID, Allowed FROM AclOrganizations WHERE PermissionID = @PermissionID AND OrganizationID = @OrganizationID AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclOrganizations.SecurityGroupID AND UserID = @UserID)
)
");
            Sql(@"CREATE FUNCTION [dbo].[AclProjectDataMartsFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier, @ProjectID uniqueidentifier, @DataMartID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT SecurityGroupID, PermissionID, Allowed FROM AclProjectDataMarts WHERE PermissionID = @PermissionID AND DataMartID = @DataMartID AND ProjectID = @ProjectID AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclProjectDataMarts.SecurityGroupID AND UserID = @UserID)
)");

            Sql(@"CREATE FUNCTION [dbo].[AclProjectOrganizationsFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier, @ProjectID uniqueidentifier, @OrganizationID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT SecurityGroupID, PermissionID, Allowed FROM AclProjectOrganizations WHERE PermissionID = @PermissionID AND OrganizationID = @OrganizationID AND ProjectID = @ProjectID AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclProjectOrganizations.SecurityGroupID AND UserID = @UserID)
)");

            Sql(@"CREATE FUNCTION [dbo].[AclProjectsFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier, @ProjectID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT SecurityGroupID, PermissionID, Allowed FROM AclProjects WHERE PermissionID = @PermissionID AND ProjectID = @ProjectID AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclProjects.SecurityGroupID AND UserID = @UserID)
)");

            Sql(@"CREATE FUNCTION [dbo].[AclRequestSharedFoldersFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier, @RequestSharedFolderID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT SecurityGroupID, PermissionID, Allowed FROM AclRequestSharedFolders WHERE PermissionID = @PermissionID AND RequestSharedFolderID = @RequestSharedFolderID AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclRequestSharedFolders.SecurityGroupID AND UserID = @UserID)
)");

            //And finally the actual filter for the requests
            Sql(@"CREATE FUNCTION [dbo].[FilteredRequestList]
(	
	@UserID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
SELECT Requests.* FROM Requests

WHERE
	--Rule #1: User who creates a request or submits it can always see the request
	(Requests.CreatedByID = @UserID OR Requests.SubmittedByID = @UserID)
	OR
	--Rule #2: User has any of these Rights: See Requests, Reject Request, Request Shared Folder View, View Request, Approve Reject Request
	(
		(
			--See Requests
			EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestDataMarts.RequestID = Requests.ID 
				AND
				( 
					(
						EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', RequestDataMarts.DataMartID))
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
					)
					OR
					(
						EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectID, RequestDataMarts.DataMartID))
						AND
						NOT EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectID, RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
					)
				)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20'))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20') AS a WHERE  a.Allowed = 0)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', Requests.ProjectID) AS a WHERE  a.Allowed = 0)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)	
			)
		)
		OR
		(
			-- Reject Request
			EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestDataMarts.RequestID = Requests.ID 
					AND
					( 
						(
							EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', RequestDataMarts.DataMartID))
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
						)
						OR
						(
							EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectID, RequestDataMarts.DataMartID))
							AND
							NOT EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectID, RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
						)
					)
				)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945'))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945') AS a WHERE  a.Allowed = 0)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0CABF382-93D3-4DAC-AA80-2DE500A5F945', Requests.ProjectID) AS a WHERE  a.Allowed = 0)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)	
			)

		)
		OR
		(
			--Request Shared Folder View
			EXISTS(SELECT NULL FROM RequestSharedFolderRequests JOIN RequestSharedFolders ON RequestSharedFolderRequests.RequestSharedFolderID = RequestSharedFolders.ID WHERE RequestSharedFolderRequests.RequestID = Requests.ID 
			AND
			(
				EXISTS(SELECT NULL FROM dbo.AclRequestSharedFoldersFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791', RequestSharedFolders.ID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclRequestSharedFoldersFiltered(@UserID, '5CCB0EC2-006D-4345-895E-5DD2C6C8C791', RequestSharedFolders.ID) AS a WHERE  a.Allowed = 0)	
			)
		)
		OR
		(
			--View Request
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422'))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422') AS a WHERE  a.Allowed = 0)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectID) AS a WHERE  a.Allowed = 0)
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)	
			)
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectID, Requests.OrganizationID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '0549F5C8-6C0E-4491-BE90-EE0F29652422', Requests.ProjectID, Requests.OrganizationID) AS a WHERE  a.Allowed = 0)	
			)
		)
		OR
		(
			--Approve Reject Request Submission
			Requests.Status >= 300 
			AND
			(
				(
					EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99'))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99') AS a WHERE  a.Allowed = 0)
				)
				OR
				(
					EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID) AS a WHERE  a.Allowed = 0)
				)
				OR
				(
					EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.OrganizationID))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)	
				)
				OR
				(
					EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID, Requests.OrganizationID))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID, Requests.OrganizationID) AS a WHERE  a.Allowed = 0)	
				)
			)
		)
	)

	OR
	--Rule #3: User who can see requests because of a datamart can see the request so long as it's submitted
	(EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestDataMarts.RequestID = Requests.ID
		AND 
		NOT Requests.SubmittedByID IS NULL 
		AND 
		EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '5D6DD388-7842-40A1-A27A-B9782A445E20', RequestDataMarts.DataMartID)))
	)
	OR
	--Rule #4: User can approve or reject submission of the request
	(Requests.Status >= 300 AND
		(
			EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID))
			OR
			EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.OrganizationID))
			OR
			EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', Requests.ProjectID, Requests.OrganizationID))
			OR
			EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99'))
		)
	)
	OR
	--Rule #5: User can approve responses for the request
	(
		NOT Requests.SubmittedByID IS NULL 
		AND

		--First make sure that the user didn't have the right to skip approval who submitted it
		NOT (
			EXISTS(SELECT NULL FROM RequestDataMarts WHERE Requests.ID = RequestDataMarts.RequestID AND
				(
					(
						EXISTS(SELECT NULL FROM DBO.AclDataMartsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', RequestDataMarts.DataMartID))
						AND
						NOT EXISTS(SELECT NULL FROM DBO.AclDataMartsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
					)
					OR
					(
						EXISTS(SELECT NULL FROM DBO.AclProjectDataMartsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID, RequestDataMarts.DataMartID))
						AND
						NOT EXISTS(SELECT NULL FROM DBO.AclProjectDataMartsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID, RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
					)
				)
			)	
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.ProjectID) AS a WHERE  a.Allowed = 0)
			)					
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.OrganizationID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)
			)					
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2'))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A0F5B621-277A-417C-A862-801D7B9030A2') AS a WHERE  a.Allowed = 0)
			)					
		)
		AND
		--Now make sure that the user passed has the right to Approve Responses
		(
			EXISTS(SELECT NULL FROM RequestDataMarts WHERE Requests.ID = RequestDataMarts.RequestID AND
				(
					(
						EXISTS(SELECT NULL FROM DBO.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', RequestDataMarts.DataMartID))
						AND
						NOT EXISTS(SELECT NULL FROM DBO.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
					)
					OR
					(
						EXISTS(SELECT NULL FROM DBO.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectID, RequestDataMarts.DataMartID))
						AND
						NOT EXISTS(SELECT NULL FROM DBO.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectID, RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
					)
				)
			)	
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectID) AS a WHERE  a.Allowed = 0)
			)					
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)
			)					
			OR
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'))
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') AS a WHERE  a.Allowed = 0)
			)							
		)
	)
)");
        }

        public override void Down()
        {
        }
    }
}
