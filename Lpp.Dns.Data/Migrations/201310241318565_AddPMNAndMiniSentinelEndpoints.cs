namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPMNAndMiniSentinelEndpoints : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO SsoEndpoints (Name, Description, PostUrl, oAuthKey, oAuthHash) VALUES ('PopMedNet', 'Access the PopMedNet System.', 'http://localhost:55381/ssologin', 'PopMedNetSso', 'fd2cdd12298a4ee098ca985957dd0263')"); //Note this inserts the dev device, edit the table directly on production once this is created.

            Sql("INSERT INTO SsoEndpoints (Name, Description, PostUrl, oAuthKey, oAuthHash) VALUES ('Mini-Sentinel', 'Acces the Mini-Sentinel.', 'http://localhost:63737/account/ssologin.aspx', 'MiniSentinelSso', 'cbccd742221b4ef0965972c3b00cfe69')"); //Note this inserts the dev device, edit the table directly on production once this is created.

        }


        public override void Down()
        {
            Sql("DELETE FROM SsoEndpoints WHERE Name = 'PopMedNet'");
            Sql("DELETE FROM SsoEndpoints WHERE Name = 'Mini-Sentinel'");
        }
    }
}
