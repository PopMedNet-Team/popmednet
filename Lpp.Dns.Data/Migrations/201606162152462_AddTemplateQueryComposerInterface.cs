namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTemplateQueryComposerInterface : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "ComposerInterface", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "ComposerInterface");
        }
    }
}
