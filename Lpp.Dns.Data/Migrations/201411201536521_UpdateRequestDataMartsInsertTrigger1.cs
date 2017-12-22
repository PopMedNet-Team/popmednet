namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestDataMartsInsertTrigger1 : DbMigration
    {
        public override void Up()
        {
            //Limit the partially completed to only if there are more than 1 routing outstanding.
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
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled = Total AND RequestID = Requests.ID) THEN 10000 -- all routings are complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted + Canceled = Total AND RequestID = Requests.ID) THEN 500 -- all routings are submitted
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Total - Canceled > 1 AND Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled < Total AND RequestID = Requests.ID) THEN 9000 -- partially complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
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
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled = Total AND RequestID = Requests.ID) THEN 10000 -- all routings are complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted + Canceled = Total AND RequestID = Requests.ID) THEN 500 -- all routings are submitted
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled < Total AND RequestID = Requests.ID) THEN 9000 -- partially complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
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
    }
}
