namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectOrganizations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectOrganizations",
                c => new
                    {
                        ProjectID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.OrganizationID })
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.OrganizationID);

            Sql("INSERT INTO ProjectOrganizations (ProjectID, OrganizationID) SELECT Projects.ID, OrganizationGroups.OrganizationID FROM Projects JOIN OrganizationGroups ON Projects.GroupID = OrganizationGroups.GroupID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectOrganizations", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectOrganizations", "OrganizationID", "dbo.Organizations");
            DropIndex("dbo.ProjectOrganizations", new[] { "OrganizationID" });
            DropIndex("dbo.ProjectOrganizations", new[] { "ProjectID" });
            DropTable("dbo.ProjectOrganizations");
        }
    }
}
