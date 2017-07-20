namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveInvalidTermsFromPCORnetModel : DbMigration
    {
        public override void Up()
        {
            //remove any invalid terms associated with the PCORnet model.
            Sql(@"DELETE FROM DataModelSupportedTerms WHERE DataModelID = '85EE982E-F017-4BC4-9ACD-EE6EE55D2446'
AND TermID NOT IN (
'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C', -- age range
'2DE50001-7882-4EDE-AC4F-A3CB00D9051A', -- setting
'98A78326-35D2-461A-B858-5B69E0FED28A', -- observation period
'8BC627CA-5729-4E7A-9702-0000A45A0018', -- height
'3B0ED310-DDE9-4836-9857-0000A45A0018', -- weight
'71B4545C-345B-48B2-AF5E-F84DC18E4E1A', -- sex
'834F0001-FA03-4ECD-BE28-A3CD00EC02E2', -- race
'D26FE166-49A2-47F8-87E2-4F42A945D4D5', -- hispanic
'F9920001-AEB1-425C-A929-A4BB01515850', -- vitals measure date
'86110001-4BAB-4183-B0EA-A4BC0125A6A7' -- combined diagnosis codes
)");
        }
        
        public override void Down()
        {
        }
    }
}
