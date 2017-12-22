namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclTables1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ACLDataMarts",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        DataMartID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.DataMartID);           
          
            CreateTable(
                "dbo.AclOrganizations",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.OrganizationID);
            
            CreateTable(
                "dbo.AclProjects",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID);
            
            AddColumn("dbo.SecurityGroups", "Type", c => c.Int(nullable: false, defaultValue: 1));
            DropTable("dbo.RequestTypes");

            Sql(
                "UPDATE SecurityGroups SET Type = 1 WHERE EXISTS(SELECT NULL FROM Projects WHERE ID = SecurityGroups.OwnerID)");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ModelID = c.Guid(nullable: false),
                        LocalID = c.Guid(nullable: false),
                        Name = c.String(maxLength: 255),
                        Description = c.String(),
                        isMetadataRequest = c.Boolean(nullable: false),
                        CreateRequestUrl = c.String(),
                        RetrieveResponseUrl = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.AclProjects", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjects", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjects", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.ACLDataMarts", "DataMartID", "dbo.DataMarts");
            DropForeignKey("dbo.ACLDataMarts", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclOrganizations", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.AclOrganizations", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclOrganizations", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.ACLDataMarts", "PermissionIdentifiers", "dbo.Permissions");
            DropIndex("dbo.AclProjects", new[] { "ProjectID" });
            DropIndex("dbo.AclProjects", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclProjects", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclOrganizations", new[] { "OrganizationID" });
            DropIndex("dbo.AclOrganizations", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclOrganizations", new[] { "SecurityGroupID" });
            DropIndex("dbo.Permissions", new[] { "Name" });
            DropIndex("dbo.ACLDataMarts", new[] { "DataMartID" });
            DropIndex("dbo.ACLDataMarts", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.ACLDataMarts", new[] { "SecurityGroupID" });
            DropColumn("dbo.SecurityGroups", "Type");
            DropTable("dbo.AclProjects");
            DropTable("dbo.AclOrganizations");
            DropTable("dbo.Permissions");
            DropTable("dbo.ACLDataMarts");
        }
    }
}
