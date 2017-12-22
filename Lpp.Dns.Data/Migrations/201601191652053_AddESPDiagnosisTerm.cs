namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddESPDiagnosisTerm : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('A21E9775-39A4-4ECC-848B-1DC881E13689', 'ESP Diagnosis', 'ESP Diagnosis Term', null, null, 1)");
        }
        
        public override void Down()
        {
        }
    }
}
