namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveSecurityPermissionsUpLevels2 : DbMigration
    {
        public override void Up()
        {
            Sql(
    "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclDataMarts WHERE DataMartID = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4'");
            Sql("DELETE FROM AclDataMarts WHERE DataMartID = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4'");
            AddForeignKey("AclDataMarts", "DataMartID", "DataMarts", "ID", true);


            AddForeignKey("AclGroups", "GroupID", "Groups", "ID", true);

            Sql(
"INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclOrganizations WHERE OrganizationID = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' OR OrganizationID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclOrganizations WHERE OrganizationID = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' OR OrganizationID = '00000000-0000-0000-0000-000000000000'");
            AddForeignKey("AclOrganizations", "OrganizationID", "Organizations", "ID", true);
            AddForeignKey("AclOrganizationEvents", "OrganizationID", "Organizations", "ID", true);

            AddForeignKey("AclProjectDataMartRequestTypes", "ProjectID", "Projects", "ID", true);
            AddForeignKey("AclProjectDataMartRequestTypes", "DataMartID", "DataMarts", "ID", true);

            Sql(
"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT ProjectID, DataMartID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclProjectDataMartRequestTypes WHERE RequestTypeID = 'EC260001-2AD7-4EC9-B492-A221011E5AF8' OR RequestTypeID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclProjectDataMartRequestTypes WHERE RequestTypeID = 'EC260001-2AD7-4EC9-B492-A221011E5AF8' OR RequestTypeID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclProjectDataMartRequestTypes WHERE NOT EXISTS(SELECT NULL FROM RequestTypes WHERE ID = AclProjectDataMartRequesttypes.RequestTypeID)");

            AddForeignKey("AclProjectDataMartRequestTypes", "RequestTypeID", "RequestTypes", "ID", true);
            AddForeignKey("AclProjectDataMarts", "ProjectID", "Projects", "ID", true);

            Sql(
"INSERT INTO AclProjects (ProjectID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT ProjectID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclProjectDataMarts WHERE DataMartID = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4'");
            Sql("DELETE FROM AclProjectDataMarts WHERE DataMartID = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4'");

            AddForeignKey("AclProjectDataMarts", "DataMartID", "DataMarts", "ID", true);
            AddForeignKey("AclProjectEvents", "ProjectID", "Projects", "ID", true);
            AddForeignKey("AclProjects", "ProjectID", "Projects", "ID", true);
            AddForeignKey("AclRegistries", "RegistryID", "Registries", "ID", true);
            AddForeignKey("AclUsers", "UserID", "Users", "ID", true);

            Sql(@"ALTER TRIGGER [dbo].[DataMartsDelete] 
		ON  [dbo].[DataMarts]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
        DELETE FROM AclDataMartEvents WHERE DataMartID IN (SELECT ID FROM deleted)
	END");
            Sql(@"DROP TRIGGER GroupDelete");
            Sql(@"ALTER TRIGGER [dbo].[OrganizationDelete] 
    ON  [dbo].[Organizations]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE Organizations SET ParentOrganizationID = NULL WHERE ID IN (SELECT ID FROM deleted)
    DELETE FROM Users WHERE OrganizationID IN (SELECT ID FROM deleted)
    DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
END");

            Sql(@"DROP TRIGGER ProjectDelete");

            Sql(@"  ALTER TRIGGER [dbo].[RegistriesDelete] 
		ON  [dbo].[Registries]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
	END");
            Sql(@"DROP TRIGGER RequestTypeDelete");

            Sql(@"ALTER TRIGGER [dbo].[Users_DeleteItem] 
        ON  [dbo].[Users]
        AFTER DELETE
    AS 
    BEGIN
		DELETE FROM SecurityGroups WHERE ID IN (SELECT ID FROM deleted)
	END");
        }
        
        public override void Down()
        {
        }
    }
}
