namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclTables2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ACLDataMarts");
            DropPrimaryKey("dbo.AclOrganizations");
            DropPrimaryKey("dbo.AclProjects");
            AddPrimaryKey("dbo.AclDataMarts", new[] { "SecurityGroupID", "PermissionID", "DataMartID" });
            AddPrimaryKey("dbo.AclOrganizations", new[] { "SecurityGroupID", "PermissionID", "OrganizationID" });
            AddPrimaryKey("dbo.AclProjects", new[] { "SecurityGroupID", "PermissionID", "ProjectID" });
            RenameTable("dbo.ACLDataMarts", "AclDataMarts");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AclProjects");
            DropPrimaryKey("dbo.AclOrganizations");
            DropPrimaryKey("dbo.AclDataMarts");
            AddPrimaryKey("dbo.AclProjects", new[] { "SecurityGroupID", "PermissionID" });
            AddPrimaryKey("dbo.AclOrganizations", new[] { "SecurityGroupID", "PermissionID" });
            AddPrimaryKey("dbo.ACLDataMarts", new[] { "SecurityGroupID", "PermissionID" });
        }
    }
}
