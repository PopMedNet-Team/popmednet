namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTVFS3 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [dbo].[vLogsNewRequestsSubmitted]
AS
SELECT        dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.UserID, dbo.Users.Email, dbo.Users.Phone, 
                         dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, dbo.LogsNewRequestSubmitted.TimeStamp, dbo.LogsNewRequestSubmitted.RequestID, dbo.LogsNewRequestSubmitted.Description
FROM            dbo.LogsNewRequestSubmitted INNER JOIN
                         dbo.UserEventSubscriptions ON dbo.LogsNewRequestSubmitted.EventID = dbo.UserEventSubscriptions.EventID INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID
WHERE        (dbo.UserEventSubscriptions.EventID = '06E30001-ED86-4427-9936-A22200CC74F0') AND EXISTS
                             (SELECT        NULL AS Expr1
                               FROM            dbo.FilteredRequestList(dbo.UserEventSubscriptions.UserID) AS FilteredRequestList_2)");

            Sql(@"CREATE VIEW [dbo].[vLogsRoutingStatusChanged]
AS
SELECT        dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.UserID, dbo.LogsRoutingStatusChange.TimeStamp, 
                         dbo.LogsRoutingStatusChange.RequestDataMartID, dbo.LogsRoutingStatusChange.Description, dbo.Users.Email, dbo.Users.Phone, dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, 
                         dbo.RequestDataMarts.RequestID
FROM            dbo.UserEventSubscriptions INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID INNER JOIN
                         dbo.LogsRoutingStatusChange ON dbo.UserEventSubscriptions.EventID = dbo.LogsRoutingStatusChange.EventID INNER JOIN
                         dbo.RequestDataMarts ON dbo.LogsRoutingStatusChange.RequestDataMartID = dbo.RequestDataMarts.ID
");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.vLogsNewRequestsSubmitted");
            DropColumn("dbo.vLogsNewRequestsSubmitted", "Description");
            DropTable("dbo.vLogsRoutingStatusChanged");
            AddPrimaryKey("dbo.vLogsNewRequestsSubmitted", new[] { "TimeStamp", "RequestID", "UserID" });
        }
    }
}
