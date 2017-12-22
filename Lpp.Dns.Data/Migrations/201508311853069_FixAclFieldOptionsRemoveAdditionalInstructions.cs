namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAclFieldOptionsRemoveAdditionalInstructions : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM AclGlobalFieldOptions WHERE FieldIdentifier = 'Request-Additional-Information'");
            Sql(@"DELETE FROM AclProjectFieldOptions WHERE FieldIdentifier = 'Request-Additional-Information'");

        }
        
        public override void Down()
        {
        }
    }
}
