namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequesttypeAclsToUsePermissionEnum : DbMigration
    {
        public override void Up()
        {
            //Drop all of the data, as we're going to put it in from the legacy tables.
            Sql("TRUNCATE TABLE AclDataMartRequestTypes");
            Sql("TRUNCATE TABLE AclProjectDataMartRequestTypes");
            Sql("TRUNCATE TABLE AclRequestTypes");

            DropForeignKey("dbo.AclDataMartRequestTypes", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectDataMartRequestTypes", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclRequestTypes", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.AclDataMartRequestTypes", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectDataMartRequestTypes", new[] { "PermissionID" });
            DropIndex("dbo.AclRequestTypes", new[] { "PermissionID" });
            DropPrimaryKey("dbo.AclDataMartRequestTypes");
            DropPrimaryKey("dbo.AclProjectDataMartRequestTypes");
            DropPrimaryKey("dbo.AclRequestTypes");
            AddColumn("dbo.AclDataMartRequestTypes", "Permission", c => c.Int(nullable: false));
            AddColumn("dbo.AclProjectDataMartRequestTypes", "Permission", c => c.Int(nullable: false));
            AddColumn("dbo.AclRequestTypes", "Permission", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AclDataMartRequestTypes", new[] { "SecurityGroupID", "RequestTypeID", "Permission", "DataMartID" });
            AddPrimaryKey("dbo.AclProjectDataMartRequestTypes", new[] { "SecurityGroupID", "RequestTypeID", "Permission", "ProjectID", "DataMartID" });
            AddPrimaryKey("dbo.AclRequestTypes", new[] { "SecurityGroupID", "RequestTypeID", "Permission" });
            DropColumn("dbo.AclDataMartRequestTypes", "PermissionID");
            DropColumn("dbo.AclDataMartRequestTypes", "Allowed");
            DropColumn("dbo.AclProjectDataMartRequestTypes", "PermissionID");
            DropColumn("dbo.AclProjectDataMartRequestTypes", "Allowed");
            DropColumn("dbo.AclRequestTypes", "PermissionID");
            DropColumn("dbo.AclRequestTypes", "Allowed");

            //Drop the permissions that are no longer used.
            Sql("DELETE FROM Permissions WHERE ID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3' OR ID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B'");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AclRequestTypes", "Allowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclRequestTypes", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclProjectDataMartRequestTypes", "Allowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclProjectDataMartRequestTypes", "PermissionID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclDataMartRequestTypes", "Allowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AclDataMartRequestTypes", "PermissionID", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.AclRequestTypes");
            DropPrimaryKey("dbo.AclProjectDataMartRequestTypes");
            DropPrimaryKey("dbo.AclDataMartRequestTypes");
            DropColumn("dbo.AclRequestTypes", "Permission");
            DropColumn("dbo.AclProjectDataMartRequestTypes", "Permission");
            DropColumn("dbo.AclDataMartRequestTypes", "Permission");
            AddPrimaryKey("dbo.AclRequestTypes", new[] { "SecurityGroupID", "PermissionID", "RequestTypeID" });
            AddPrimaryKey("dbo.AclProjectDataMartRequestTypes", new[] { "SecurityGroupID", "PermissionID", "ProjectID", "DataMartID", "RequestTypeID" });
            AddPrimaryKey("dbo.AclDataMartRequestTypes", new[] { "SecurityGroupID", "PermissionID", "DataMartID", "RequestTypeID" });
            CreateIndex("dbo.AclRequestTypes", "PermissionID");
            CreateIndex("dbo.AclProjectDataMartRequestTypes", "PermissionID");
            CreateIndex("dbo.AclDataMartRequestTypes", "PermissionID");
            AddForeignKey("dbo.AclRequestTypes", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclProjectDataMartRequestTypes", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclDataMartRequestTypes", "PermissionID", "dbo.Permissions", "ID", cascadeDelete: true);
        }
    }
}
