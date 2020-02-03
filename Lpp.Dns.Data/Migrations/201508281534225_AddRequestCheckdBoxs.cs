namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestCheckdBoxs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "isChecked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "isChecked");
        }
    }
}
