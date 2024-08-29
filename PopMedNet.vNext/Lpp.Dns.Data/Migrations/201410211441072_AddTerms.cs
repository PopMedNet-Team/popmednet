namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTerms : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('2DE50001-7882-4EDE-AC4F-A3CB00D9051A', 'Setting', 'The reported Setting of the encounter', null, null, 1)");
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('DC880001-23B2-4F36-94E2-A3CB00DA8248', 'Coverage', 'The reported Coverage of the encounter', null, null, 1)");
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('E39D0001-07A1-4DFD-9299-A3CB00F2474B', 'Metric', 'The reported Metric of the encounter', null, null, 1)");
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('17540001-8185-41BB-BE64-A3CB00F27EC2', 'Criteria', 'The reported Criteria of the encounter', null, null, 1)");
        }
        
        public override void Down()
        {
        }
    }
}
