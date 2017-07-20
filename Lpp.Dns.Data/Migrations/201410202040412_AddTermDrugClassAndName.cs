namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTermDrugClassAndName : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('75290001-0E78-490C-9635-A3CA01550704', 'Drug Class', 'The reported Drug Class of the encounter', null, null)");
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('0E1F0001-CA0C-42D2-A9CC-A3CA01550E84', 'Drug Name', 'The reported Drug Name of the encounter', null, null)");
        }
        
        public override void Down()
        {
        }
    }
}
