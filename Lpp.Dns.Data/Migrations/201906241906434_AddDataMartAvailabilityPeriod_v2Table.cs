namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataMartAvailabilityPeriod_v2Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataMartAvailabilityPeriods_v2",
                c => new
                    {
                        DataMartID = c.Guid(nullable: false),
                        DataTable = c.String(nullable: false, maxLength: 80),
                        PeriodCategory = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        Period = c.String(nullable: false, maxLength: 10),
                        Year = c.Int(nullable: false),
                        Quarter = c.Int(),
                    })
                .PrimaryKey(t => new { t.DataMartID, t.DataTable, t.PeriodCategory, t.Period })
                .ForeignKey("dbo.DataMarts", t => t.DataMartID)
                .Index(t => t.DataMartID);



            Sql(@"CREATE FUNCTION [dbo].[GetDataMartsAvailability]
                    (
                        @UserID uniqueidentifier
                    )
                    RETURNS 
                    @items TABLE 
                    (
	                    [DataMartID] [uniqueidentifier] NOT NULL,
	                    [DataMart] [nvarchar](255) NOT NULL,
	                    [DataTable] [nvarchar](80) NOT NULL,
	                    [PeriodCategory] [char](1) NOT NULL,
	                    [Period] [nvarchar](10) NOT NULL,
	                    [Year] [int] NOT NULL,
	                    [Quarter] [int] NULL
                    )
                    AS
                    BEGIN

                    INSERT INTO @items (DataMartID,DataMart, DataTable, PeriodCategory, Period, Year, Quarter) 
	                    Select DataMartID, dm.Name, DataTable, PeriodCategory, Period, Year, Quarter from DataMartAvailabilityPeriods_v2 dap
	                    join DataMarts as dm on dap.DataMartID = dm.ID
	                    WHERE	EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '6CCB0EC2-006D-4345-895E-5DD2C6C8C791'))
			                    OR
			                    EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '6CCB0EC2-006D-4345-895E-5DD2C6C8C791', dap.DataMartID))
			                    OR
			                    EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '6CCB0EC2-006D-4345-895E-5DD2C6C8C791', dm.OrganizationID)) 

                    INSERT INTO @items (DataMartID,DataMart, DataTable, PeriodCategory, Period, Year, Quarter) 
	                    Select DataMartID, 
		                       dm.Name,
		                       rt.Name as DataTable, 
		                       PeriodCategory, 
		                       Period, 
		                       CASE When PeriodCategory = 'Y' THEN dap.Period ELSE SUBSTRING(dap.Period, 0, CHARINDEX('Q', dap.Period)) END AS Year, 
		                       CASE WHEN PeriodCategory = 'Y' THEN NULL ELSE SUBSTRING(dap.Period, CHARINDEX('Q', dap.Period) + 1, 1) END as Quarter 
		                       from DataMartAvailabilityPeriods dap
	                    join RequestTypes as rt on dap.RequestTypeID = rt.ID 
	                    join DataMarts as dm on dap.DataMartID = dm.ID
	                    WHERE	EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '6CCB0EC2-006D-4345-895E-5DD2C6C8C791'))
			                    OR
			                    EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '6CCB0EC2-006D-4345-895E-5DD2C6C8C791', dap.DataMartID))
			                    OR
			                    EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '6CCB0EC2-006D-4345-895E-5DD2C6C8C791', dm.OrganizationID)) 
	                    RETURN
                    END
                    GO");
            
        }
        
        public override void Down()
        {
            Sql("DROP FUNCTION [dbo].[GetDataMartsAvailability]");
            DropForeignKey("dbo.DataMartAvailabilityPeriods_v2", "DataMartID", "dbo.DataMarts");
            DropIndex("dbo.DataMartAvailabilityPeriods_v2", new[] { "DataMartID" });
            DropTable("dbo.DataMartAvailabilityPeriods_v2");
        }
    }
}
