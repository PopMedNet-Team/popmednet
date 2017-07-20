using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclDeleteTriggers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AclDataMarts", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclRequestSharedFolders", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclGroups", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclOrganizations", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclOrganizationDataMarts", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclRegistries", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclRequests", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclUsers", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclProjectDataMarts", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclProjects", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclRequestTypes", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclGlobal", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclProjectDataMartRequestTypes", "Overridden", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclDataMarts", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclRequestSharedFolders", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclGroups", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclOrganizations", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclOrganizationDataMarts", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclRegistries", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclRequests", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclUsers", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclProjectDataMarts", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclProjects", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclRequestTypes", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclGlobal", "Allowed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AclProjectDataMartRequestTypes", "Allowed", c => c.Boolean(nullable: false));

            Sql(MigrationHelpers.AddAclDeleteScript("AclDataMarts"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclGlobal"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclGroups"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclOrganizationDataMarts"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclOrganizations"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectDataMartRequestTypes"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectDataMarts"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjects"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRegistries"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRequests"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRequestSharedFolders"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRequestTypes"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclUsers"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AclProjectDataMartRequestTypes", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclGlobal", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclRequestTypes", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclProjects", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclProjectDataMarts", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclUsers", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclRequests", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclRegistries", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclOrganizationDataMarts", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclOrganizations", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclGroups", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclRequestSharedFolders", "Allowed", c => c.Boolean());
            AlterColumn("dbo.AclDataMarts", "Allowed", c => c.Boolean());
            DropColumn("dbo.AclProjectDataMartRequestTypes", "Overridden");
            DropColumn("dbo.AclGlobal", "Overridden");
            DropColumn("dbo.AclRequestTypes", "Overridden");
            DropColumn("dbo.AclProjects", "Overridden");
            DropColumn("dbo.AclProjectDataMarts", "Overridden");
            DropColumn("dbo.AclUsers", "Overridden");
            DropColumn("dbo.AclRequests", "Overridden");
            DropColumn("dbo.AclRegistries", "Overridden");
            DropColumn("dbo.AclOrganizationDataMarts", "Overridden");
            DropColumn("dbo.AclOrganizations", "Overridden");
            DropColumn("dbo.AclGroups", "Overridden");
            DropColumn("dbo.AclRequestSharedFolders", "Overridden");
            DropColumn("dbo.AclDataMarts", "Overridden");
        }
    }
}
