namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecurityGroupTriggers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SecurityGroupUsers", "Overridden", c => c.Boolean(nullable: false));

            Sql(@"  CREATE TRIGGER [dbo].[SecurityGroupUsers_DeleteItem] 
                        ON  [dbo].[SecurityGroupUsers]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE SecurityGroupUsers FROM SecurityGroupUsers JOIN deleted ON SecurityGroupUsers.UserID = deleted.UserID JOIN SecurityGroups ON SecurityGroupUsers.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE SecurityGroupUsers.Overridden = 0
						END
                    END");

            Sql(@"CREATE TRIGGER [dbo].[SecurityGroupUsers_InsertItem] 
                        ON  [dbo].[SecurityGroupUsers]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
	                    if ((SELECT COUNT(*) FROM inserted) > 0)
	                    BEGIN
		                    INSERT INTO SecurityGroupUsers (SecurityGroupID, UserID) 
			                    SELECT SecurityGroups.ID, inserted.UserID FROM SecurityGroups JOIN inserted ON NOT SecurityGroups.ParentSecurityGroupID IS NULL AND SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID 
			                    WHERE NOT EXISTS(SELECT NULL FROM SecurityGroupUsers WHERE SecurityGroupUsers.SecurityGroupID = SecurityGroups.ID AND SecurityGroupUsers.UserID = inserted.UserID)
	                    END
                    END");
            Sql(@"CREATE TRIGGER [dbo].[SecurityGroups_DeleteItem] 
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

        --Add others here
	END");

            Sql(@"CREATE TRIGGER [dbo].[SecurityGroups_InsertItem] 
                       ON  [dbo].[SecurityGroups]
                       AFTER INSERT
                    AS 
                    BEGIN
	                    SET NOCOUNT ON;
	                    --Add the contacts from the inherited group
	                    INSERT INTO SecurityGroupUsers (SecurityGroupID, UserID) SELECT inserted.ID, SecurityGroupUsers.UserID FROM SecurityGroupUsers JOIN inserted ON SecurityGroupUsers.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

	                    --Add the ACL's from the inherited group
                        INSERT INTO AclDataMarts (SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
						INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclGlobal AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
						
						INSERT INTO AclGroups(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclGroups AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizationDataMarts(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizationDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizations(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizations AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMartRequestTypes(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectDataMartRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMarts(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjects(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjects AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
						INSERT INTO AclRegistries(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclRegistries AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequests(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequests AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestSharedFolders(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequestSharedFolders AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestTypes(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclUsers(SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        --Add other acl tables here


                    END");

            //This removes a user's group if they're deleted
            Sql(@"CREATE TRIGGER [dbo].[Users_DeleteItem] 
        ON  [dbo].[Users]
        AFTER DELETE
    AS 
    BEGIN
		DELETE FROM SecurityGroups WHERE ID IN (SELECT ID FROM deleted)
	END");
        }
        
        public override void Down()
        {
            DropColumn("dbo.SecurityGroupUsers", "Overridden");
        }
    }
}
