namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveEvents1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AclDataMartEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclOrganizationEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.ProjectDataMartEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.ProjectOrganizationEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclRegistryEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclUserEvents", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclGroupEvents", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.AclGroupEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclDataMartEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclOrganizationEvents", new[] { "PermissionID" });
            DropIndex("dbo.ProjectDataMartEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectEvents", new[] { "PermissionID" });
            DropIndex("dbo.ProjectOrganizationEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclRegistryEvents", new[] { "PermissionID" });
            DropIndex("dbo.AclUserEvents", new[] { "PermissionID" });
            DropPrimaryKey("dbo.AclProjectEvents");
            DropPrimaryKey("dbo.AclDataMartEvents");
            DropPrimaryKey("dbo.AclEvents");
            DropPrimaryKey("dbo.AclGroupEvents");
            DropPrimaryKey("dbo.AclOrganizationEvents");
            DropPrimaryKey("dbo.ProjectDataMartEvents");
            DropPrimaryKey("dbo.ProjectOrganizationEvents");
            DropPrimaryKey("dbo.AclRegistryEvents");
            DropPrimaryKey("dbo.AclUserEvents");
            AddPrimaryKey("dbo.AclProjectEvents", new[] { "SecurityGroupID", "EventID", "ProjectID" });
            AddPrimaryKey("dbo.AclDataMartEvents", new[] { "SecurityGroupID", "EventID", "DataMartID" });
            AddPrimaryKey("dbo.AclEvents", new[] { "SecurityGroupID", "EventID" });
            AddPrimaryKey("dbo.AclGroupEvents", new[] { "SecurityGroupID", "EventID", "GroupID" });
            AddPrimaryKey("dbo.AclOrganizationEvents", new[] { "SecurityGroupID", "EventID", "OrganizationID" });
            AddPrimaryKey("dbo.ProjectDataMartEvents", new[] { "SecurityGroupID", "EventID", "ProjectID", "DataMartID" });
            AddPrimaryKey("dbo.ProjectOrganizationEvents", new[] { "SecurityGroupID", "EventID", "ProjectID", "OrganizationID" });
            AddPrimaryKey("dbo.AclRegistryEvents", new[] { "SecurityGroupID", "EventID", "RegistryID" });
            AddPrimaryKey("dbo.AclUserEvents", new[] { "SecurityGroupID", "EventID", "UserID" });
            DropColumn("dbo.AclProjectEvents", "PermissionID");
            DropColumn("dbo.AclDataMartEvents", "PermissionID");
            DropColumn("dbo.AclEvents", "PermissionID");
            DropColumn("dbo.AclGroupEvents", "PermissionID");
            DropColumn("dbo.AclOrganizationEvents", "PermissionID");
            DropColumn("dbo.ProjectDataMartEvents", "PermissionID");
            DropColumn("dbo.ProjectOrganizationEvents", "PermissionID");
            DropColumn("dbo.AclRegistryEvents", "PermissionID");
            DropColumn("dbo.AclUserEvents", "PermissionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AclUserEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclRegistryEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.ProjectOrganizationEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.ProjectDataMartEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclOrganizationEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclGroupEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclDataMartEvents", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclProjectEvents", "PermissionID", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.AclUserEvents");
            DropPrimaryKey("dbo.AclRegistryEvents");
            DropPrimaryKey("dbo.ProjectOrganizationEvents");
            DropPrimaryKey("dbo.ProjectDataMartEvents");
            DropPrimaryKey("dbo.AclOrganizationEvents");
            DropPrimaryKey("dbo.AclGroupEvents");
            DropPrimaryKey("dbo.AclEvents");
            DropPrimaryKey("dbo.AclDataMartEvents");
            DropPrimaryKey("dbo.AclProjectEvents");
            AddPrimaryKey("dbo.AclUserEvents", new[] { "SecurityGroupID", "PermissionID", "UserID", "EventID" });
            AddPrimaryKey("dbo.AclRegistryEvents", new[] { "SecurityGroupID", "PermissionID", "RegistryID", "EventID" });
            AddPrimaryKey("dbo.ProjectOrganizationEvents", new[] { "SecurityGroupID", "PermissionID", "ProjectID", "OrganizationID", "EventID" });
            AddPrimaryKey("dbo.ProjectDataMartEvents", new[] { "SecurityGroupID", "PermissionID", "ProjectID", "DataMartID", "EventID" });
            AddPrimaryKey("dbo.AclOrganizationEvents", new[] { "SecurityGroupID", "PermissionID", "OrganizationID", "EventID" });
            AddPrimaryKey("dbo.AclGroupEvents", new[] { "SecurityGroupID", "PermissionID", "GroupID", "EventID" });
            AddPrimaryKey("dbo.AclEvents", new[] { "SecurityGroupID", "PermissionID", "EventID" });
            AddPrimaryKey("dbo.AclDataMartEvents", new[] { "SecurityGroupID", "PermissionID", "DataMartID", "EventID" });
            AddPrimaryKey("dbo.AclProjectEvents", new[] { "SecurityGroupID", "PermissionID", "ProjectID", "EventID" });
            CreateIndex("dbo.AclUserEvents", "PermissionID");
            CreateIndex("dbo.AclRegistryEvents", "PermissionID");
            CreateIndex("dbo.ProjectOrganizationEvents", "PermissionID");
            CreateIndex("dbo.AclProjectEvents", "PermissionID");
            CreateIndex("dbo.ProjectDataMartEvents", "PermissionID");
            CreateIndex("dbo.AclOrganizationEvents", "PermissionID");
            CreateIndex("dbo.AclEvents", "PermissionID");
            CreateIndex("dbo.AclDataMartEvents", "PermissionID");
            CreateIndex("dbo.AclGroupEvents", "PermissionID");
            AddForeignKey("dbo.AclGroupEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclUserEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclRegistryEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectOrganizationEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectDataMartEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclOrganizationEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclDataMartEvents", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
        }
    }
}
