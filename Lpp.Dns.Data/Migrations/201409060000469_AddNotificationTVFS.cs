namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTVFS : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[AclUsersFiltered] (@UserID uniqueidentifier, @PermissionID uniqueidentifier, @TargetUserID uniqueidentifier)
RETURNS TABLE 
AS
RETURN 
(
	SELECT SecurityGroupID, PermissionID, Allowed FROM AclUsers WHERE PermissionID = @PermissionID AND UserID = @TargetUserID AND EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupID = AclUsers.SecurityGroupID AND UserID = @UserID)
)
");

            Sql(@"CREATE FUNCTION [dbo].[ReturnUserEventSubscriptionsByRequest]
(	
	@RequestID uniqueidentifier,
	@EventID uniqueidentifier,
	@immediate bit
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT UserEventSubscriptions.*, Users.FirstName + ' ' + Users.LastName AS UserName, Users.Email, Users.Phone
	FROM UserEventSubscriptions
	JOIN Users ON UserEventSubscriptions.UserID = Users.ID
	WHERE 
		EXISTS(SELECT NULL FROM dbo.FilteredRequestList(UserEventSubscriptions.UserID) as r WHERE r.ID = @RequestID)
	AND
		UserEventSubscriptions.EventID = @EventID
	AND
		(
			(@immediate = 0 AND UserEventSubscriptions.NextDueTime <= GETUTCDATE())
			OR
			UserEventSubscriptions.Frequency = 0
		)
)");

            Sql(@"CREATE FUNCTION [dbo].[FilteredResponseList]
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
	)
)");
        }
        
        public override void Down()
        {
        }
    }
}
