namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLogsRoutingStatusChangedView : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [dbo].[vLogsRoutingStatusChanged]
AS
SELECT DISTINCT dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.UserID, dbo.LogsRoutingStatusChange.TimeStamp, 
                         dbo.LogsRoutingStatusChange.RequestDataMartID, dbo.LogsRoutingStatusChange.Description, dbo.Users.Email, dbo.Users.Phone, dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, 
                         dbo.RequestDataMarts.RequestID
FROM            dbo.UserEventSubscriptions INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID INNER JOIN
                         dbo.LogsRoutingStatusChange ON dbo.UserEventSubscriptions.EventID = dbo.LogsRoutingStatusChange.EventID INNER JOIN
                         dbo.RequestDataMarts ON dbo.LogsRoutingStatusChange.RequestDataMartID = dbo.RequestDataMarts.ID
WHERE EXISTS(
  SELECT NULL FROM dbo.FilteredResponseList(UserEventSubscriptions.UserID) r WHERE r.RequestDataMartID = RequestDataMarts.ID
)");
        }
        
        public override void Down()
        {
            Sql(@"ALTER VIEW [dbo].[vLogsRoutingStatusChanged]
AS
SELECT DISTINCT dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.UserID, dbo.LogsRoutingStatusChange.TimeStamp, 
                         dbo.LogsRoutingStatusChange.RequestDataMartID, dbo.LogsRoutingStatusChange.Description, dbo.Users.Email, dbo.Users.Phone, dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, 
                         dbo.RequestDataMarts.RequestID
FROM            dbo.UserEventSubscriptions INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID INNER JOIN
                         dbo.LogsRoutingStatusChange ON dbo.UserEventSubscriptions.EventID = dbo.LogsRoutingStatusChange.EventID INNER JOIN
                         dbo.RequestDataMarts ON dbo.LogsRoutingStatusChange.RequestDataMartID = dbo.RequestDataMarts.ID
WHERE EXISTS(SELECT NULL FROM dbo.FilteredRequestList(UserEventSubscriptions.UserID) r WHERE r.ID = RequestDataMarts.RequestID)");
        }
    }
}
