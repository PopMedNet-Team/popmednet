namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRequestViewsAndTriggersForRoutingType : DbMigration
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
                    Where RoutingType != 0 
                    GROUP BY RequestID");

            Sql(@" ALTER TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
                    ON  [dbo].[RequestDataMarts]
                    AFTER INSERT, UPDATE, DELETE
                AS 
                BEGIN
	                IF ((SELECT COUNT(*) FROM inserted) > 0)
					BEGIN
						IF ((SELECT RoutingType FROM Inserted) = 0)
						BEGIN
						    UPDATE Requests SET UpdatedOn = GETUTCDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
							UPDATE Requests SET Status = 10000 WHERE Requests.ID IN (SELECT RequestID FROM inserted)
						END
						ELSE
						BEGIN
							UPDATE Requests SET UpdatedOn = GETUTCDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
							UPDATE Requests SET Status = 
							-- if request has canceled date, set to canceled
							CASE WHEN NOT CancelledOn IS NULL THEN 9999						
							-- any responses awaiting request approval
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingRequestApproval > 0 AND RequestID = Requests.ID) THEN 300
							-- all routings are complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled = Total AND RequestID = Requests.ID) THEN 10000 
							-- all routings are submitted
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted + Canceled = Total AND RequestID = Requests.ID) THEN 500
							-- more than one response and not all are complete
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE (Total - Canceled - Draft) > 1 AND Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled < Total AND RequestID = Requests.ID) THEN 9000 -- partially complete							
							-- any requests resubmitted
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Resubmitted > 0 AND RequestID = Requests.ID) THEN 600
							-- any responses rejected after upload
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
							-- any responses awaiting approval 
							WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
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
					END

					DELETE FROM LogsNewRequestSubmitted WHERE RequestDataMartID IN (SELECT ID FROM deleted)
					DELETE FROM LogsRequestDataMartMetadataChange WHERE RequestDataMartID IN (SELECT ID FROM deleted)
                END");
        }
        
        public override void Down()
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

            Sql(@" ALTER TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
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
						-- all routings are complete
						WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled = Total AND RequestID = Requests.ID) THEN 10000 
						-- all routings are submitted
						WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted + Canceled = Total AND RequestID = Requests.ID) THEN 500
						-- more than one response and not all are complete
						WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE (Total - Canceled - Draft) > 1 AND Completed + RejectedAfterUploadResults + RejectedBeforeUploadResults + RejectedRequest + Canceled < Total AND RequestID = Requests.ID) THEN 9000 -- partially complete							
						-- any requests resubmitted
						WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Resubmitted > 0 AND RequestID = Requests.ID) THEN 600
						-- any responses rejected after upload
						WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
						-- any responses awaiting approval 
						WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
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
    }
}
