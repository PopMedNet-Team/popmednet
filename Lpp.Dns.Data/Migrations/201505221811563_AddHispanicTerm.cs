namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHispanicTerm : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('D26FE166-49A2-47F8-87E2-4F42A945D4D5', 'Hispanic', 'Hispanic Term', null, null, 1)");
        }
        
        public override void Down()
        {
        }
    }
}
