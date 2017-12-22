namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKeysToDataAvailabilityPeriodCategory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DataMartAvailabilityPeriods", "RequestTypeID", c => c.Guid(nullable: false));
            AlterColumn("DataMartAvailabilityPeriods", "DataMartID", c => c.Guid(nullable: false));
            AlterColumn("DataMartAvailabilityPeriods", "Period", c => c.String(nullable: false, maxLength: 10));
            AddPrimaryKey("DataMartAvailabilityPeriods", new string[] { "DataMartID", "RequestTypeID", "PeriodCategory", "Period" }, "PK_DataMartAvailabilityPeriods_DataMartID_RequestTypeID");
            CreateIndex("DataMartAvailabilityPeriods", new string[] { "DataMartID", "RequestTypeID", "PeriodCategory", "Period" }, name: "IDX_DataMartAvailabilityPeriods_DataMartID_RequestTypeID");
        }
        
        public override void Down()
        {
        }
    }
}
