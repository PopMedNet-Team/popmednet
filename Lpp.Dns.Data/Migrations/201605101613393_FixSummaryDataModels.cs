namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSummaryDataModels : DbMigration
    {
        public override void Up()
        {

            Sql(@"Update DataModels set QueryComposer = 0 where ID = '805DF412-4ACC-4BA0-B0AD-7C4C24FD9F67'");

            Sql(@"Update DataModels set QueryComposer = 0 where ID = '4F364773-20A0-4036-800B-841421CB3209'");

            Sql(@"Update DataModels set QueryComposer = 0 where ID = '4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA'");

        }
        
        public override void Down()
        {

            Sql(@"Update DataModels set QueryComposer = 1 where ID = '805DF412-4ACC-4BA0-B0AD-7C4C24FD9F67'");

            Sql(@"Update DataModels set QueryComposer = 1 where ID = '4F364773-20A0-4036-800B-841421CB3209'");

            Sql(@"Update DataModels set QueryComposer = 1 where ID = '4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA'");

        }
    }
}
