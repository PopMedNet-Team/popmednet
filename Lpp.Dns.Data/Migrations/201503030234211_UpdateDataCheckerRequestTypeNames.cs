namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataCheckerRequestTypeNames : DbMigration
    {
        public override void Up()
        {
            Sql("update RequestTypes SET Name = 'Demographic: Ethnicity' where ID = '4EE29758-DCFF-4D2A-A7A8-626C81FBA367'");
            Sql("update RequestTypes SET Name = 'Demographic: Race' where ID = '5CA5940A-CF8B-48CC-836C-66B2EB97AFB3'");
            Sql("update RequestTypes SET Name = 'Diagnosis: Diagnosis Codes' where ID = 'D5DA7ACA-7179-4EA5-BD9C-534D47B6C6C4'");
            Sql("update RequestTypes SET Name = 'Diagnosis: PDX' where ID = '0F1EA012-B588-4775-9E16-CB6DBE12F8BE'");
            Sql("update RequestTypes SET Name = 'Dispensing: NDC' where ID = '0F1EA011-B588-4775-9E16-CB6DBE12F8BE'");
            Sql("update RequestTypes SET Name = 'Procedure: Procedure Codes' where ID = '39F8E764-BDD8-4D75-AE50-809C59C28E43'");
           
        }
        
        public override void Down()
        {
        }
    }
}
