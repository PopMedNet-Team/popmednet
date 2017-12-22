namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesToTemplates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "Notes");
        }
    }
}
