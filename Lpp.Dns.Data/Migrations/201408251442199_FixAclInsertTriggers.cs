namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAclInsertTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[AclDataMarts_InsertItem] 
                        ON  [dbo].[AclDataMarts]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclDataMarts] (SecurityGroupID, PermissionID, Allowed, DataMartID) SELECT SecurityGroups.ID, inserted.PermissionID, inserted.Allowed, inserted.DataMartID FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclDataMarts] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.DataMartID = inserted.DataMartID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclGroups_InsertItem] 
                        ON  [dbo].[AclGroups]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclGroups] (SecurityGroupID, GroupID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.GroupID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclGroups] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.GroupID = inserted.GroupID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclOrganizationDataMarts_InsertItem] 
                        ON  [dbo].[AclOrganizationDataMarts]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclOrganizationDataMarts] (SecurityGroupID, OrganizationID, DataMartID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.OrganizationID, inserted.DataMartID,  inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclOrganizationDataMarts] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.OrganizationID = inserted.OrganizationID AND a.DataMartID = inserted.DataMartID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclOrganizationUsers_InsertItem] 
                        ON  [dbo].[AclOrganizationUsers]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclOrganizationUsers] (SecurityGroupID, OrganizationID, UserID, PermissionID, Allowed) SELECT SecurityGroups.ID,   inserted.OrganizationID, inserted.UserID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclOrganizationUsers] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.OrganizationID = inserted.OrganizationID AND a.UserID = inserted.UserID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclProjectDataMarts_InsertItem] 
                        ON  [dbo].[AclProjectDataMarts]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectDataMarts] (SecurityGroupID, ProjectID, DataMartID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.ProjectID, inserted.DataMartID,  inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclProjectDataMarts] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.ProjectID = inserted.ProjectID AND a.DataMartID = inserted.DataMartID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclProjectOrganizations_InsertItem] 
                        ON  [dbo].[AclProjectOrganizations]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectOrganizations] (SecurityGroupID, ProjectID, OrganizationID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.ProjectID, inserted.OrganizationID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclProjectOrganizations] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.OrganizationID = inserted.OrganizationID AND a.ProjectID = inserted.ProjectID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclProjectOrganizationUsers_InsertItem] 
                        ON  [dbo].[AclProjectOrganizationUsers]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectOrganizationUsers] (SecurityGroupID, OrganizationID, UserID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.OrganizationID, inserted.UserID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclProjectOrganizationUsers] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.OrganizationID = inserted.OrganizationID AND a.UserID = inserted.UserID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclProjects_InsertItem] 
                        ON  [dbo].[AclProjects]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjects] (SecurityGroupID, ProjectID, PermissionID, Allowed, Overridden) SELECT SecurityGroups.ID, inserted.ProjectID, inserted.PermissionID, inserted.Allowed, 0 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclProjects] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND inserted.ProjectID = a.ProjectID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[AclProjectUsers_InsertItem] 
                        ON  [dbo].[AclProjectUsers]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectUsers] (SecurityGroupID, ProjectID, UserID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.ProjectID, inserted.UserID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclProjectUsers] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.ProjectID = inserted.ProjectID AND a.UserID = inserted.UserID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclRegistries_InsertItem] 
                        ON  [dbo].[AclRegistries]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclRegistries] (SecurityGroupID, RegistryID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.RegistryID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclRegistries] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.RegistryID = inserted.RegistryID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclRequests_InsertItem] 
                        ON  [dbo].[AclRequests]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclRequests] (SecurityGroupID, RequestID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.RequestID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclRequests] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.RequestID = inserted.RequestID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclRequestSharedFolders_InsertItem] 
                        ON  [dbo].[AclRequestSharedFolders]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclRequestSharedFolders] (SecurityGroupID, RequestSharedFolderID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.RequestSharedFolderID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclRequestSharedFolders] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.RequestSharedFolderID = inserted.RequestSharedFolderID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclUsers_InsertItem] 
                        ON  [dbo].[AclUsers]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclUsers] (SecurityGroupID, UserID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.UserID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclUsers] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID AND a.UserID = inserted.UserID)
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
