namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixIncorrectlySetFieldsForResponses : DbMigration
    {
        public override void Up()
        {
            //the responded by time and ID was getting set by the dmc for status's where they should not have.
            //these fields should only get set when the response is 'Complete'
            Sql(@"UPDATE r SET r.ResponseTime = NULL, r.RespondedByID = NULL
FROM RequestDataMartResponses r INNER JOIN RequestDataMarts dm ON r.RequestDataMartID = dm.ID
WHERE dm.QueryStatusTypeId NOT IN (3, 11, 12, 13) AND (r.ResponseTime IS NOT NULL OR r.RespondedByID IS NOT NULL)");
        }
        
        public override void Down()
        {
        }
    }
}
