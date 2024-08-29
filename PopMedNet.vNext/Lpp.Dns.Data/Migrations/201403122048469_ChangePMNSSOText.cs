namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePMNSSOText : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE SsoEndpoints SET Description = 'Access the Mini-Sentinel Distributed Research Network Query Tool' WHERE oAuthKey = 'PopMedNetSso'");
        }
        
        public override void Down()
        {
        }
    }
}
