namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTooShortSSOSalt : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE SsoEndpoints SET oAuthKey = 'WbdSiteSso' WHERE oAuthKey = 'WbdSso'"); //Was too short
        }
        
        public override void Down()
        {
            Sql(@"UPDATE SsoEndpoints SET oAuthKey = 'WbdSso' WHERE oAuthKey = 'WbdSiteSso'");
        }
    }
}
