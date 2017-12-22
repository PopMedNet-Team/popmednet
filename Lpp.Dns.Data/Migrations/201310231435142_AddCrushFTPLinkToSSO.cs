namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCrushFTPLinkToSSO : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO SsoEndpoints (Name, Description, PostUrl) VALUES ('Crush FTP', 'Access the Crush FTP site. Note: You will have to login again.', 'http://crushftpstaging.lincolnpeak.com')"); //Note this inserts the qa server, edit the table directly on production once this is created.
        }
        
        public override void Down()
        {
            Sql("DELETE FROM SsoEndpoints WHERE Name = 'Crush FTP'");
        }
    }
}
