namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMSAdminFromSSO : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM SsoEndpoints WHERE oAuthKey = 'MiniSentinelSso'");
        }
        
        public override void Down()
        {
            Sql("INSERT (Name, Description, PostUrl, oAuthKey, oAuthHash) VALUES ('Mini-Sentinel', 'Access the Mini-Sentinel.', 'http://localhost:63737/account/ssologin.aspx', 'MiniSentinelSso', 'cbccd742221b4ef0965972c3b00cfe69')");
        }
    }
}
