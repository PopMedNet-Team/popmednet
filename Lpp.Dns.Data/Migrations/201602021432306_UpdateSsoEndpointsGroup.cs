namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSsoEndpointsGroup : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE dbo.SsoEndpoints SET [Group] = '3C594946-6C14-4AA6-86BB-BDB91989365F'");

            AlterColumn("dbo.SsoEndpoints", "Group", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SsoEndpoints", "Group", c => c.Guid());

            Sql(@"UPDATE dbo.SsoEndpoints SET [Group] = NULL");
        }
    }
}
