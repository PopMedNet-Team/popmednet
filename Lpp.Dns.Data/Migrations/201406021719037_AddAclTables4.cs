namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclTables4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclRequestSharedFolders",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        RequestSharedFolderID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.RequestSharedFolderID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.RequestSharedFolders", t => t.RequestSharedFolderID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.RequestSharedFolderID);
            
            CreateTable(
                "dbo.AclGroups",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        GroupID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.GroupID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.AclOrganizationDataMarts",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        DataMartID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.OrganizationID, t.DataMartID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: false)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.OrganizationID)
                .Index(t => t.DataMartID);
            
            CreateTable(
                "dbo.AclRegistries",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        RegistryID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.RegistryID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Registries", t => t.RegistryID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.RegistryID);
            
            CreateTable(
                "dbo.AclUsers",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.UserID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AclProjectDataMarts",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        DataMartID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.DataMartID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.DataMartID);
            
            CreateTable(
                "dbo.AclGlobal",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID);
            
            CreateTable(
                "dbo.AclProjectDataMartRequestTypes",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        DataMartID = c.Guid(nullable: false),
                        RequestTypeID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.DataMartID, t.RequestTypeID })
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.DataMartID)
                .Index(t => t.RequestTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclProjectDataMartRequestTypes", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectDataMartRequestTypes", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.AclProjectDataMartRequestTypes", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectDataMartRequestTypes", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectDataMartRequestTypes", "DataMartID", "dbo.DataMarts");
            DropForeignKey("dbo.AclGlobal", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclGlobal", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectDataMarts", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectDataMarts", "DataMartID", "dbo.DataMarts");
            DropForeignKey("dbo.AclProjectDataMarts", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectDataMarts", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.AclOrganizationDataMarts", "DataMartID", "dbo.DataMarts");
            DropForeignKey("dbo.AclUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.AclUsers", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclUsers", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.AclRegistries", "RegistryID", "dbo.Registries");
            DropForeignKey("dbo.AclRegistries", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclRegistries", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.AclOrganizationDataMarts", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.AclOrganizationDataMarts", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclOrganizationDataMarts", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.AclGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.AclGroups", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclGroups", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.AclRequestSharedFolders", "RequestSharedFolderID", "dbo.RequestSharedFolders");
            DropForeignKey("dbo.AclRequestSharedFolders", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclRequestSharedFolders", "PermissionIdentifiers", "dbo.Permissions");
            DropIndex("dbo.AclProjectDataMartRequestTypes", new[] { "RequestTypeID" });
            DropIndex("dbo.AclProjectDataMartRequestTypes", new[] { "DataMartID" });
            DropIndex("dbo.AclProjectDataMartRequestTypes", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectDataMartRequestTypes", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclProjectDataMartRequestTypes", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclGlobal", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclGlobal", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectDataMarts", new[] { "DataMartID" });
            DropIndex("dbo.AclProjectDataMarts", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectDataMarts", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclProjectDataMarts", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclUsers", new[] { "UserID" });
            DropIndex("dbo.AclUsers", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclUsers", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclRegistries", new[] { "RegistryID" });
            DropIndex("dbo.AclRegistries", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclRegistries", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclOrganizationDataMarts", new[] { "DataMartID" });
            DropIndex("dbo.AclOrganizationDataMarts", new[] { "OrganizationID" });
            DropIndex("dbo.AclOrganizationDataMarts", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclOrganizationDataMarts", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclGroups", new[] { "GroupID" });
            DropIndex("dbo.AclGroups", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclGroups", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclRequestSharedFolders", new[] { "RequestSharedFolderID" });
            DropIndex("dbo.AclRequestSharedFolders", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclRequestSharedFolders", new[] { "SecurityGroupID" });
            DropTable("dbo.AclProjectDataMartRequestTypes");
            DropTable("dbo.AclGlobal");
            DropTable("dbo.AclProjectDataMarts");
            DropTable("dbo.AclUsers");
            DropTable("dbo.AclRegistries");
            DropTable("dbo.AclOrganizationDataMarts");
            DropTable("dbo.AclGroups");
            DropTable("dbo.AclRequestSharedFolders");
        }
    }
}
