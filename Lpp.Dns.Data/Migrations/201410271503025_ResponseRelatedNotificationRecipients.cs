namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResponseRelatedNotificationRecipients : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[ResponseRelatedNotificationRecipients]
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
  SELECT NULL FROM dbo.FilteredRequestListForEvent(UserEventSubscriptions.UserID) r 
  JOIN dbo.RequestDataMarts rdm ON rdm.RequestID=r.ID
  WHERE
  rdm.ID=@RequestDataMartID
)
AND RequestDataMartID=@RequestDataMartID
)");
        }
        
        public override void Down()
        {
            Sql(@"DROP FUNCTION [dbo].[ResponseRelatedNotificationRecipients]");
        }
    }
}
