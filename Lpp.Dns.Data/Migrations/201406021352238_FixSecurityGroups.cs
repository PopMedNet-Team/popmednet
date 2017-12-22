namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityGroups : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SecurityGroups", name: "DisplayName", newName: "Name");
            AddColumn("dbo.SecurityGroups", "OwnerID", c => c.Guid());
            AlterColumn("dbo.SecurityGroups", "Name", c => c.String(nullable: false, maxLength: 255));

            //Copy the data
            Sql("UPDATE SecurityGroups SET OwnerID = OrganizationID");

            DropForeignKey("dbo.SecurityGroups", "FK_SecurityGroups_Organizations_OrganizationID");
            DropIndex("dbo.SecurityGroups", new[] { "OrganizationID" });
            DropColumn("dbo.SecurityGroups", "OrganizationID");

            //Project Security groups moved.
            Sql(
                "INSERT INTO SecurityGroups (ID, Name, Kind, OwnerID) SELECT ID, DisplayName, Kind, ProjectId FROM ProjectSecurityGroups");

            
            DropTable("dbo.ProjectSecurityGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectSecurityGroups",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        DisplayName = c.String(nullable: false),
                        Kind = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.SecurityGroups", "OrganizationID", c => c.Guid(nullable: false));
            AlterColumn("dbo.SecurityGroups", "Name", c => c.String(nullable: false));
            DropColumn("dbo.SecurityGroups", "OwnerID");
            RenameColumn(table: "dbo.SecurityGroups", name: "Name", newName: "DisplayName");
            CreateIndex("dbo.ProjectSecurityGroups", "ProjectId");
            CreateIndex("dbo.SecurityGroups", "OrganizationID");
            AddForeignKey("dbo.ProjectSecurityGroups", "ProjectId", "dbo.Projects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SecurityGroups", "OrganizationID", "dbo.Organizations", "ID", cascadeDelete: true);
        }
    }
}
