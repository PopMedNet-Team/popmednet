namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetRequirePasswordOnWbdSSO : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE SsoEndpoints SET RequirePassword = 1 WHERE oAuthKey = 'WbdSso'");
        }
        
        public override void Down()
        {
        }
    }
}
