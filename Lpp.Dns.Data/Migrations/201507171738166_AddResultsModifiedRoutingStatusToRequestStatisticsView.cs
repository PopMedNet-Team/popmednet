namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResultsModifiedRoutingStatusToRequestStatisticsView : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [dbo].[vwRequestStatistics]
                    AS
                    SELECT RequestID, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 1 OR QueryStatusTypeId = 0 THEN 1 ELSE 0 END), 0) AS Draft,
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 2 OR QueryStatusTypeId = 11 THEN 1 ELSE 0 END), 0) AS Submitted, --submitted or hold (which is submitted dmc state)
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 3 OR QueryStatusTypeId = 14 OR QueryStatusTypeId = 16 THEN 1 ELSE 0 END), 0) AS Completed, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 4 THEN 1 ELSE 0 END), 0) AS AwaitingRequestApproval, 
                        ISNULL(SUM(CASE WHEN QueryStatusTypeId = 10 THEN 1 ELSE 0 END), 0) AS AwaitingResponseApproval, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 5 THEN 1 ELSE 0 END), 0) AS RejectedRequest, 
                        ISNULL(SUM(CASE WHEN QueryStatusTypeId = 12 THEN 1 ELSE 0 END), 0) AS RejectedBeforeUploadResults, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 13 THEN 1 ELSE 0 END), 0) AS RejectedAfterUploadResults, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 6 THEN 1 ELSE 0 END), 0) AS Canceled, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 7 THEN 1 ELSE 0 END), 0) AS Resubmitted, 
	                    COUNT(*) AS Total
                    FROM            dbo.RequestDataMarts
                    GROUP BY RequestID");
        }
        
        public override void Down()
        {
            Sql(@"ALTER VIEW [dbo].[vwRequestStatistics]
                    AS
                    SELECT RequestID, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 1 OR QueryStatusTypeId = 0 THEN 1 ELSE 0 END), 0) AS Draft,
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 2 OR QueryStatusTypeId = 11 THEN 1 ELSE 0 END), 0) AS Submitted, --submitted or hold (which is submitted dmc state)
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 3 OR QueryStatusTypeId = 14 THEN 1 ELSE 0 END), 0) AS Completed, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 4 THEN 1 ELSE 0 END), 0) AS AwaitingRequestApproval, 
                        ISNULL(SUM(CASE WHEN QueryStatusTypeId = 10 THEN 1 ELSE 0 END), 0) AS AwaitingResponseApproval, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 5 THEN 1 ELSE 0 END), 0) AS RejectedRequest, 
                        ISNULL(SUM(CASE WHEN QueryStatusTypeId = 12 THEN 1 ELSE 0 END), 0) AS RejectedBeforeUploadResults, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 13 THEN 1 ELSE 0 END), 0) AS RejectedAfterUploadResults, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 6 THEN 1 ELSE 0 END), 0) AS Canceled, 
	                    ISNULL(SUM(CASE WHEN QueryStatusTypeId = 7 THEN 1 ELSE 0 END), 0) AS Resubmitted, 
	                    COUNT(*) AS Total
                    FROM            dbo.RequestDataMarts
                    GROUP BY RequestID");
        }
    }
}
