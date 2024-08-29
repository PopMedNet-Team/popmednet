namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeletedToActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "Deleted");
        }
    }
}
