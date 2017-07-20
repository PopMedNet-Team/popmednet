namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupToSsoEndpoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SsoEndpoints", "Group", c => c.Guid());

        }
        
        public override void Down()
        {
            DropColumn("dbo.SsoEndpoints", "Group");
        }
    }
}
