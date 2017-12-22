namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixResponseNeedsApprovalNotificationNotShowing : DbMigration
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
        
        public override void Down()
        {
        }
    }
}
