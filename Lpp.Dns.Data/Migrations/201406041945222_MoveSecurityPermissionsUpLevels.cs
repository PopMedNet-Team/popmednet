namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveSecurityPermissionsUpLevels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclDataMartRequestTypes",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        DataMartID = c.Guid(nullable: false),
                        RequestTypeID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.DataMartID, t.RequestTypeID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)                
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.DataMartID)
                .Index(t => t.RequestTypeID);

            AddForeignKey("AclDataMartRequestTypes", "RequestTypeID", "RequestTypes", "ID", true);

            Sql(@"EXEC sp_msforeachtable ""ALTER TABLE ? DISABLE TRIGGER ALL""");

            Sql(
                "INSERT INTO AclDataMarts (DataMartID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT DataMartID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclOrganizationDataMarts WHERE OrganizationID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclOrganizationDataMarts WHERE OrganizationID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclDataMarts WHERE DataMartID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclDataMarts WHERE DataMartID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclEvents (EventID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT EventID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclDataMartEvents WHERE DataMartID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclDataMartEvents WHERE DataMartID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclEvents (EventID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT EventID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclGroupEvents WHERE GroupID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclGroupEvents WHERE GroupID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclGroups WHERE GroupID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclGroups WHERE GroupID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclEvents (EventID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT EventID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclOrganizationEvents WHERE OrganizationID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclOrganizationEvents WHERE OrganizationID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclDataMartRequestTypes (DataMartID, RequestTypeID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT DataMartID, RequestTypeID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclProjectDataMartRequestTypes WHERE ProjectID = '00000000-0000-0000-0000-000000000000' AND EXISTS(SELECT NULL FROM RequestTypes WHERE RequestTypes.ID = AclProjectDataMartRequesttypes.RequestTypeID)");
            Sql("DELETE FROM AclProjectDataMartRequestTypes WHERE ProjectID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclDataMarts (DataMartID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT DataMartID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclProjectDataMarts WHERE ProjectID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclProjectDataMarts WHERE ProjectID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclEvents (EventID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT EventID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclProjectEvents WHERE ProjectID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclProjectEvents WHERE ProjectID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclProjects WHERE ProjectID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclProjects WHERE ProjectID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclRegistries WHERE RegistryID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclRegistries WHERE RegistryID = '00000000-0000-0000-0000-000000000000'");

            Sql(
    "INSERT INTO AclEvents (EventID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT EventID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclRegistryEvents WHERE RegistryID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclRegistryEvents WHERE RegistryID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclRequests WHERE RequestID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclRequests WHERE RequestID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclRequestSharedFolders WHERE RequestSharedFolderID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclRequestSharedFolders WHERE RequestSharedFolderID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM AclRequestTypes WHERE RequestTypeID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclRequestTypes WHERE RequestTypeID = '00000000-0000-0000-0000-000000000000'");

            Sql(
    "INSERT INTO AclEvents (EventID, PermissionID, SecurityGroupID, Allowed, Overridden) SELECT EventID, PermissionID, SecurityGroupID, Allowed, Overridden FROM AclUserEvents WHERE UserID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM AclUserEvents WHERE UserID = '00000000-0000-0000-0000-000000000000'");

            Sql(
                "INSERT INTO AclGlobal (PermissionID, SecurityGroupID, Allowed, Overridden) SELECT PermissionID, SecurityGroupID, Allowed, Overridden FROM aclUsers WHERE UserID = '00000000-0000-0000-0000-000000000000'");
            Sql("DELETE FROM aclUsers WHERE UserID = '00000000-0000-0000-0000-000000000000'");


            Sql(@"EXEC sp_msforeachtable ""ALTER TABLE ? ENABLE TRIGGER ALL""");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclDataMartRequestTypes", "DataMartID", "dbo.DataMarts");
            DropForeignKey("dbo.AclDataMartRequestTypes", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclDataMartRequestTypes", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.AclDataMartRequestTypes", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.AclDataMartRequestTypes", new[] { "RequestTypeID" });
            DropIndex("dbo.AclDataMartRequestTypes", new[] { "DataMartID" });
            DropIndex("dbo.AclDataMartRequestTypes", new[] { "PermissionID" });
            DropIndex("dbo.AclDataMartRequestTypes", new[] { "SecurityGroupID" });
            DropTable("dbo.AclDataMartRequestTypes");
        }
    }
}
