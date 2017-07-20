namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixInheritedSecurityGroupCreateFailOverridenNull : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_InsertItem] 
                       ON  [dbo].[SecurityGroups]
                       AFTER INSERT
                    AS 
                    BEGIN
	                    SET NOCOUNT ON;
	                    --Add the contacts from the inherited group
	                    INSERT INTO SecurityGroupUsers (SecurityGroupID, UserID) SELECT inserted.ID, SecurityGroupUsers.UserID FROM SecurityGroupUsers JOIN inserted ON SecurityGroupUsers.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

	                    --Add the ACL's from the inherited group
                        INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed,0 FROM AclDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden) SELECT inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclGlobal AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
						
						INSERT INTO AclGroups(GroupID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.GroupID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclGroups AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizationDataMarts(OrganizationID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.OrganizationID, acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclOrganizationDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizations(OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.OrganizationID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclOrganizations AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMartRequestTypes(ProjectID, DataMartID, RequestTypeID, SecurityGroupID, Permission, Overridden) SELECT acl.ProjectID, acl.DataMartID, acl.RequestTypeID, inserted.ID, acl.Permission, 0 FROM AclProjectDataMartRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMarts(ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.ProjectID, acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclProjectDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjects(ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.ProjectID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclProjects AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRegistries(RegistryID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.RegistryID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclRegistries AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequests(RequestID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.RequestID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclRequests AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestSharedFolders(RequestSharedFolderID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.RequestSharedFolderID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclRequestSharedFolders AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestTypes(RequestTypeID, SecurityGroupID, Permission, Overridden) SELECT acl.RequestTypeID, inserted.ID, acl.Permission, 0 FROM AclRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclUsers(UserID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO UserEvents(UserID, EventID, SecurityGroupID, Allowed, Overridden) SELECT acl.UserID, acl.EventID, inserted.ID, acl.Allowed, 0 FROM UserEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO DataMartEvents(DataMartID, EventID, SecurityGroupID, Allowed, Overridden) SELECT acl.DataMartID, acl.EventID, inserted.ID, acl.Allowed, 0 FROM DataMartEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO GroupEvents(GroupID, EventID, SecurityGroupID, Allowed, Overridden) SELECT acl.GroupID, acl.EventID, inserted.ID, acl.Allowed, 0 FROM GroupEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        
                        INSERT INTO OrganizationEvents(OrganizationID, EventID, SecurityGroupID, Allowed, Overridden) SELECT acl.OrganizationID, acl.EventID, inserted.ID, acl.Allowed, 0 FROM OrganizationEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO ProjectEvents(ProjectID, EventID, SecurityGroupID, Allowed, Overridden) SELECT acl.ProjectID, acl.EventID, inserted.ID, acl.Allowed, 0 FROM ProjectEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO RegistryEvents(RegistryID, EventID, SecurityGroupID, Allowed, Overridden) SELECT acl.RegistryID, acl.EventID, inserted.ID, acl.Allowed, 0 FROM RegistryEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO GlobalEvents(EventID, SecurityGroupID, Allowed, Overridden) SELECT acl.EventID, inserted.ID, acl.Allowed, 0 FROM GlobalEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclOrganizationUsers(OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.OrganizationID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclOrganizationUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectUsers(ProjectID, UserID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.ProjectID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclProjectUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectOrganizationUsers(ProjectID, OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.ProjectID, acl.OrganizationID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclProjectOrganizationUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectOrganizations(ProjectID, OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden) SELECT acl.ProjectID, acl.OrganizationID, inserted.ID, acl.PermissionID, acl.Allowed, 0 FROM AclProjectOrganizations AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO ProjectOrganizationEvents(ProjectID, OrganizationID, SecurityGroupID, EventID, Allowed, Overridden) SELECT acl.ProjectID, acl.OrganizationID, inserted.ID, acl.EventID, acl.Allowed, 0 FROM ProjectOrganizationEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        
                        INSERT INTO ProjectDataMartEvents(ProjectID, DataMartID, SecurityGroupID, EventID, Allowed, Overridden) SELECT acl.ProjectID, acl.DataMartID, inserted.ID, acl.EventID, acl.Allowed, 0 FROM ProjectDataMartEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        --Add other acl tables here
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
