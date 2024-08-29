namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixEventPermissionFunctions : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER FUNCTION [dbo].[RequestRelatedNotificationRecipients]
(	
	@EventID uniqueidentifier,
	@RequestID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
SELECT DISTINCT dbo.Users.*
--SELECT        dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.UserID, dbo.Users.Email, dbo.Users.Phone, 
--                         dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, dbo.LogsNewRequestSubmitted.TimeStamp, dbo.LogsNewRequestSubmitted.RequestID, dbo.LogsNewRequestSubmitted.Description
FROM            dbo.LogsNewRequestSubmitted INNER JOIN
                         dbo.UserEventSubscriptions ON dbo.LogsNewRequestSubmitted.EventID = dbo.UserEventSubscriptions.EventID INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID
WHERE        (dbo.UserEventSubscriptions.EventID = @EventID) AND dbo.Users.isDeleted=0 AND dbo.Users.isActive=1 AND EXISTS
                             (SELECT        NULL AS Expr1
                               FROM            dbo.FilteredRequestListForEvent(dbo.UserEventSubscriptions.UserID, null) WHERE ID=@RequestID)
)");
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
		EXISTS(SELECT NULL FROM dbo.FilteredRequestListForEvent(UserEventSubscriptions.UserID, null) as r WHERE r.ID = @RequestID)
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
            Sql(@"ALTER FUNCTION [dbo].[RequestRelatedNotificationRecipients]
(	
	@EventID uniqueidentifier,
	@RequestID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
SELECT DISTINCT dbo.Users.*
--SELECT        dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.UserID, dbo.Users.Email, dbo.Users.Phone, 
--                         dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, dbo.LogsNewRequestSubmitted.TimeStamp, dbo.LogsNewRequestSubmitted.RequestID, dbo.LogsNewRequestSubmitted.Description
FROM            dbo.LogsNewRequestSubmitted INNER JOIN
                         dbo.UserEventSubscriptions ON dbo.LogsNewRequestSubmitted.EventID = dbo.UserEventSubscriptions.EventID INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID
WHERE        (dbo.UserEventSubscriptions.EventID = @EventID) AND dbo.Users.isDeleted=0 AND dbo.Users.isActive=1 AND EXISTS
                             (SELECT        NULL AS Expr1
                               FROM            dbo.FilteredRequestListForEvent(dbo.UserEventSubscriptions.UserID) WHERE ID=@RequestID)
)");
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
)");
        }
    }
}
