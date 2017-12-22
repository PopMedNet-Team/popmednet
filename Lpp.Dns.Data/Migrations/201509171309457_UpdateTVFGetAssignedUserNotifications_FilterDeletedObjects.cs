namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTVFGetAssignedUserNotifications_FilterDeletedObjects : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER FUNCTION [dbo].[GetAssignedUserNotifications]
                    (	
	                    @UserID uniqueidentifier
                    )
                    RETURNS TABLE 
                    AS
                    RETURN 
                    (
	                    Select Events.Name As [Event], EventID, '' As [Description], 'Global' As [Level] from GlobalEvents Inner Join Events On Events.ID = EventID Where SecurityGroupID IN (Select SecurityGroupID from SecurityGroupUsers where UserID = @UserID)
						Union
						Select Events.Name As [Event], EventID, Projects.Name As [Description], 'Project' As [Level] from ProjectEvents Inner Join Events On Events.ID = EventID Inner Join Projects on Projects.ID = ProjectID Where Projects.isDeleted = 0 AND SecurityGroupID IN (Select SecurityGroupID from SecurityGroupUsers where UserID = @UserID)
						Union
						Select Events.Name As [Event], EventID, Organizations.Name As [Description], 'Organization' As [Level] from OrganizationEvents Inner Join Events On Events.ID = EventID Inner Join Organizations on Organizations.ID = OrganizationID Where SecurityGroupID IN (Select SecurityGroupID from SecurityGroupUsers where UserID = @UserID)
						Union
						Select Events.Name As [Event], EventID, DataMarts.Name As [Description], 'Datamart' As [Level] from DataMartEvents Inner Join Events On Events.ID = EventID Inner Join DataMarts on DataMarts.ID = DataMartID Where DataMarts.isDeleted = 0 AND SecurityGroupID IN (Select SecurityGroupID from SecurityGroupUsers where UserID = @UserID)
						Union
						Select Events.Name As [Event], EventID, DataMarts.Name + ' / ' + Projects.Name As [Description], 'Datamart w/in Project' As [Level] from ProjectDataMartEvents Inner Join Events On Events.ID = EventID Inner Join Projects on Projects.ID = ProjectID Inner Join DataMarts on DataMarts.ID = DataMartID Where DataMarts.isDeleted = 0 AND Projects.isDeleted = 0 AND SecurityGroupID IN (Select SecurityGroupID from SecurityGroupUsers where UserID = @UserID)
                    )");
        }
        
        public override void Down()
        {
        }
    }
}
