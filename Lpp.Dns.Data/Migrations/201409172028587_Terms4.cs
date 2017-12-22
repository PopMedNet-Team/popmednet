namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Terms4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TemplateTerms",
                c => new
                    {
                        TemplateID = c.Guid(nullable: false),
                        TermID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.TemplateID, t.TermID })
                .ForeignKey("dbo.Templates", t => t.TemplateID, cascadeDelete: true)
                .ForeignKey("dbo.Terms", t => t.TermID, cascadeDelete: true)
                .Index(t => t.TemplateID)
                .Index(t => t.TermID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TemplateTerms", "TermID", "dbo.Terms");
            DropForeignKey("dbo.TemplateTerms", "TemplateID", "dbo.Templates");
            DropIndex("dbo.TemplateTerms", new[] { "TermID" });
            DropIndex("dbo.TemplateTerms", new[] { "TemplateID" });
            DropTable("dbo.TemplateTerms");
        }
    }
}
