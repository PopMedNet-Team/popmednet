namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixEventInsertScripts : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER  TRIGGER [dbo].[DataMartEvents_InsertItem] 
                        ON  [dbo].[DataMartEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [DataMartEvents] (SecurityGroupID, EventID, Allowed, Overridden) SELECT SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [DataMartEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.DataMartID = inserted.DataMartID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[GlobalEvents_InsertItem] 
                        ON  [dbo].[GlobalEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [GlobalEvents] (SecurityGroupID, EventID, Allowed, Overridden) SELECT SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [GlobalEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[GroupEvents_InsertItem] 
                        ON  [dbo].[GroupEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [GroupEvents] (GroupID, SecurityGroupID, EventID, Allowed, Overridden) SELECT GroupID, SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [GroupEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.GroupID = inserted.GroupID AND a.EventID = inserted.EventID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[OrganizationEvents_InsertItem] 
                        ON  [dbo].[OrganizationEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [OrganizationEvents] (OrganizationID, SecurityGroupID, EventID, Allowed, Overridden) SELECT inserted.OrganizationID, SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [OrganizationEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.OrganizationID = inserted.OrganizationID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[ProjectDataMartEvents_InsertItem] 
                        ON  [dbo].[ProjectDataMartEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [ProjectDataMartEvents] (ProjectID, DataMartID, SecurityGroupID, EventID, Allowed, Overridden) SELECT inserted.ProjectID, inserted.DataMartID, SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [ProjectDataMartEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.ProjectID = inserted.ProjectID AND a.DataMartID = inserted.DataMartID)
						END
                    END");
                        

            Sql(@"ALTER  TRIGGER [dbo].[ProjectEvents_InsertItem] 
                        ON  [dbo].[ProjectEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [ProjectEvents] (ProjectID, SecurityGroupID, EventID, Allowed, Overridden) SELECT inserted.ProjectID, SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [ProjectEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.ProjectID = inserted.ProjectID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[ProjectOrganizationEvents_InsertItem] 
                        ON  [dbo].[ProjectOrganizationEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [ProjectOrganizationEvents] (ProjectID, OrganizationID, SecurityGroupID, EventID, Allowed, Overridden) SELECT inserted.ProjectID, inserted.OrganizationID, SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [ProjectOrganizationEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.ProjectID = inserted.ProjectID AND a.OrganizationID = inserted.OrganizationID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[RegistryEvents_InsertItem] 
                        ON  [dbo].[RegistryEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [RegistryEvents] (RegistryID, SecurityGroupID, EventID, Allowed, Overridden) SELECT inserted.RegistryID, SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [RegistryEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.RegistryID = inserted.RegistryID)
						END
                    END");
            Sql(@"ALTER  TRIGGER [dbo].[UserEvents_InsertItem] 
                        ON  [dbo].[UserEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [UserEvents] (UserID, SecurityGroupID, EventID, Allowed, Overridden) SELECT inserted.UserID, SecurityGroups.ID, inserted.EventID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [UserEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.UserID = inserted.UserID)
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
