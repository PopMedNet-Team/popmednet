using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOverridenAndUpdateTriggers : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AclDataMarts", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclRequestSharedFolders", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclGroups", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclOrganizations", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclOrganizationDataMarts", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclRegistries", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclRequests", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclUsers", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclProjectDataMarts", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclProjects", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclRequestTypes", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclGlobal", "Overridden", c => c.Boolean(nullable: false));
            //AddColumn("dbo.AclProjectDataMartRequestTypes", "Overridden", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclDataMarts", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclRequestSharedFolders", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclGroups", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclOrganizations", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclOrganizationDataMarts", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclRegistries", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclRequests", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclUsers", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclProjectDataMarts", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclProjects", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclRequestTypes", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclGlobal", "Allowed", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.AclProjectDataMartRequestTypes", "Allowed", c => c.Boolean(nullable: false));

            Sql(MigrationHelpers.AddAclDeleteScript("AclDataMarts", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclGlobal", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclGroups", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclOrganizationDataMarts", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclOrganizations", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectDataMartRequestTypes", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectDataMarts", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjects", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRegistries", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRequests", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRequestSharedFolders", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRequestTypes", true));
            Sql(MigrationHelpers.AddAclDeleteScript("AclUsers", true));

            Sql(MigrationHelpers.AddAclInsertScript("AclDataMarts", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclGlobal", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclGroups", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclOrganizationDataMarts", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclOrganizations", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectDataMartRequestTypes", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectDataMarts", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjects", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclRegistries", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclRequests", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclRequestSharedFolders", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclRequestTypes", true));
            Sql(MigrationHelpers.AddAclInsertScript("AclUsers", true));

            Sql(MigrationHelpers.AddAclUpdateScript("AclDataMarts", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclGlobal", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclGroups", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclOrganizationDataMarts", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclOrganizations", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectDataMartRequestTypes", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectDataMarts", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjects", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRegistries", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRequests", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRequestSharedFolders", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRequestTypes", true));
            Sql(MigrationHelpers.AddAclUpdateScript("AclUsers", true));
        }
        
        public override void Down()
        {            
        }
    }
}
