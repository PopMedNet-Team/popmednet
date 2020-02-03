namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltervLogsNewRequestSubmitted : DbMigration
    {
        public override void Up()
        {
            Sql(@"drop view dbo.vLogsNewRequestsSubmitted");
            Sql(@"CREATE VIEW [dbo].[vLogsNewRequestsSubmitted]
AS
SELECT        dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.UserID, dbo.Users.Email, dbo.Users.Phone, 
                         dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, dbo.LogsNewRequestSubmitted.TimeStamp, dbo.LogsNewRequestSubmitted.RequestID, dbo.LogsNewRequestSubmitted.Description
FROM            dbo.LogsNewRequestSubmitted INNER JOIN
                         dbo.UserEventSubscriptions ON dbo.LogsNewRequestSubmitted.EventID = dbo.UserEventSubscriptions.EventID INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID
WHERE        (dbo.UserEventSubscriptions.EventID = '06E30001-ED86-4427-9936-A22200CC74F0') AND EXISTS
                             (SELECT        NULL AS Expr1
                               FROM            dbo.FilteredRequestListForEvent(dbo.UserEventSubscriptions.UserID) AS FilteredRequestList_2)
");
        }
        
        public override void Down()
        {
            Sql(@"drop view dbo.vLogsNewRequestsSubmitted");
            Sql(@"CREATE VIEW [dbo].[vLogsNewRequestsSubmitted]
AS
SELECT        dbo.UserEventSubscriptions.Frequency, dbo.UserEventSubscriptions.NextDueTime, dbo.UserEventSubscriptions.LastRunTime, dbo.UserEventSubscriptions.UserID, dbo.Users.Email, dbo.Users.Phone, 
                         dbo.Users.FirstName + ' ' + dbo.Users.LastName AS UserName, dbo.LogsNewRequestSubmitted.TimeStamp, dbo.LogsNewRequestSubmitted.RequestID, dbo.LogsNewRequestSubmitted.Description
FROM            dbo.LogsNewRequestSubmitted INNER JOIN
                         dbo.UserEventSubscriptions ON dbo.LogsNewRequestSubmitted.EventID = dbo.UserEventSubscriptions.EventID INNER JOIN
                         dbo.Users ON dbo.UserEventSubscriptions.UserID = dbo.Users.ID
WHERE        (dbo.UserEventSubscriptions.EventID = '06E30001-ED86-4427-9936-A22200CC74F0') AND EXISTS
                             (SELECT        NULL AS Expr1
                               FROM            dbo.FilteredRequestList(dbo.UserEventSubscriptions.UserID) AS FilteredRequestList_2)
");
        }
    }
}
