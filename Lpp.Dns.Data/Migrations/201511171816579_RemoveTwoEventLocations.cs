namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTwoEventLocations : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM EventLocations WHERE EventID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869' AND Location = 11");

            Sql(@"DELETE FROM EventLocations WHERE EventID = 'D2DC0001-43E9-477E-B60D-A3BE01550FA4' AND Location = 11");

        }
        
        public override void Down()
        {
            Sql(@"IF NOT EXSISTS(SELECT NULL FROM Events WHERE EventID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869' AND Location = 11) INSERT INTO Events (EventID, Location) VALUES ('6549439E-E3E4-4F4C-92CF-88FB81FF8869', 11) ");

            Sql(@"IF NOT EXISTS(SELECT NULL FROM Events WHERE EventID = 'D2DC0001-43E9-477E-B60D-A3BE01550FA4' AND Location = 11) INSERT INTO Events (EventID, Location) VALUES ('D2DC0001-43E9-477E-B60D-A3BE01550FA4', 11)");

        }
    }
}
