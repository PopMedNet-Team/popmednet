namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityOptionsThatShouldntBeThereAndEnableRequestTypeSecurity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AclProjectOrganizationUsers", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectUsers", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectUsers", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectUsers", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.AclProjectUsers", "UserID", "dbo.Users");
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "OrganizationID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "UserID" });
            DropIndex("dbo.AclProjectUsers", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectUsers", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectUsers", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectUsers", new[] { "UserID" });
            DropPrimaryKey("dbo.AclRequestTypes");
            AddColumn("dbo.AclRequestTypes", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclRequestTypes", "Allowed", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.AclRequestTypes", new[] { "SecurityGroupID", "PermissionID" });
            CreateIndex("dbo.AclRequestTypes", "PermissionID");
            AddForeignKey("dbo.AclRequestTypes", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            DropColumn("dbo.AclRequestTypes", "Permission");
            DropTable("dbo.AclProjectOrganizationUsers");
            DropTable("dbo.AclProjectUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AclProjectUsers",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.UserID });
            
            CreateTable(
                "dbo.AclProjectOrganizationUsers",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.OrganizationID, t.UserID });
            
            AddColumn("dbo.AclRequestTypes", "Permission", c => c.Int(nullable: false));
            DropForeignKey("dbo.AclRequestTypes", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.AclRequestTypes", new[] { "PermissionID" });
            DropPrimaryKey("dbo.AclRequestTypes");
            DropColumn("dbo.AclRequestTypes", "Allowed");
            DropColumn("dbo.AclRequestTypes", "PermissionID");
            AddPrimaryKey("dbo.AclRequestTypes", new[] { "SecurityGroupID", "RequestTypeID", "Permission" });
            CreateIndex("dbo.AclProjectUsers", "UserID");
            CreateIndex("dbo.AclProjectUsers", "ProjectID");
            CreateIndex("dbo.AclProjectUsers", "PermissionID");
            CreateIndex("dbo.AclProjectUsers", "SecurityGroupID");
            CreateIndex("dbo.AclProjectOrganizationUsers", "UserID");
            CreateIndex("dbo.AclProjectOrganizationUsers", "OrganizationID");
            CreateIndex("dbo.AclProjectOrganizationUsers", "ProjectID");
            CreateIndex("dbo.AclProjectOrganizationUsers", "PermissionID");
            CreateIndex("dbo.AclProjectOrganizationUsers", "SecurityGroupID");
            AddForeignKey("dbo.AclProjectUsers", "UserID", "dbo.Users", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectOrganizationUsers", "UserID", "dbo.Users", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectUsers", "ProjectID", "dbo.Projects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectUsers", "SecurityGroupID", "dbo.SecurityGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectUsers", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectOrganizationUsers", "ProjectID", "dbo.Projects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectOrganizationUsers", "OrganizationID", "dbo.Organizations", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectOrganizationUsers", "SecurityGroupID", "dbo.SecurityGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectOrganizationUsers", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
        }
    }
}
