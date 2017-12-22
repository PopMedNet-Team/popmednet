namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclGroupEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        GroupID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.GroupID, t.EventID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.GroupID);
           
            CreateTable(
                "dbo.AclOrganizationEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.OrganizationID, t.EventID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.OrganizationID);
            
            CreateTable(
                "dbo.AclRegistryEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        RegistryID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.RegistryID, t.EventID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Registries", t => t.RegistryID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.RegistryID);
            
            CreateTable(
                "dbo.AclUserEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.UserID, t.EventID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AclDataMartEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        DataMartID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.DataMartID, t.EventID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.DataMartID);
            
            CreateTable(
                "dbo.AclProjectEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.EventID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.AclEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.EventID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID);


            var Empty = Guid.Empty.ToString();

            //All DataMart Events
            Sql(@"INSERT INTO AclDataMartEvents (DataMartID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND  (ID1 = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' OR EXISTS(SELECT NULL FROM DataMarts WHERE ID = Security_Tuple2.ID1))");

            //All Organization Events
            Sql(@"INSERT INTO AclOrganizationEvents (OrganizationID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND  (ID1 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' OR EXISTS(SELECT NULL FROM Organizations WHERE ID = Security_Tuple2.ID1))");

            //All Project Events
            Sql(@"INSERT INTO AclProjectEvents (ProjectID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND  (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple2.ID1))");

            //All User Events
            Sql(@"INSERT INTO AclUserEvents (UserID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND  (ID1 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' OR EXISTS(SELECT NULL FROM Users WHERE ID = Security_Tuple2.ID1))");

            //All Group Events
            Sql(@"INSERT INTO AclGroupEvents (GroupID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6C380001-FD30-4A47-BC64-A22100FE22EF' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND (ID1 = '6C380001-FD30-4A47-BC64-A22100FE22EF' OR EXISTS(SELECT NULL FROM Groups WHERE ID = Security_Tuple2.ID1)) GROUP BY ID1, ID2, SubjectID, PrivilegeID");

            //All Registry Events
            Sql(@"INSERT INTO AclRegistryEvents (RegistryID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND (ID1 = '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' OR EXISTS(SELECT NULL FROM Registries WHERE ID = Security_Tuple2.ID1))");
            Sql(@"DELETE FROM AclRegistryEvents WHERE RegistryID NOT IN (SELECT ID FROM Registries)");

            //Global Events
            Sql(@"INSERT INTO AclEvents (EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND ID1 = 'BBBA0001-2BC2-4E12-A5B4-A22100FDBAFD'");

            //Triggers
            Sql(@"ALTER TRIGGER [dbo].[GroupDelete] 
    ON  [dbo].[Groups]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM AclGroups WHERE GroupID IN (SELECT ID FROM deleted)
    DELETE FROM AclGroupEvents WHERE GroupID IN (SELECT ID FROM deleted)
END");

            Sql(@"ALTER TRIGGER [dbo].[OrganizationDelete] 
    ON  [dbo].[Organizations]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE Organizations SET ParentOrganizationID = NULL WHERE ID IN (SELECT ID FROM deleted)
    DELETE FROM Users WHERE OrganizationID IN (SELECT ID FROM deleted)
    DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
	DELETE FROM AclOrganizations WHERE OrganizationID IN (SELECT ID FROM deleted)
    DELETE FROM AclOrganizationEvents WHERE OrganizationID IN (SELECT ID FROM deleted)
END");

            Sql(@"ALTER TRIGGER [dbo].[ProjectDelete] 
    ON  [dbo].[Projects]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM AclProjects WHERE ProjectID IN (SELECT ID FROM deleted)
    DELETE FROM AclProjectEvents WHERE ProjectID IN (SELECT ID FROM deleted)
    DELETE FROM AclProjectDataMarts WHERE ProjectID IN (SELECT ID FROM deleted) 
    DELETE FROM AclProjectDataMartRequestTypes WHERE ProjectID IN (SELECT ID FROM deleted)
END");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectEvents", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclDataMartEvents", "DataMartID", "dbo.DataMarts");
            DropForeignKey("dbo.AclDataMartEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclDataMartEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclUserEvents", "UserID", "dbo.Users");
            DropForeignKey("dbo.AclUserEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclUserEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclRegistryEvents", "RegistryID", "dbo.Registries");
            DropForeignKey("dbo.AclRegistryEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclRegistryEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclOrganizationEvents", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.AclOrganizationEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclOrganizationEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclGroupEvents", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.AclGroupEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclGroupEvents", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.AclEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclEvents", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectEvents", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectEvents", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclDataMartEvents", new[] { "DataMartID" });
            DropIndex("dbo.AclDataMartEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclDataMartEvents", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclUserEvents", new[] { "UserID" });
            DropIndex("dbo.AclUserEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclUserEvents", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclRegistryEvents", new[] { "RegistryID" });
            DropIndex("dbo.AclRegistryEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclRegistryEvents", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclOrganizationEvents", new[] { "OrganizationID" });
            DropIndex("dbo.AclOrganizationEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclOrganizationEvents", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclGroupEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclGroupEvents", new[] { "SecurityGroupID" });
            DropTable("dbo.AclEvents");
            DropTable("dbo.AclProjectEvents");
            DropTable("dbo.AclDataMartEvents");
            DropTable("dbo.AclUserEvents");
            DropTable("dbo.AclRegistryEvents");
            DropTable("dbo.AclOrganizationEvents");
            DropTable("dbo.AclGroupEvents");
        }
    }
}
