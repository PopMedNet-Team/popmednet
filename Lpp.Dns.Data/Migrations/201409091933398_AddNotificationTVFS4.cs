namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTVFS4 : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[ReturnUserEventSubscriptionsByResponse]
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
		EXISTS(SELECT NULL FROM dbo.FilteredResponseList(UserEventSubscriptions.UserID) as r WHERE r.ID = @ResponseID)
	AND
		UserEventSubscriptions.EventID = @EventID
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
