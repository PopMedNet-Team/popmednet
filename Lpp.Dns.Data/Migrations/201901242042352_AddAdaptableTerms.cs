namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdaptableTerms : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO TERMS ([ID],[Name],[Description],[OID],[ReferenceUrl],[Type]) 
                VALUES ('56A38F6D-993A-4953-BCB9-11BDD809C0B4', 'Trail', 'The ID of the trial the patient participated in.', NULL, NULL, 3)");
            Sql(@"INSERT INTO TERMS ([ID],[Name],[Description],[OID],[ReferenceUrl],[Type]) 
                VALUES ('AE87C785-BB74-4708-B0CD-FA91D584615C', 'Patient Reported Outcome (PRO)', 'A term to allow querying against the PRO_CM table for patient-reported outcome measures.', NULL, NULL, 3)");
            Sql(@"INSERT INTO TERMS ([ID],[Name],[Description],[OID],[ReferenceUrl],[Type])
                VALUES ('A11DCC97-4C8D-4B80-AE61-58ECB2F66C3D', 'Patient Reported Outcome Encounters', 'A term to allow the inclusion of Encounters based on PRO measures.', NULL, NULL, 2)");

            Sql("INSERT INTO DataModelSupportedTerms (DataModelID, TermID) VALUES ('85EE982E-F017-4BC4-9ACD-EE6EE55D2446','56A38F6D-993A-4953-BCB9-11BDD809C0B4')");
            Sql("INSERT INTO DataModelSupportedTerms (DataModelID, TermID) VALUES ('85EE982E-F017-4BC4-9ACD-EE6EE55D2446','AE87C785-BB74-4708-B0CD-FA91D584615C')");
            Sql("INSERT INTO DataModelSupportedTerms (DataModelID, TermID) VALUES ('85EE982E-F017-4BC4-9ACD-EE6EE55D2446','A11DCC97-4C8D-4B80-AE61-58ECB2F66C3D')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM DataModelSupportedTerms WHERE TermID = '56A38F6D-993A-4953-BCB9-11BDD809C0B4'");
            Sql("DELETE FROM DataModelSupportedTerms WHERE TermID = 'AE87C785-BB74-4708-B0CD-FA91D584615C'");
            Sql("DELETE FROM DataModelSupportedTerms WHERE TermID = 'A11DCC97-4C8D-4B80-AE61-58ECB2F66C3D'");
            Sql("DELETE FROM TERMS WHERE ID = '56A38F6D-993A-4953-BCB9-11BDD809C0B4'");
            Sql("DELETE FROM TERMS WHERE ID = 'AE87C785-BB74-4708-B0CD-FA91D584615C'");
            Sql("DELETE FROM TERMS WHERE ID = 'A11DCC97-4C8D-4B80-AE61-58ECB2F66C3D'");
        }
    }
}
