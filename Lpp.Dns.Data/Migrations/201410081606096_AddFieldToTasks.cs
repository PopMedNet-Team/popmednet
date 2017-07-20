namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldToTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "DirectToRequest", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "DirectToRequest");
        }
    }
}
