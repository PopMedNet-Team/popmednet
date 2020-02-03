namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQueryTypeToTemplates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "QueryType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "QueryType");
        }
    }
}
