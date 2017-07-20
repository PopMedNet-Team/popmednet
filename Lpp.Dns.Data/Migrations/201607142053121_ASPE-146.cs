namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ASPE146 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestDataMarts", "RoutingType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestDataMarts", "RoutingType");
        }
    }
}
