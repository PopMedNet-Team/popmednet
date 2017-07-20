namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTVFForFilteredResponseList : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER FUNCTION [dbo].[FilteredResponseList]
(	
	@UserID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
SELECT RequestDataMartResponses.*
    
FROM RequestDataMartResponses
JOIN RequestDataMarts ON RequestDataMartResponses.RequestDataMartID = RequestDataMarts.ID
JOIN Requests ON RequestDataMarts.RequestID = Requests.ID

WHERE
    --Rule #1 --You can see the request
	EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) AS request WHERE request.ID = RequestDataMarts.RequestID)

	--Rule #2 -- You can see the response
	AND 
	(
		--Rule a -- Can Group: F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE
		(
			--global
			-- project
			-- projectDataMart
			-- datamart
			-- org
			(
				--check has permission
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE'))
				OR
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', Requests.ProjectId))
				OR
				EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', Requests.ProjectId, RequestDataMarts.DataMartID))
				OR
				EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', RequestDataMarts.DataMartID))
				OR
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID) 
			)
			AND
			(
				-- check has no denies
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE') a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', Requests.ProjectId) a WHERE a.Allowed = 0)
				AND 
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', Requests.ProjectId, RequestDataMarts.DataMartID) a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', RequestDataMarts.DataMartID) a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID WHERE a.Allowed = 0) 
			)
		)
		OR
		--Rule b -- Can View Status: D4494B80-966A-473D-A1B3-4B18BBEF1F34
		(
			-- global
			-- project
			-- org
			-- projectOrg
			-- user
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34'))
				OR
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', Requests.ProjectId))
				OR
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID)
				OR
				EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', Requests.ProjectId, Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID)
				OR
				EXISTS(SELECT NULL FROM dbo.AclUsersFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', @UserID))
			)
			AND
			(
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34') a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', Requests.ProjectId) a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectOrganizationsFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', Requests.ProjectId, Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclUsersFiltered(@UserID, 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', @UserID) a WHERE a.Allowed = 0)
			)
		)
		OR
		--Rule c -- Can Approve Response: A58791B5-E8AF-48D0-B9CD-ED0B54E564E6
		(
			-- golbal
			-- project
			-- projectDatamart
			-- datamart
			-- org
			RequestDataMarts.QueryStatusTypeId = 10 AND
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'))
				OR
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId))
				OR
				EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, RequestDataMarts.DataMartID))
				OR
				EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', RequestDataMarts.DataMartID))
				OR
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID)
			)
			AND
			(
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId) a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, RequestDataMarts.DataMartID) a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', RequestDataMarts.DataMartID) a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.ID = RequestDataMarts.DataMartID AND dm.OrganizationID = Requests.OrganizationID WHERE a.Allowed = 0)
			)
		)
		OR
		--Rule d -- Can View Results: BDC57049-27BA-41DF-B9F9-A15ABF19B120
		(
			-- global
			-- project
			-- org
			-- user
			(
				EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120'))
				OR
				EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.ProjectId))
				OR
				EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.OrganizationID) INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = RequestDataMarts.DataMartID)
				OR
				EXISTS(SELECT NULL FROM dbo.AclUsersFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', @UserID))
			)
			AND
			(
				NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120') a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.ProjectId) a WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.OrganizationID) a INNER JOIN DataMarts dm ON dm.OrganizationID = Requests.OrganizationID AND dm.ID = RequestDataMarts.DataMartID WHERE a.Allowed = 0)
				AND
				NOT EXISTS(SELECT NULL FROM dbo.AclUsersFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', @UserID) a WHERE a.Allowed = 0)
			)
		)
		OR
		--Rule e -- Is the Creator or Submitter and the routing status is Complete
		(
			(RequestDataMarts.QueryStatusTypeId = 3 OR RequestDataMarts.QueryStatusTypeId = 14) AND (Requests.CreatedByID = @UserID OR Requests.SubmittedByID = @UserID)
		)
	)
)");
        }
        
        public override void Down()
        {
            Sql(@"ALTER FUNCTION [dbo].[FilteredResponseList]
(	
	@UserID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
SELECT RequestDataMartResponses.*
    
FROM RequestDataMartResponses
JOIN RequestDataMarts ON RequestDataMartResponses.RequestDataMartID = RequestDataMarts.ID
JOIN Requests ON RequestDataMarts.RequestID = Requests.ID

WHERE
	--Rule #1 --You can see the request
	EXISTS(SELECT NULL FROM dbo.FilteredRequestList(@UserID) AS request WHERE request.ID = RequestDataMarts.RequestID)

	--Rule #2 -- You can see the response
	AND 
	(
		( --View Results Projects
			EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.ProjectId))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.ProjectId) AS a WHERE  a.Allowed = 0)
		)
		OR
		( --Global
			EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120'))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120') AS a WHERE  a.Allowed = 0)
		)
		OR
		( --Organizations
			EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.OrganizationID))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)
		)
		OR
		( --Users
			EXISTS(SELECT NULL FROM dbo.AclUsersFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.SubmittedByID))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclUsersFiltered(@UserID, 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', Requests.SubmittedByID) AS a WHERE  a.Allowed = 0)
		)

		--They can see a response if they are the creator or the submitter and it's finished.
		OR ((RequestDataMarts.QueryStatusTypeId = 3 OR RequestDataMarts.QueryStatusTypeId = 14) AND (Requests.CreatedByID = @UserID OR Requests.SubmittedByID = @UserID))
		--They can see it because they can approve
		OR 
		(
			RequestDataMarts.QueryStatusTypeId = 10 AND
			(
				(
					EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId) AS a WHERE  a.Allowed = 0)
				)
				OR
				( --Global
					EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6') AS a WHERE  a.Allowed = 0)
				)
				OR
				( --Organizations
					EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.OrganizationID) AS a WHERE  a.Allowed = 0)
				)
				OR
				( --DataMarts
					EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', RequestDataMarts.DataMartID))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
				)
				OR
				( --Project & DataMarts
					EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, RequestDataMarts.DataMartID))
					AND
					NOT EXISTS(SELECT NULL FROM dbo.AclProjectDataMartsFiltered(@UserID, 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', Requests.ProjectId, RequestDataMarts.DataMartID) AS a WHERE  a.Allowed = 0)
				)				
			)
		)
	)
)");
        }
    }
}
