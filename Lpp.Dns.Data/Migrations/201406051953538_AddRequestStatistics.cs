namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestStatistics : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE VIEW [dbo].[vwRequestStatistics]
AS
SELECT        RequestID, ISNULL(SUM(CASE WHEN QueryStatusTypeId = 2 THEN 1 ELSE 0 END), 0) AS Submitted, ISNULL(SUM(CASE WHEN QueryStatusTypeId = 3 OR
                         QueryStatusTypeId = 14 THEN 1 ELSE 0 END), 0) AS Completed, ISNULL(SUM(CASE WHEN QueryStatusTypeId = 4 THEN 1 ELSE 0 END), 0) AS AwaitingRequestApproval, 
                         ISNULL(SUM(CASE WHEN QueryStatusTypeId = 10 THEN 1 ELSE 0 END), 0) AS AwaitingResponseApproval, ISNULL(SUM(CASE WHEN QueryStatusTypeId = 5 THEN 1 ELSE 0 END), 0) AS RejectedRequest, 
                         ISNULL(SUM(CASE WHEN QueryStatusTypeId = 12 THEN 1 ELSE 0 END), 0) AS RejectedBeforeUploadResults, ISNULL(SUM(CASE WHEN QueryStatusTypeId = 13 THEN 1 ELSE 0 END), 0) 
                         AS RejectedAfterUploadResults, COUNT(*) AS Total
FROM            dbo.RequestDataMarts
GROUP BY RequestID");
            Sql("DROP VIEW dbo.vwRequestCounts");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ID", "dbo.vwRequestStatistics");
            DropIndex("dbo.Requests", new[] { "ID" });
            DropTable("dbo.vwRequestStatistics");
        }
    }
}
