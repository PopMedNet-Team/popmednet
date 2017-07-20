namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixFKOnTemplatesToRequestTypes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RequestTypes", new[] { "TemplateID" });
            AlterColumn("dbo.RequestTypes", "TemplateID", c => c.Guid());
            CreateIndex("dbo.RequestTypes", "TemplateID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequestTypes", new[] { "TemplateID" });
            AlterColumn("dbo.RequestTypes", "TemplateID", c => c.Guid(nullable: false));
            CreateIndex("dbo.RequestTypes", "TemplateID");
        }
    }
}
