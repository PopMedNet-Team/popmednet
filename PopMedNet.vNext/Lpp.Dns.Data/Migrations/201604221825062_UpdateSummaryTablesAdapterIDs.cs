namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSummaryTablesAdapterIDs : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM DataModelSupportedTerms WHERE DataModelID = '4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA' or DataModelID = '4F364773-20A0-4036-800B-841421CB3209' or DataModelID = '805DF412-4ACC-4BA0-B0AD-7C4C24FD9F67'");

            Sql(@"UPDATE DataMarts SET AdapterID = 'CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB' WHERE AdapterID = '4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA' or AdapterID = '4F364773-20A0-4036-800B-841421CB3209' or AdapterID = '805DF412-4ACC-4BA0-B0AD-7C4C24FD9F67'");

        }
        
        public override void Down()
        {
        }
    }
}
