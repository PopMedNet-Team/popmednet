namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableStatusOnRequest2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "ApprovedForDraftOn", c => c.DateTime());
            AddColumn("dbo.Requests", "ApprovedForDraftByID", c => c.Guid());
            AddColumn("dbo.Requests", "RejectedOn", c => c.DateTime());
            AddColumn("dbo.Requests", "RejectedByID", c => c.Guid());
            AddColumn("dbo.Requests", "CancelledOn", c => c.DateTime());
            AddColumn("dbo.Requests", "CancelledByID", c => c.Guid());
            CreateIndex("dbo.Requests", "ApprovedForDraftByID");
            CreateIndex("dbo.Requests", "RejectedByID");
            CreateIndex("dbo.Requests", "CancelledByID");
            AlterColumn("dbo.Requests", "Status", c => c.Int(false, defaultValue: 200));
            CreateIndex("dbo.Requests", "Status");

            Sql(@"ALTER TRIGGER [dbo].[Users_DeleteItem] 
        ON  [dbo].[Users]
        AFTER DELETE
    AS 
    BEGIN
		DELETE FROM SecurityGroups WHERE ID IN (SELECT ID FROM deleted)
		UPDATE Requests SET SubmittedByID = NULL WHERE SubmittedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET CreatedByID = NULL WHERE CreatedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET UpdatedByID = NULL WHERE UpdatedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET RejectedByID = NULL WHERE RejectedByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET ApprovedForDraftByID = NULL WHERE ApprovedForDraftByID IN (SELECT ID FROM deleted)
		UPDATE Requests SET CancelledByID = NULL WHERE CancelledByID IN (SELECT ID FROM deleted)
	END");

            Sql(@"UPDATE Requests SET Status = 

CASE WHEN NOT CancelledOn IS NULL THEN 9999 
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed = Total AND RequestID = Requests.ID) THEN 10000
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Completed > 0 AND RequestID = Requests.ID) THEN 9000
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingResponseApproval > 0 AND RequestID = Requests.ID) THEN 1100
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedAfterUploadResults > 0 AND RequestID = Requests.ID) THEN 900
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedBeforeUploadResults > 0 AND RequestID = Requests.ID) THEN 800
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE Submitted > 0 AND RequestID = Requests.ID) THEN 500
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE RejectedRequest > 0 AND RequestID = Requests.ID) THEN 400
WHEN EXISTS(SELECT NULL FROM vwRequestStatistics WHERE AwaitingRequestApproval > 0 AND RequestID = Requests.ID) THEN 300
WHEN NOT EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestID = Requests.ID) AND ApprovedForDraftByID IS NULL THEN 100
ELSE 200
END");

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
								WHEN NOT EXISTS(SELECT NULL FROM RequestDataMarts WHERE RequestID = Requests.ID) AND ApprovedForDraftByID IS NULL THEN 100
								ELSE 200
								END
							 WHERE Requests.ID IN (SELECT RequestID FROM inserted)
						END
                    END");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "RejectedByID", "dbo.Users");
            DropForeignKey("dbo.Requests", "ApprovedForDraftByID", "dbo.Users");
            DropForeignKey("dbo.Requests", "CancelledByID", "dbo.Users");
            DropIndex("dbo.Requests", new[] { "Status" });
            DropIndex("dbo.Requests", new[] { "CancelledByID" });
            DropIndex("dbo.Requests", new[] { "RejectedByID" });
            DropIndex("dbo.Requests", new[] { "ApprovedForDraftByID" });
            DropColumn("dbo.Requests", "CancelledByID");
            DropColumn("dbo.Requests", "CancelledOn");
            DropColumn("dbo.Requests", "RejectedByID");
            DropColumn("dbo.Requests", "RejectedOn");
            DropColumn("dbo.Requests", "ApprovedForDraftByID");
            DropColumn("dbo.Requests", "ApprovedForDraftOn");
        }
    }
}
