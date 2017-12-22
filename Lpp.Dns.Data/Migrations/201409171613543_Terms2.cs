namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Terms2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestTypeTemplates",
                c => new
                    {
                        RequestTypeID = c.Guid(nullable: false),
                        TemplateID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestTypeID, t.TemplateID })
                .ForeignKey("dbo.Templates", t => t.TemplateID, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .Index(t => t.RequestTypeID)
                .Index(t => t.TemplateID);

            DropTable("dbo.Models");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestTypeTemplates", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.RequestTypeTemplates", "TemplateID", "dbo.Templates");
            DropIndex("dbo.RequestTypeTemplates", new[] { "TemplateID" });
            DropIndex("dbo.RequestTypeTemplates", new[] { "RequestTypeID" });
            DropTable("dbo.RequestTypeTemplates");
        }
    }
}
