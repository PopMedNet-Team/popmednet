namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSectionToTemplateTerm : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TemplateTerms");
            AddColumn("dbo.TemplateTerms", "Section", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TemplateTerms", new[] { "TemplateID", "TermID", "Section" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TemplateTerms");
            DropColumn("dbo.TemplateTerms", "Section");
            AddPrimaryKey("dbo.TemplateTerms", new[] { "TemplateID", "TermID" });
        }
    }
}
