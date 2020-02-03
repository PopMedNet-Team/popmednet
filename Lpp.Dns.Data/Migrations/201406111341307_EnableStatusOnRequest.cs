namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableStatusOnRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Status");
        }
    }
}
