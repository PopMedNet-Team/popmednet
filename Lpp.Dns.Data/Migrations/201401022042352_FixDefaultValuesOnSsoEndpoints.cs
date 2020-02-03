namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDefaultValuesOnSsoEndpoints : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE SsoEndpoints SET RequirePassword = 0 WHERE oAuthKey <> 'WbdSiteSso'");
        }
        
        public override void Down()
        {
        }
    }
}
