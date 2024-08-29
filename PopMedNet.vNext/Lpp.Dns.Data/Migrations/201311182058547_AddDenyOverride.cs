namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDenyOverride : DbMigration
    {
        public override void Up()
        {
           
            AddColumn("dbo.ACL", "Denied", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ACL", "Denied");
        }
    }
}
