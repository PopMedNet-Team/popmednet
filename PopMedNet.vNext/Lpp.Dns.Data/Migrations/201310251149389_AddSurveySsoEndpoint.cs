namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSurveySsoEndpoint : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO SsoEndpoints (Name, Description, PostUrl, oAuthKey, oAuthHash) VALUES ('Survey Tool', 'Access the Survey Administration Tool.', 'http://localhost:4976/default.aspx', 'SurveyToolSso', 'fd2cdd12299a4ee098ba985958df0263')"); //Note this inserts the dev device, edit the table directly on production once this is created.

        }
        
        public override void Down()
        {
            Sql("DELETE FROM SsoEndpoints WHERE Name = 'PopMedNet'");
        }
    }
}
