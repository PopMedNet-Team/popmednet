namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGroupSsoEndpointsType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SsoEndpoints", "Group", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SsoEndpoints", "Group", c => c.Guid());
        }
    }
}
