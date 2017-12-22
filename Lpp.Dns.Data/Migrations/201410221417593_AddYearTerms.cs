namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddYearTerms : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('D62F0001-3FE1-4910-99A9-A3CB014C2BC7', 'Quarterly Year', 'The reported Quarterly Year of the encounter', null, null, 3)");
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('781A0001-1FF0-41AB-9E67-A3CB014C37F2', 'Year', 'The reported Year of the encounter', null, null, 3)");
            Sql(@"update Terms Set Name = 'Code Metric Type' Where ID = 'E39D0001-07A1-4DFD-9299-A3CB00F2474B'");
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl, Type) VALUES ('16ED0001-7E2D-4B27-B943-A3CB0162C262', 'Dispensing Metric Type', 'The reported Metiric Type of the encounter', null, null, 3)");
        }
        
        public override void Down()
        {
        }
    }
}
