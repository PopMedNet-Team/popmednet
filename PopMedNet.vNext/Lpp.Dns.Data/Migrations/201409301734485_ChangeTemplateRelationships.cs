namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTemplateRelationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectTemplates", "TemplateID", "dbo.Templates");
            DropForeignKey("dbo.RequestTypeTemplates", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.ProjectTemplates", "ProjectID", "dbo.Projects");
            DropIndex("dbo.ProjectTemplates", new[] { "ProjectID" });
            DropIndex("dbo.ProjectTemplates", new[] { "TemplateID" });
            DropIndex("dbo.RequestTypeTemplates", new[] { "RequestTypeID" });
            DropIndex("dbo.RequestTypeTemplates", new[] { "TemplateID" });
            AddColumn("dbo.RequestTypes", "TemplateID", c => c.Guid(nullable: false));
            AddColumn("dbo.RequestTypes", "WorkflowID", c => c.Guid());
            CreateIndex("dbo.RequestTypes", "TemplateID");
            CreateIndex("dbo.RequestTypes", "WorkflowID");
            AddForeignKey("dbo.RequestTypes", "WorkflowID", "dbo.Workflows", "ID");
            DropTable("dbo.ProjectTemplates");
            DropTable("dbo.RequestTypeTemplates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestTypeTemplates",
                c => new
                    {
                        RequestTypeID = c.Guid(nullable: false),
                        TemplateID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestTypeID, t.TemplateID });
            
            CreateTable(
                "dbo.ProjectTemplates",
                c => new
                    {
                        ProjectID = c.Guid(nullable: false),
                        TemplateID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.TemplateID });
            
            DropForeignKey("dbo.RequestTypes", "WorkflowID", "dbo.Workflows");
            DropIndex("dbo.RequestTypes", new[] { "WorkflowID" });
            DropIndex("dbo.RequestTypes", new[] { "TemplateID" });
            DropColumn("dbo.RequestTypes", "WorkflowID");
            DropColumn("dbo.RequestTypes", "TemplateID");
            CreateIndex("dbo.RequestTypeTemplates", "TemplateID");
            CreateIndex("dbo.RequestTypeTemplates", "RequestTypeID");
            CreateIndex("dbo.ProjectTemplates", "TemplateID");
            CreateIndex("dbo.ProjectTemplates", "ProjectID");
            AddForeignKey("dbo.ProjectTemplates", "ProjectID", "dbo.Projects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.RequestTypeTemplates", "RequestTypeID", "dbo.RequestTypes", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectTemplates", "TemplateID", "dbo.Templates", "ID", cascadeDelete: true);
        }
    }
}
