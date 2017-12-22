namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSSOEndpointsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SsoEndpoints", "DisplayIndex", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.SsoEndpoints", "Enabled", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SsoEndpoints", "Enabled");
            DropColumn("dbo.SsoEndpoints", "DisplayIndex");
        }
    }
}
