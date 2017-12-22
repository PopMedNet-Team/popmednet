namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDataMartAvailabilityPeriods : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataMartAvailabilityPeriods", "RequestTypeID", c => c.Guid(true, false));

            // Use QueryID (unused) to distinguish re-inserted data to eliminate duplicates in original.
            Sql(@"UPDATE p
                    SET RequestTypeID = rt.ID, QueryId = 1
                    FROM DataMartAvailabilityPeriods p
                    JOIN QueryTypes qt ON p.QueryTypeId = qt.QueryTypeId
                    JOIN RequestTypes rt ON qt.QueryType = rt.Name");
            Sql(@"INSERT INTO DataMartAvailabilityPeriods (QueryId, DataMartID, RequestTypeID, PeriodCategory, Period, isActive) 
                    SELECT DISTINCT 0, DataMartID, RequestTypeID, PeriodCategory, Period, isActive FROM DataMartAvailabilityPeriods");
            Sql(@"DELETE FROM DataMartAvailabilityPeriods WHERE QueryId = 1");
            DropColumn("DataMartAvailabilityPeriods", "QueryId");
            AddForeignKey("DataMartAvailabilityPeriods", "RequestTypeID", "RequestTypes", "ID", true, "FK_DataMartAvailabilityPeriods_RequestTypes_RequestTypeID");
            DropForeignKey("DataMartAvailabilityPeriods", "FK_DataMartAvailabilityPeriods_QueryTypes_QueryTypeId");
            DropColumn("DataMartAvailabilityPeriods", "QueryTypeId");
            DropTable("QueryTypes");
        }
        
        public override void Down()
        {
        }
    }
}
