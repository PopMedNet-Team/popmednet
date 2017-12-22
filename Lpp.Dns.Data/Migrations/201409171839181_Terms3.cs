namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Terms3 : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('98A78326-35D2-461A-B858-5B69E0FED28A', 'Observation Period', 'The observation period of the encounter.', null, null)");

            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('D9DD6E82-BBCA-466A-8022-B54FF3D99A3C', 'Age Range', 'A range of ages for the patient subject of the encounter.', null, null)");

            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('EC593176-D0BF-4E5A-BCFF-4BBD13E88DBF', 'Conditions', 'A general condition reported during the encounter', null, null)");

            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('702CE918-9A59-4082-A8C7-A9234536FE79', 'Ethnicity', 'The reported ethnicity of the patient subject', null, null)");

            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('71B4545C-345B-48B2-AF5E-F84DC18E4E1A', 'Gender', 'The reported gender of the patient subject', null, null)");

            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('5E5020DC-C0E4-487F-ADF2-45431C2B7695', 'ICD-9 Codes', 'The reported ICD-9 codes of the encounter', null, null)");

            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('F01BE0A4-7D8E-4288-AE33-C65166AF8335', 'Visits', 'The number of unique visits for a patient typically combined with other terms', null, null)");

            Sql(@"INSERT INTO Terms (ID, Name, Description, OID, ReferenceUrl) VALUES ('8B5FAA77-4A4B-4AC7-B817-69F1297E24C5', 'Zip Code', 'A list of geographical locations for patient encounters', null, null)");
        }
        
        public override void Down()
        {
        }
    }
}
