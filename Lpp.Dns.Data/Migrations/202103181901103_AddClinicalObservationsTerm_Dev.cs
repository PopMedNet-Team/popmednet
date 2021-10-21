namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClinicalObservationsTerm_Dev : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO TERMS ([ID],[Name],[Description],[OID],[ReferenceUrl],[Type])
                VALUES ('7A51AB7A-AEC5-4A4B-A073-FBFF754EA478', 'Clinical Observations', 'A term to allow querying against the OBS_CLIN table in the PCORNet Common Data Model for clinical observations.', NULL, NULL, 2)");

            Sql("INSERT INTO DataModelSupportedTerms (DataModelID, TermID) VALUES ('85EE982E-F017-4BC4-9ACD-EE6EE55D2446','7A51AB7A-AEC5-4A4B-A073-FBFF754EA478')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM DataModelSupportedTerms WHERE TermID = '7A51AB7A-AEC5-4A4B-A073-FBFF754EA478'");
            Sql("DELETE FROM TERMS WHERE ID = '7A51AB7A-AEC5-4A4B-A073-FBFF754EA478'");
        }
    }
}
