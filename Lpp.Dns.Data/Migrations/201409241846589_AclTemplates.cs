namespace Lpp.Dns.Data.Migrations
{
    using Lpp.Utilities;
    using System;
    using System.Data.Entity.Migrations;
    using Lpp.Dns.DTO.Security;
    
    public partial class AclTemplates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclTemplates",
                c => new
                {
                    SecurityGroupID = c.Guid(nullable: false),
                    PermissionID = c.Guid(nullable: false),
                    TemplateID = c.Guid(nullable: false),
                    Allowed = c.Boolean(nullable: false),
                    Overridden = c.Boolean(nullable: false, defaultValue: false)
                })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.TemplateID })
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete:true)
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete:true)
                .ForeignKey("dbo.Templates", t => t.TemplateID, cascadeDelete:true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.TemplateID);
            
            Sql(MigrationHelpers.AddAclInsertScript("AclTemplates"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclTemplates"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclTemplates"));

            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.Portal.CreateTemplates.ID.ToString("D") + "', 'Create Templates', 'Can create new Query Composer request templates.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Portal.CreateTemplates.ID.ToString("D") + "', 0)");//Global
            
            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.Portal.ListTemplates.ID.ToString("D") + "', 'List Templates', 'Allows the list of Query Composer request templates to be viewed.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Portal.ListTemplates.ID.ToString("D") + "', 0)");//Global

            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.Templates.Delete.ID.ToString("D") + "', 'Delete Template', 'Allows the selected security group to delete the selected template.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.Delete.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.Delete.ID.ToString("D") + "', 23)");//Templates
            
            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.Templates.Edit.ID.ToString("D") + "', 'Edit Template', 'Allows the selected security group to edit the selected template.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.Edit.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.Edit.ID.ToString("D") + "', 23)");//Templates
            
            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.Templates.ManageSecurity.ID.ToString("D") + "', 'Manage Template Security', 'Allows the selected security group to manage security of the selected template.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.ManageSecurity.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.ManageSecurity.ID.ToString("D") + "', 23)");//Templates
            
            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.Templates.View.ID.ToString("D") + "', 'View Template', 'Allows the selected security group to view the selected template.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.View.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.Templates.View.ID.ToString("D") + "', 23)");//Templates
        }
        
        public override void Down()
        {
            Sql(MigrationHelpers.DropAclDeleteScript("AclTemplates"));
            Sql(MigrationHelpers.DropAclInsertScript("AclTemplates"));
            Sql(MigrationHelpers.DropAclUpdateScript("AclTemplates"));
            DropForeignKey("dbo.AclTemplates", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclTemplates", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclTemplates", "TemplateID", "dbo.Templates");
            DropIndex("dbo.AclTemplates", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclTemplates", new[] { "PermissionID" });
            DropIndex("dbo.AclTemplates", new[] { "TemplateID" });
            DropTable("dbo.AclTemplates");

            Sql(string.Format("DELETE FROM PermissionLocations WHERE PermissionID in ('{0:D}','{1:D}','{2:D}','{3:D}','{4:D}','{5:D}')", PermissionIdentifiers.Portal.CreateTemplates.ID, PermissionIdentifiers.Portal.ListTemplates.ID, PermissionIdentifiers.Templates.Delete.ID, PermissionIdentifiers.Templates.Edit.ID, PermissionIdentifiers.Templates.ManageSecurity.ID, PermissionIdentifiers.Templates.View.ID));
            Sql(string.Format("DELETE FROM Permissions WHERE ID in ('{0:D}','{1:D}','{2:D}','{3:D}','{4:D}','{5:D}')", PermissionIdentifiers.Portal.CreateTemplates.ID, PermissionIdentifiers.Portal.ListTemplates.ID, PermissionIdentifiers.Templates.Delete.ID, PermissionIdentifiers.Templates.Edit.ID, PermissionIdentifiers.Templates.ManageSecurity.ID, PermissionIdentifiers.Templates.View.ID));
        }
    }
}
