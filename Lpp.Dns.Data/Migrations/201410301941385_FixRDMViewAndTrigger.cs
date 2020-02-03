namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRDMViewAndTrigger : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [dbo].[vwRequestStatistics]
AS
SELECT RequestID, 
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
            Sql(@"ALTER TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
                    ON  [dbo].[RequestDataMarts]
                    AFTER INSERT, UPDATE, DELETE
                AS 
                BEGIN
	                IF ((SELECT COUNT(*) FROM inserted) > 0)
					BEGIN
		                UPDATE Requests SET UpdatedOn = GETUTCDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
						UPDATE Requests SET Status = 
							CASE WHEN NOT CancelledOn IS NULL THEN 9999 
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled = Total AND RequestID = Requests.ID) THEN 10000
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed > 0 AND RequestID = Requests.ID) THEN 9000
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted > 0 AND RequestID = Requests.ID) THEN 500
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedRequest > 0 AND RequestID = Requests.ID) THEN 400
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingRequestApproval > 0 AND RequestID = Requests.ID) THEN 300
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Resubmitted > 0 AND RequestID = Requests.ID) THEN 600
							WHEN Requests.Private = 1 THEN 200
							ELSE 250
							END
							WHERE Requests.ID IN (SELECT RequestID FROM inserted)
					END
                END");
        }
        
        public override void Down()
        {
        }
    }
}
