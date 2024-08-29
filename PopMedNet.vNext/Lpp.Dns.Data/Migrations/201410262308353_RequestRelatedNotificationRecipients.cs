namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestRelatedNotificationRecipients : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[RequestRelatedNotificationRecipients]
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
        }
        
        public override void Down()
        {
            Sql(@"drop function dbo.RequestRelatedNotificationRecipients");
        }
    }
}
