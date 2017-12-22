namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTermICD9Codes : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('096A0001-73B4-405D-B45F-A3CA014C6E7D', 'HCPCS Procedure Codes', 'The reported HCPCS Procedure codes of the encounter', null, null)");
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('E1CC0001-1D9A-442A-94C4-A3CA014C7B94', 'ICD 9 Procedure Codes', 'The reported ICD-9 Procedure codes of the encounter', null, null)");
            Sql(@"Update Terms Set Name = 'ICD 9 Diagnosis Codes' Where ID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695' ");
            Sql(@"Update Terms Set Description = 'The reported ICD-9 Diagnosis codes of the encounter' Where ID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695' ");
        }
        
        public override void Down()
        {
        }
    }
}
