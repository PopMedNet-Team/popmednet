namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveEvents2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AclGroupEvents", newName: "GroupEvents");
            RenameTable(name: "dbo.AclDataMartEvents", newName: "DataMartEvents");
            RenameTable(name: "dbo.AclEvents", newName: "GlobalEvents");
            RenameTable(name: "dbo.AclOrganizationEvents", newName: "OrganizationEvents");
            RenameTable(name: "dbo.AclProjectEvents", newName: "ProjectEvents");
            RenameTable(name: "dbo.AclRegistryEvents", newName: "RegistryEvents");
            RenameTable(name: "dbo.AclUserEvents", newName: "UserEvents");

            //Update Triggers

            //Global Events
            Sql(@"DROP Trigger [dbo].[AclEvents_DeleteItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[GlobalEvents_DeleteItem] 
                        ON  [dbo].[GlobalEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [GlobalEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql(@"DROP  TRIGGER [dbo].[AclEvents_InsertItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[GlobalEvents_InsertItem] 
                        ON  [dbo].[GlobalEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [GlobalEvents] (SecurityGroupID, EventID, Allowed) SELECT SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID)
						END
                    END");

            Sql(@"DROP  TRIGGER [dbo].[AclEvents_UpdateItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[GlobalEvents_UpdateItem] 
                        ON  [dbo].[GlobalEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [GlobalEvents] SET [GlobalEvents].Allowed = inserted.Allowed FROM [AclEvents] INNER JOIN inserted ON [AclEvents].EventID = inserted.EventID JOIN SecurityGroups ON [GlobalEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [GlobalEvents].Overridden = 0
						END
                    END");

            //Data Marts
            Sql("DROP  TRIGGER [dbo].[AclDataMartEvents_DeleteItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[DataMartEvents_DeleteItem] 
                        ON  [dbo].[DataMartEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [DataMartEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.DataMartID = deleted.DataMartID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");
            Sql(@"DROP  TRIGGER [dbo].[AclDataMartEvents_InsertItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[DataMartEvents_InsertItem] 
                        ON  [dbo].[DataMartEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [DataMartEvents] (SecurityGroupID, EventID, Allowed) SELECT SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [DataMartEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.DataMartID = inserted.DataMartID)
						END
                    END");
            Sql(@"DROP  TRIGGER [dbo].[AclDataMartEvents_UpdateItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[DataMartEvents_UpdateItem] 
                        ON  [dbo].[DataMartEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [DataMartEvents] SET [DataMartEvents].Allowed = inserted.Allowed FROM [DataMartEvents] INNER JOIN inserted ON [DataMartEvents].EventID = inserted.EventID AND [DataMartEvents].DataMartID = inserted.DataMartID JOIN SecurityGroups ON [DataMartEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [DataMartEvents].Overridden = 0
						END
                    END");

            //Groups
            Sql("DROP  TRIGGER [dbo].[AclGroupEvents_DeleteItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[GroupEvents_DeleteItem] 
                        ON  [dbo].[GroupEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [GroupEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.GroupID = deleted.GroupID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql("DROP  TRIGGER [dbo].[AclGroupEvents_InsertItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[GroupEvents_InsertItem] 
                        ON  [dbo].[GroupEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [GroupEvents] (GroupID, SecurityGroupID, EventID, Allowed) SELECT GroupID, SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [GroupEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.GroupID = inserted.GroupID AND a.EventID = inserted.EventID)
						END
                    END");
            Sql(@"DROP  TRIGGER [dbo].[AclGroupEvents_UpdateItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[GroupEvents_UpdateItem] 
                        ON  [dbo].[GroupEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [GroupEvents] SET [GroupEvents].Allowed = inserted.Allowed FROM [GroupEvents] INNER JOIN inserted ON [GroupEvents].EventID = inserted.EventID AND [GroupEvents].GroupID = inserted.GroupID JOIN SecurityGroups ON [GroupEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [GroupEvents].Overridden = 0
						END
                    END");

            //Organizations
            Sql(@"DROP  TRIGGER [dbo].[AclOrganizationEvents_DeleteItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[OrganizationEvents_DeleteItem] 
                        ON  [dbo].[OrganizationEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [OrganizationEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.OrganizationID = deleted.OrganizationID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");
            Sql(@"DROP  TRIGGER [dbo].[AclOrganizationEvents_InsertItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[OrganizationEvents_InsertItem] 
                        ON  [dbo].[OrganizationEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [OrganizationEvents] (OrganizationID, SecurityGroupID, EventID, Allowed) SELECT inserted.OrganizationID, SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [OrganizationEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.OrganizationID = inserted.OrganizationID)
						END
                    END");
            Sql(@"DROP  TRIGGER [dbo].[AclOrganizationEvents_UpdateItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[OrganizationEvents_UpdateItem] 
                        ON  [dbo].[OrganizationEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [OrganizationEvents] SET [OrganizationEvents].Allowed = inserted.Allowed FROM [OrganizationEvents] INNER JOIN inserted ON [OrganizationEvents].EventID = inserted.EventID AND [OrganizationEvents].OrganizationID = inserted.OrganizationID JOIN SecurityGroups ON [OrganizationEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [OrganizationEvents].Overridden = 0
						END
                    END");

            //Project DataMart Events
            Sql(@"CREATE  TRIGGER [dbo].[ProjectDataMartEvents_DeleteItem] 
                        ON  [dbo].[ProjectDataMartEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [ProjectDataMartEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.ProjectID = deleted.ProjectID AND a.DataMartID = deleted.DataMartID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");
            Sql(@"CREATE  TRIGGER [dbo].[ProjectDataMartEvents_InsertItem] 
                        ON  [dbo].[ProjectDataMartEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [ProjectDataMartEvents] (ProjectID, DataMartID, SecurityGroupID, EventID, Allowed) SELECT inserted.ProjectID, inserted.DataMartID, SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [ProjectDataMartEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.ProjectID = inserted.ProjectID AND a.DataMartID = inserted.DataMartID)
						END
                    END");
            Sql(@"CREATE  TRIGGER [dbo].[ProjectDataMartEvents_UpdateItem] 
                        ON  [dbo].[ProjectDataMartEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [ProjectDataMartEvents] SET [ProjectDataMartEvents].Allowed = inserted.Allowed FROM [ProjectDataMartEvents] INNER JOIN inserted ON [ProjectDataMartEvents].EventID = inserted.EventID JOIN SecurityGroups ON [ProjectDataMartEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [ProjectDataMartEvents].Overridden = 0
						END
                    END");

            //Projects
            Sql("DROP  TRIGGER [dbo].[AclProjectEvents_DeleteItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[ProjectEvents_DeleteItem] 
                        ON  [dbo].[ProjectEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [ProjectEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.ProjectID = deleted.ProjectID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql("DROP  TRIGGER [dbo].[AclProjectEvents_InsertItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[ProjectEvents_InsertItem] 
                        ON  [dbo].[ProjectEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [ProjectEvents] (ProjectID, SecurityGroupID, EventID, Allowed) SELECT inserted.ProjectID, SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [ProjectEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.ProjectID = inserted.ProjectID)
						END
                    END");
            Sql("DROP  TRIGGER [dbo].[AclProjectEvents_UpdateItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[ProjectEvents_UpdateItem] 
                        ON  [dbo].[ProjectEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [ProjectEvents] SET [ProjectEvents].Allowed = inserted.Allowed FROM [ProjectEvents] INNER JOIN inserted ON [ProjectEvents].EventID = inserted.EventID AND [ProjectEvents].ProjectID = inserted.ProjectID JOIN SecurityGroups ON [ProjectEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [ProjectEvents].Overridden = 0
						END
                    END");

            //Project Organizations
            Sql(@"CREATE  TRIGGER [dbo].[ProjectOrganizationEvents_DeleteItem] 
                        ON  [dbo].[ProjectOrganizationEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [ProjectOrganizationEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.ProjectID = deleted.ProjectID AND a.OrganizationID = deleted.OrganizationID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql(@"CREATE  TRIGGER [dbo].[ProjectOrganizationEvents_InsertItem] 
                        ON  [dbo].[ProjectOrganizationEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [ProjectOrganizationEvents] (ProjectID, OrganizationID, SecurityGroupID, EventID, Allowed) SELECT inserted.ProjectID, inserted.OrganizationID, SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [ProjectOrganizationEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.ProjectID = inserted.ProjectID AND a.OrganizationID = inserted.OrganizationID)
						END
                    END");

            Sql(@"CREATE  TRIGGER [dbo].[ProjectOrganizationEvents_UpdateItem] 
                        ON  [dbo].[ProjectOrganizationEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [ProjectOrganizationEvents] SET [ProjectOrganizationEvents].Allowed = inserted.Allowed FROM [ProjectOrganizationEvents] INNER JOIN inserted ON [ProjectOrganizationEvents].EventID = inserted.EventID AND [ProjectOrganizationEvents].ProjectID = inserted.ProjectID AND [ProjectOrganizationEvents].OrganizationID = inserted.OrganizationID JOIN SecurityGroups ON [ProjectOrganizationEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [ProjectOrganizationEvents].Overridden = 0
						END
                    END");

            //Registries
            Sql("DROP  TRIGGER [dbo].[AclRegistryEvents_DeleteItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[RegistryEvents_DeleteItem] 
                        ON  [dbo].[RegistryEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [RegistryEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.RegistryID = deleted.RegistryID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");
            Sql("DROP  TRIGGER [dbo].[AclRegistryEvents_InsertItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[RegistryEvents_InsertItem] 
                        ON  [dbo].[RegistryEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [RegistryEvents] (RegistryID, SecurityGroupID, EventID, Allowed) SELECT inserted.RegistryID, SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [RegistryEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.RegistryID = inserted.RegistryID)
						END
                    END");
            Sql(@"DROP  TRIGGER [dbo].[AclRegistryEvents_UpdateItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[RegistryEvents_UpdateItem] 
                        ON  [dbo].[RegistryEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [RegistryEvents] SET [RegistryEvents].Allowed = inserted.Allowed FROM [RegistryEvents] INNER JOIN inserted ON [RegistryEvents].EventID = inserted.EventID AND [RegistryEvents].RegistryID = inserted.RegistryID JOIN SecurityGroups ON [RegistryEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [RegistryEvents].Overridden = 0
						END
                    END");

            //Users
            Sql(@"DROP  TRIGGER [dbo].[AclUserEvents_DeleteItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[UserEvents_DeleteItem] 
                        ON  [dbo].[UserEvents]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [UserEvents] a INNER JOIN deleted ON a.EventID = deleted.EventID AND a.UserID = deleted.UserID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");

            Sql(@"DROP  TRIGGER [dbo].[AclUserEvents_InsertItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[UserEvents_InsertItem] 
                        ON  [dbo].[UserEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [UserEvents] (UserID, SecurityGroupID, EventID, Allowed) SELECT inserted.UserID, SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [UserEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID AND a.UserID = inserted.UserID)
						END
                    END");
            Sql(@"DROP  TRIGGER [dbo].[AclUserEvents_UpdateItem] ");
            Sql(@"CREATE  TRIGGER [dbo].[UserEvents_UpdateItem] 
                        ON  [dbo].[UserEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [UserEvents] SET [UserEvents].Allowed = inserted.Allowed FROM [UserEvents] INNER JOIN inserted ON [UserEvents].EventID = inserted.EventID AND [UserEvents].UserID = inserted.UserID JOIN SecurityGroups ON [UserEvents].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [UserEvents].Overridden = 0
						END
                    END");


            //Overall triggers

            //Security Groups
            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_DeleteItem] 
        ON  [dbo].[SecurityGroups]
        AFTER DELETE
    AS 
    BEGIN
		UPDATE SecurityGroups SET ParentSecurityGroupID = NULL WHERE ParentSecurityGroupID IN (SELECT ID FROM deleted)

        DELETE FROM AclDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGlobal WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGroups WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizationDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMartRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjects WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRegistries WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequests WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestSharedFolders WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM UserEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM GlobalEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM DataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM GroupEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM OrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM RegistryEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclOrganizationUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectOrganizationUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectOrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectDataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        --Add others here
	END");

            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_InsertItem] 
                       ON  [dbo].[SecurityGroups]
                       AFTER INSERT
                    AS 
                    BEGIN
	                    SET NOCOUNT ON;
	                    --Add the contacts from the inherited group
	                    INSERT INTO SecurityGroupUsers (SecurityGroupID, UserID) SELECT inserted.ID, SecurityGroupUsers.UserID FROM SecurityGroupUsers JOIN inserted ON SecurityGroupUsers.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

	                    --Add the ACL's from the inherited group
                        INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed) SELECT acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclGlobal AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
						
						INSERT INTO AclGroups(GroupID, SecurityGroupID, PermissionID, Allowed) SELECT acl.GroupID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclGroups AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizationDataMarts(OrganizationID, DataMartID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizationDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizations(OrganizationID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizations AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMartRequestTypes(ProjectID, DataMartID, RequestTypeID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.DataMartID, acl.RequestTypeID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectDataMartRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMarts(ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjects(ProjectID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjects AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRegistries(RegistryID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RegistryID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRegistries AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequests(RequestID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RequestID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequests AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestSharedFolders(RequestSharedFolderID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RequestSharedFolderID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequestSharedFolders AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestTypes(RequestTypeID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RequestTypeID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclUsers(UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO UserEvents(UserID, EventID, SecurityGroupID, Allowed) SELECT acl.UserID, acl.EventID, inserted.ID, acl.Allowed FROM UserEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO DataMartEvents(DataMartID, EventID, SecurityGroupID, Allowed) SELECT acl.DataMartID, acl.EventID, inserted.ID, acl.Allowed FROM DataMartEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO GroupEvents(GroupID, EventID, SecurityGroupID, Allowed) SELECT acl.GroupID, acl.EventID, inserted.ID, acl.Allowed FROM GroupEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        
                        INSERT INTO OrganizationEvents(OrganizationID, EventID, SecurityGroupID, Allowed) SELECT acl.OrganizationID, acl.EventID, inserted.ID, acl.Allowed FROM OrganizationEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO ProjectEvents(ProjectID, EventID, SecurityGroupID, Allowed) SELECT acl.ProjectID, acl.EventID, inserted.ID, acl.Allowed FROM ProjectEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO RegistryEvents(RegistryID, EventID, SecurityGroupID, Allowed) SELECT acl.RegistryID, acl.EventID, inserted.ID, acl.Allowed FROM RegistryEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO GlobalEvents(EventID, SecurityGroupID, Allowed) SELECT acl.EventID, inserted.ID, acl.Allowed FROM GlobalEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclOrganizationUsers(OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizationUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectUsers(ProjectID, UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectOrganizationUsers(ProjectID, OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.OrganizationID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectOrganizationUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectOrganizations(ProjectID, OrganizationID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.OrganizationID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectOrganizations AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO ProjectOrganizationEvents(ProjectID, OrganizationID, SecurityGroupID, EventID, Allowed) SELECT acl.ProjectID, acl.OrganizationID, inserted.ID, acl.EventID, acl.Allowed FROM ProjectOrganizationEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        
                        INSERT INTO ProjectDataMartEvents(ProjectID, DataMartID, SecurityGroupID, EventID, Allowed) SELECT acl.ProjectID, acl.DataMartID, inserted.ID, acl.EventID, acl.Allowed FROM ProjectDataMartEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        --Add other acl tables here
                    END");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserEvents", newName: "AclUserEvents");
            RenameTable(name: "dbo.RegistryEvents", newName: "AclRegistryEvents");
            RenameTable(name: "dbo.ProjectEvents", newName: "AclProjectEvents");
            RenameTable(name: "dbo.OrganizationEvents", newName: "AclOrganizationEvents");
            RenameTable(name: "dbo.GlobalEvents", newName: "AclEvents");
            RenameTable(name: "dbo.DataMartEvents", newName: "AclDataMartEvents");
            RenameTable(name: "dbo.GroupEvents", newName: "AclGroupEvents");
        }
    }
}
