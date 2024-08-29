namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestDataMarts_UpdateTriggerForWFStatus : DbMigration
    {
        public override void Up()
        {
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
							-- if workflow request with status between draft pending review and rejected or between Pending Draft Report and Final Report Pending Review
							WHEN Requests.WorkFlowActivityID IS NOT NULL AND ((Requests.Status > 300 AND Requests.Status < 400) OR (Requests.Status > 9000 AND Requests.Status < 9500)) THEN Requests.Status
							-- draft status
							ELSE 250
							END
							WHERE Requests.ID IN (SELECT RequestID FROM inserted)
					END

					DELETE FROM LogsNewRequestSubmitted WHERE RequestDataMartID IN (SELECT ID FROM deleted)
					DELETE FROM LogsRequestDataMartMetadataChange WHERE RequestDataMartID IN (SELECT ID FROM deleted)
                END");
        }
        
        public override void Down()
        {
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

					DELETE FROM LogsNewRequestSubmitted WHERE RequestDataMartID IN (SELECT ID FROM deleted)
					DELETE FROM LogsRequestDataMartMetadataChange WHERE RequestDataMartID IN (SELECT ID FROM deleted)
                END");
        }
    }
}
