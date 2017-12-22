namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTermType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Terms", "Type", c => c.Int(nullable: false, defaultValue: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Terms", "Type");
        }
    }
}
