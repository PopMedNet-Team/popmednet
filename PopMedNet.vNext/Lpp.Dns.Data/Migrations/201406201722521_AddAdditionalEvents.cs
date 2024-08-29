namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdditionalEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectDataMartEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        DataMartID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.DataMartID, t.EventID })
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.DataMartID);
            
            CreateTable(
                "dbo.ProjectOrganizationEvents",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.OrganizationID, t.EventID })
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.OrganizationID);

            Sql(@"INSERT INTO ProjectDataMartEvents (ProjectID, DataMartID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, ID2, ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1) AND EXISTS(SELECT NULL FROM DataMarts WHERE ID = Security_Tuple3.ID2)");
            Sql(@"INSERT INTO ProjectOrganizationEvents (ProjectID, OrganizationID, EventID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, ID2, ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9' AND EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1) AND EXISTS(SELECT NULL FROM Organizations WHERE ID = Security_Tuple3.ID2)");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectOrganizationEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.ProjectOrganizationEvents", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectOrganizationEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.ProjectOrganizationEvents", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.ProjectDataMartEvents", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.ProjectDataMartEvents", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectDataMartEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.ProjectDataMartEvents", "DataMartID", "dbo.DataMarts");
            DropIndex("dbo.ProjectOrganizationEvents", new[] { "OrganizationID" });
            DropIndex("dbo.ProjectOrganizationEvents", new[] { "ProjectID" });
            DropIndex("dbo.ProjectOrganizationEvents", new[] { "PermissionID" });
            DropIndex("dbo.ProjectOrganizationEvents", new[] { "SecurityGroupID" });
            DropIndex("dbo.ProjectDataMartEvents", new[] { "DataMartID" });
            DropIndex("dbo.ProjectDataMartEvents", new[] { "ProjectID" });
            DropIndex("dbo.ProjectDataMartEvents", new[] { "PermissionID" });
            DropIndex("dbo.ProjectDataMartEvents", new[] { "SecurityGroupID" });
            DropTable("dbo.ProjectOrganizationEvents");
            DropTable("dbo.ProjectDataMartEvents");
        }
    }
}
