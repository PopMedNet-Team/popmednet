namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequirePasswordFlagSSO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SsoEndpoints", "RequirePassword", c => c.Boolean(nullable: false, defaultValue: true));

            Sql("INSERT INTO SsoEndpoints (Name, Description, PostUrl, oAuthKey, oAuthHash) VALUES ('Web-Based DataMart', 'Access the Web-Based DataMart.', 'http://localhost:64900/ssologin', 'WbdSso', 'fd2cdd12298a4ee098ca985957dd0263')"); //Note this inserts the dev device, edit the table directly on production once this is created.
        }
        
        public override void Down()
        {
            DropColumn("dbo.SsoEndpoints", "RequirePassword");
        }
    }
}
