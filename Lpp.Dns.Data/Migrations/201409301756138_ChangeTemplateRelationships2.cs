namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTemplateRelationships2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectRequestTypes",
                c => new
                    {
                        ProjectID = c.Guid(nullable: false),
                        RequestTypeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.RequestTypeID })
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.RequestTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectRequestTypes", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectRequestTypes", "RequestTypeID", "dbo.RequestTypes");
            DropIndex("dbo.ProjectRequestTypes", new[] { "RequestTypeID" });
            DropIndex("dbo.ProjectRequestTypes", new[] { "ProjectID" });
            DropTable("dbo.ProjectRequestTypes");
        }
    }
}
