namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRaceTerm : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Terms (ID, [Name], [Description], [Type]) VALUES ('834F0001-FA03-4ECD-BE28-A3CD00EC02E2', 'Race', 'The reported race of the patient subject.', 3)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Terms WHERE ID = '834F0001-FA03-4ECD-BE28-A3CD00EC02E2'");
        }
    }
}
