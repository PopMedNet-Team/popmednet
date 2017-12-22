namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDNSSSOToRequirePassword : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE SsoEndpoints SET RequirePassword = 1 WHERE oAuthKey = 'PopMedNetSso'");
        }
        
        public override void Down()
        {
            Sql(@"UPDATE SsoEndpoints SET RequirePassword = 0 WHERE oAuthKey = 'PopMedNetSso'");
        }
    }
}
