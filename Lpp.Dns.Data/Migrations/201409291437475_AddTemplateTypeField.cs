namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTemplateTypeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "Type", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "Type");
        }
    }
}
