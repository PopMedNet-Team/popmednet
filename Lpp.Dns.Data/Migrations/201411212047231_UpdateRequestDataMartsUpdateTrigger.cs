namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestDataMartsUpdateTrigger : DbMigration
    {
        public override void Up()
        {
            //add Draft status count to view
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

            //use the draft status when checking for partially completed
            Sql(@"ALTER TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
                    ON  [dbo].[RequestDataMarts]
                    AFTER INSERT, UPDATE, DELETE
                AS 
                BEGIN
	                IF ((SELECT COUNT(*) FROM inserted) > 0)
					BEGIN
		                UPDATE Requests SET UpdatedOn = GETUTCDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
						UPDATE Requests SET Status = 
							-- if request has canceled date, set to canceled
							CASE WHEN NOT CancelledOn IS NULL THEN 9999						
							-- any responses awaiting request approval
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingRequestApproval > 0 AND RequestID = Requests.ID) THEN 300
							-- any requests resubmitted
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Resubmitted > 0 AND RequestID = Requests.ID) THEN 600
							-- all routings are complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled = Total AND RequestID = Requests.ID) THEN 10000 
							-- all routings are submitted
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted + Canceled = Total AND RequestID = Requests.ID) THEN 500							
							-- any responses rejected after upload
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
							-- any responses awaiting approval 
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
							-- more than one response and not all are complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE (Total - Canceled - Draft) > 1 AND Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled < Total AND RequestID = Requests.ID) THEN 9000 -- partially complete
							-- any responses rejected before upload
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
							-- any responses with status RejectedRequest
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedRequest > 0 AND RequestID = Requests.ID) THEN 400
							-- if request is marked as private, return private draft
							WHEN Requests.Private = 1 THEN 200
							-- draft status
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
