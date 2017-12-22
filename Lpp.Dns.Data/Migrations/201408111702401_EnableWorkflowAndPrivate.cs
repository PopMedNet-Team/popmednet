namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableWorkflowAndPrivate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "WorkFlowActivityID", c => c.Guid());
            AddColumn("dbo.Requests", "Private", c => c.Boolean(nullable: false, defaultValue: true));
            Sql(@"  ALTER TRIGGER [dbo].[RequestDataMarts_InsertUpdateDeleteItem] 
                        ON  [dbo].[RequestDataMarts]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN
		                    UPDATE Requests SET UpdatedOn = GETDATE() WHERE Requests.ID IN (SELECT RequestID FROM inserted)
							UPDATE Requests SET Status = 
								CASE WHEN NOT CancelledOn IS NULL THEN 9999 
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed = Total AND RequestID = Requests.ID) THEN 10000
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed > 0 AND RequestID = Requests.ID) THEN 9000
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted > 0 AND RequestID = Requests.ID) THEN 500
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedRequest > 0 AND RequestID = Requests.ID) THEN 400
								WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingRequestApproval > 0 AND RequestID = Requests.ID) THEN 300
								WHEN Requests.Private = 1 THEN 200
								ELSE 250
								END
							 WHERE Requests.ID IN (SELECT RequestID FROM inserted)
						END
                    END");
            Sql(@"  CREATE TRIGGER [dbo].[Requests_InsertUpdateItem] 
                        ON  [dbo].[RequestDataMarts]
                        AFTER INSERT, UPDATE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN
							UPDATE Requests SET Private = 0 WHERE Requests.Status > 300 AND Requests.Status <> 9999
						END
                    END");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Private");
            DropColumn("dbo.Requests", "WorkFlowActivityID");
        }
    }
}
