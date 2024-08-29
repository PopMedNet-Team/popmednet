using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclInsertUpdateTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(MigrationHelpers.AddAclInsertScript("AclDataMarts"));
            Sql(MigrationHelpers.AddAclInsertScript("AclGlobal"));
            Sql(MigrationHelpers.AddAclInsertScript("AclGroups"));
            Sql(MigrationHelpers.AddAclInsertScript("AclOrganizationDataMarts"));
            Sql(MigrationHelpers.AddAclInsertScript("AclOrganizations"));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectDataMartRequestTypes"));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectDataMarts"));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjects"));
            Sql(MigrationHelpers.AddAclInsertScript("AclRegistries"));
            Sql(MigrationHelpers.AddAclInsertScript("AclRequests"));
            Sql(MigrationHelpers.AddAclInsertScript("AclRequestSharedFolders"));
            Sql(MigrationHelpers.AddAclInsertScript("AclRequestTypes"));
            Sql(MigrationHelpers.AddAclInsertScript("AclUsers"));

            Sql(MigrationHelpers.AddAclUpdateScript("AclDataMarts"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclGlobal"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclGroups"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclOrganizationDataMarts"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclOrganizations"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectDataMartRequestTypes"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectDataMarts"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjects"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRegistries"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRequests"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRequestSharedFolders"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRequestTypes"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclUsers"));
        }
        
        public override void Down()
        {
        }
    }
}
