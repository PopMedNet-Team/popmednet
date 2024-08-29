using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableEventAclTriggers : DbMigration
    {
        public override void Up()
        {
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
        DELETE FROM AclUserEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclDataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclGroupEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclOrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclRegistryEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
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

                        INSERT INTO AclUserEvents(UserID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.UserID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclUserEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclDataMartEvents(DataMartID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.DataMartID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclDataMartEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclGroupEvents(GroupID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.GroupID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclGroupEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        
                        INSERT INTO AclOrganizationEvents(OrganizationID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizationEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectEvents(ProjectID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM ProjectEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclRegistryEvents(RegistryID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RegistryID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRegistryEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclEvents(EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        --Add other acl tables here


                    END");

            Sql(MigrationHelpers.AddAclDeleteScript("AclEvents"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclDataMartEvents"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclGroupEvents"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclOrganizationEvents"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectEvents"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRegistryEvents"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclUserEvents"));

            Sql(MigrationHelpers.AddAclInsertScript("AclEvents"));
            Sql(MigrationHelpers.AddAclInsertScript("AclDataMartEvents"));
            Sql(MigrationHelpers.AddAclInsertScript("AclGroupEvents"));
            Sql(MigrationHelpers.AddAclInsertScript("AclOrganizationEvents"));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectEvents"));
            Sql(MigrationHelpers.AddAclInsertScript("AclRegistryEvents"));
            Sql(MigrationHelpers.AddAclInsertScript("AclUserEvents"));

            Sql(MigrationHelpers.AddAclUpdateScript("AclEvents"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclDataMartEvents"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclGroupEvents"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclOrganizationEvents"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectEvents"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRegistryEvents"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclUserEvents"));

        }
        
        public override void Down()
        {
        }
    }
}
