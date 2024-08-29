namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixReturnUserEventSubFunction : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER FUNCTION [dbo].[ReturnUserEventSubscriptionsByRequest]
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
		EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(UserEventSubscriptions.UserID) as r WHERE r.ID = @RequestID)
	AND
		UserEventSubscriptions.EventID = @EventID
	AND
		Users.IsDeleted = 0 AND Users.IsActive = 1
	AND
		(
			(@immediate = 0 AND UserEventSubscriptions.NextDueTime <= GETUTCDATE())
			OR
			UserEventSubscriptions.Frequency = 0
		)
)
");
            Sql(@"ALTER FUNCTION [dbo].[ReturnUserEventSubscriptionsByResponse]
(	
	@ResponseID uniqueidentifier,
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
		EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(UserEventSubscriptions.UserID) as req
		JOIN dbo.RequestDataMarts rdm ON req.ID=rdm.RequestID
		JOIN dbo.RequestDataMartResponses resp ON resp.RequestDataMartID=rdm.ID
		WHERE resp.ID = @ResponseID)
	AND
		UserEventSubscriptions.EventID = @EventID
	AND
		Users.IsDeleted = 0 AND Users.IsActive = 1
	AND
		(
			(@immediate = 0 AND UserEventSubscriptions.NextDueTime <= GETUTCDATE())
			OR
			UserEventSubscriptions.Frequency = 0
		)
)");
        }
        
        public override void Down()
        {
        }
    }
}
