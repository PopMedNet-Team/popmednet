namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Lpp.Utilities;
    
    //Notes Remaining:
    // Need to rename SID on projects and SecurityGroups
    // Update all Users and references
    // Need to fix and populate the request types table and set the FK.
    // Need to add SecurityGroupUsers and populate
    // Need to add Security tables and populate
    // Need to add as many other entities to the model as possible
    // Add SubmittedByID after updating users

    public partial class MigrateQueriesToRequests1 : DbMigration
    {
        public override void Up()
        {
            //Remove
            Sql("DROP View RoutingCounts");
            Sql("DROP View _RoutingCounts");
            Sql("DROP View Requests");
            Sql("DROP VIEW DNS3_Organizations");
            Sql("DROP VIEW DNS3_Groups");
            Sql("DROP VIEW DNS3_DataMarts");
            Sql("DROP VIEW DataMartRequests");
            Sql("DROP VIEW vwUsers");
            Sql("DROP VIEW vwUsedRequestTypes");
            Sql("DROP VIEW UsedRequestTypes");
            Sql("DROP VIEW LookUpQueryTypeMetrics_view");
            
            //RequestDataMarts
            AddColumn("RequestDataMarts", "ID", c => c.Guid(false, defaultValueSql: "[dbo].[newsqlguid]()"));
            DropPrimaryKey("RequestDataMarts");
            AddColumn("RequestDataMarts", "RequestID", c => c.Guid(true));
            Sql(
                "UPDATE RequestDataMarts SET RequestID = (SELECT TOP 1 SID FROM Queries WHERE QueryId = RequestDataMarts.QueryId)");
            DropIndex("RequestDataMarts", "_dta_index_QueriesDataMarts_16_1666104976__K1_K3");
            DropIndex("RequestDataMarts", "RequestDataMart_Request");
            DropColumn("RequestDataMarts", "QueryId");
            Sql("DELETE FROM [RequestDataMarts] WHERE [RequestId] IS NULL");
            AlterColumn("RequestDataMarts", "RequestID", c => c.Guid(false));
            AddPrimaryKey("RequestDataMarts", "ID");
            
            //RequestResponses
            Sql("DROP TRIGGER [dbo].[RequestRoutingInstances_InsertUpdateDeleteItem]");
            Sql(MigrationHelpers.DropPrimaryKeyScript("RequestResponses"));
            DropColumn("RequestResponses", "Id");
            RenameColumn("RequestResponses", "SID", "ID");
            AddColumn("RequestResponses", "RequestDataMartID", c => c.Guid(true));
            Sql(
                "UPDATE RequestResponses SET RequestDataMartID = (SELECT ID FROM RequestDataMarts INNER JOIN Queries ON RequestDataMarts.RequestID = Queries.SID WHERE Queries.QueryId = RequestResponses.RequestId AND RequestDataMarts.DataMartID = RequestResponses.DataMartID)");
            Sql(MigrationHelpers.DropForeignKeyScript("RequestResponses", "RequestId"));
            Sql(MigrationHelpers.DropForeignKeyScript("RequestResponses", "DataMartID"));
            DropColumn("RequestResponses", "RequestId");
            DropColumn("RequestResponses", "DataMartID");
            //Update ResponseGroup information properly with the ResponseGroups being on the Request?
            
            //Create Count column and set it's default value to a query of existing items + 1.            
            RenameTable("RequestResponses", "RequestDataMartResponses");
            AddColumn("RequestDataMartResponses", "Count", c => c.Int(false, defaultValue: 0));
            Sql(
                "UPDATE RequestDataMartResponses SET Count = (SELECT COUNT(*) FROM RequestDataMartResponses r WHERE r.RequestDataMartID = RequestDataMartResponses.RequestDataMartID AND r.SubmitTime < RequestDataMartResponses.SubmitTime) + 1");
            //Create the trigger for inserts that updates the count so it doesn't have to be done in code. This will be set to computed when the entity is updated.
            Sql(@"CREATE TRIGGER [dbo].RequestDataMartResponsesInsert 
		ON  [dbo].[RequestDataMartResponses]
		AFTER INSERT
	AS 
	BEGIN
		UPDATE RequestDataMartResponses SET Count = (SELECT COUNT(*) FROM RequestDataMartResponses r WHERE r.RequestDataMartID = RequestDataMartResponses.RequestDataMartID AND r.SubmitTime < RequestDataMartResponses.SubmitTime) + 1 WHERE RequestDataMartResponses.ID IN (SELECT ID FROM inserted)
	END");
            //Drop the isCurrent column as it's no longer needed.
            DropColumn("RequestDataMartResponses", "isCurrent");

            //RequestDataMartSearchResults
            //RequestOrganizationSearchResults
            //RequestSearchResults
            //RequestRegistrySearchResults
                //Take all of these and merge them into a single table. Silly that it's multiple when it's just a ref to the ID.
            CreateTable(
                "dbo.RequestDataMartResponseSearchResults",
                c => new
                {
                    RequestDataMartResponseID = c.Guid(false),
                    ItemID = c.Guid(false)
                })
                .PrimaryKey(t => new { t.RequestDataMartResponseID, t.ItemID });

            //Because this was done incorrectly and the results should have been per data mart we're going to put the results on the first data mart
            Sql(
                "INSERT INTO RequestDataMartResponseSearchResults (RequestDataMartResponseID, ItemID) SELECT (SELECT TOP 1 RequestDataMartResponses.ID FROM RequestDataMarts JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMarts.RequestID = Queries.SID) AS RequestDataMartResponseID, ResultDataMartID FROM Queries JOIN RequestDataMartSearchResults ON SearchRequestId = Queries.QueryId");
            DropTable("RequestDataMartSearchResults");

            Sql(
                "INSERT INTO RequestDataMartResponseSearchResults (RequestDataMartResponseID, ItemID) SELECT (SELECT TOP 1 RequestDataMartResponses.ID FROM RequestDataMarts JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMarts.RequestID = Queries.SID) AS RequestDataMartResponseID, ResultOrganizationID FROM Queries JOIN RequestOrganizationSearchResults ON SearchRequestId = Queries.QueryId");
            DropTable("RequestOrganizationSearchResults");

            Sql(
                "INSERT INTO RequestDataMartResponseSearchResults (RequestDataMartResponseID, ItemID) SELECT (SELECT TOP 1 RequestDataMartResponses.ID FROM RequestDataMarts JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMarts.RequestID = Queries.SID) AS RequestDataMartResponseID, ResultRegistryID FROM Queries JOIN RequestRegistrySearchResults ON SearchRequestId = Queries.QueryId");
            DropTable("RequestRegistrySearchResults");

            Sql(
                "INSERT INTO RequestDataMartResponseSearchResults (RequestDataMartResponseID, ItemID) SELECT (SELECT TOP 1 RequestDataMartResponses.ID FROM RequestDataMarts JOIN RequestDataMartResponses ON RequestDataMarts.ID = RequestDataMartResponses.RequestDataMartID WHERE RequestDataMarts.RequestID = Queries.SID) AS RequestDataMartResponseID, (SELECT TOP 1 SID FROM Queries WHERE Queries.QueryId = RequestSearchResults.ResultRequestId) AS ItemID FROM Queries JOIN RequestSearchResults ON SearchRequestId = Queries.QueryId");
            DropTable("RequestSearchResults");

            //RequestSearchTerms
            DropPrimaryKey("RequestSearchTerms", "PK_QuerySearchTerms_SearchTagId");
            DropIndex("RequestSearchTerms", "IX_QuerySearchTerms");
            DropColumn("RequestSearchTerms", "Id");
            AddColumn("RequestSearchTerms", "ID", c => c.Guid(false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddPrimaryKey("RequestSearchTerms", "ID");
            AddColumn("RequestSearchTerms", "RequestSID", c => c.Guid(true));
            Sql(
                "UPDATE RequestSearchTerms SET RequestSID = (SELECT TOP 1 SID FROM Queries WHERE QueryId = RequestSearchTerms.RequestId)");
            Sql("DELETE RequestSearchTerms WHERE RequestSID IS NULL");
            AlterColumn("RequestSearchTerms", "RequestSID", c => c.Guid(false));
            DropColumn("RequestSearchTerms", "RequestId");
            RenameColumn("RequestSearchTerms", "RequestSID", "RequestID");
            CreateIndex("RequestSearchTerms", "RequestID");

            //RequestSharedFolders_Request
            Sql(MigrationHelpers.DropPrimaryKeyScript("RequestSharedFolders_Request"));
            Sql(MigrationHelpers.DropForeignKeyScript("RequestSharedFolders_Request", "FolderId"));
            Sql(MigrationHelpers.DropForeignKeyScript("RequestSharedFolders_Request", "RequestId"));
            AddColumn("RequestSharedFolders_Request", "RequestSharedFolderID", c => c.Guid(true));
            Sql(
                "UPDATE RequestSharedFolders_Request SET RequestSharedFolderID = (SELECT TOP 1 SID from RequestSharedFolders WHERE Id = RequestSharedFolders_Request.FolderId)");
            DropColumn("RequestSharedFolders_Request", "FolderId");
            AlterColumn("RequestSharedFolders_Request", "RequestSharedFolderID", c => c.Guid(false));

            AddColumn("RequestSharedFolders_Request", "RequestSID", c => c.Guid(true));
            Sql(
                "UPDATE RequestSharedFolders_Request SET RequestSID = (SELECT TOP 1 SID FROM Queries WHERE QueryId = RequestSharedFolders_Request.RequestId)");
            DropColumn("RequestSharedFolders_Request", "RequestId");
            AlterColumn("RequestSharedFolders_Request", "RequestSID", c => c.Guid(false));
            RenameColumn("RequestSharedFolders_Request", "RequestSID", "RequestID");
            RenameTable("RequestSharedFolders_Request", "RequestSharedFolderRequests");

            //RequestSharedFolders
            Sql(MigrationHelpers.DropPrimaryKeyScript("RequestSharedFolders"));
            DropColumn("RequestSharedFolders", "Id");
            RenameColumn("RequestSharedFolders", "SID", "ID");
            AddPrimaryKey("RequestSharedFolders", "ID");
            AlterColumn("RequestSharedFolders", "Name", c => c.String(false, 255));
            CreateIndex("RequestSharedFolders", "Name");

            //Response Groups
            AddColumn("ResponseGroups", "SID", c => c.Guid(false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddColumn("RequestDataMartResponses", "ResponseGroupSID", c => c.Guid(true));
            Sql(MigrationHelpers.DropForeignKeyScript("RequestDataMartResponses", "ResponseGroupId"));
            Sql(MigrationHelpers.DropPrimaryKeyScript("ResponseGroups"));
            Sql(
                "UPDATE RequestDataMartResponses SET ResponseGroupSID = (SELECT TOP 1 SID FROM ResponseGroups WHERE Id = RequestDataMartResponses.ResponseGroupId) WHERE NOT RequestDataMartResponses.ResponseGroupId IS NULL");
            DropColumn("RequestDataMartResponses", "ResponseGroupId");
            RenameColumn("RequestDataMartResponses", "ResponseGroupSID", "ResponseGroupID");
            DropColumn("ResponseGroups", "Id");
            RenameColumn("ResponseGroups", "SID", "ID");
            AddPrimaryKey("ResponseGroups", "ID");            

            //Queries
            DropPrimaryKey("Queries", "PK_Queries");
            Sql(MigrationHelpers.ClearStatsScript("Queries"));
            DropIndex("Queries", "_dta_index_Queries_16_98099390__K1_K30");
            DropIndex("Queries", "ix_sid");
            RenameColumn("Queries", "SID", "ID");
            RenameTable("Queries", "Requests");
            AddPrimaryKey("Requests", "ID");
            DropIndex("Requests", "Queries_Type");
            RenameColumn("Requests", "RequestTypeId", "RequestTypeID");
            CreateIndex("Requests", "RequestTypeID");
            
            //Add additional stuff back

            //RequestDataMarts
            //AddForeignKey("RequestDataMarts", "RequestID", "Requests", "ID", true); --Can't be added because of cycles

            //RequestDataMartResponses            
            Sql(@"  CREATE TRIGGER [dbo].[RequestDataMartResponses_InsertUpdateDelete] 
		ON  [dbo].[RequestDataMartResponses]
		AFTER INSERT, UPDATE, DELETE
	AS 
	BEGIN
		IF ((SELECT COUNT(*) FROM inserted) > 0)
			UPDATE Requests SET Updated = GETUTCDATE() WHERE Requests.ID IN (SELECT RequestDataMarts.RequestID FROM inserted JOIN RequestDataMarts ON inserted.RequestDataMartID = RequestDataMarts.ID)
	END");
            AddPrimaryKey("RequestDataMartResponses", "ID");
            AddForeignKey("RequestDataMartResponses", "RequestDataMartID", "RequestDataMarts", "ID", true);
            CreateIndex("RequestDataMartResponses", "Count");
            CreateIndex("RequestDataMartResponses", new string[] {"RequestDataMartID", "Count"});
            //Update the response group FK etc.
            Sql(
                @"ALTER TABLE RequestDataMartResponses ADD CONSTRAINT FK_RequestDataMartResponses_ResponseGroups_ResponseGroupID FOREIGN KEY (ResponseGroupID) REFERENCES ResponseGroups(ID) ON DELETE SET NULL ON UPDATE CASCADE");

            //Search results Add triggers to each of the tables that it could be and delete if ItemID is in on delete
            Sql(@"  CREATE TRIGGER [dbo].DataMartsDelete 
		ON  [dbo].[DataMarts]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
	END");
            Sql(@"  CREATE TRIGGER [dbo].RequestsDelete 
		ON  [dbo].[Requests]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
        DELETE FROM RequestDataMarts WHERE RequestID IN (SELECT ID FROM deleted)
	END");
            Sql(@"  CREATE TRIGGER [dbo].RegistriesDelete 
		ON  [dbo].[Registries]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
	END");
            Sql(@"ALTER TRIGGER [dbo].[OrganizationDelete] 
    ON  [dbo].[Organizations]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE Organizations SET ParentOrganizationID = NULL WHERE ID IN (SELECT ID FROM deleted)
    DELETE FROM Users WHERE OrganizationID IN (SELECT ID FROM deleted)
    DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
END");

            //RequestSearchTerms
            AddForeignKey("RequestSearchTerms", "RequestID", "Requests", "ID", true);

            //RequestSharedFolderRequests
            AddPrimaryKey("RequestSharedFolderRequests", new string[] {"RequestSharedFolderID", "RequestID"});
            AddForeignKey("RequestSharedFolderRequests", "RequestSharedFolderID", "RequestSharedFolders", "ID", true);
            AddForeignKey("RequestSharedFolderRequests", "RequestID", "Requests", "ID", true);

            //RoutingCounts
            Sql(@"CREATE VIEW [dbo].[vwRequestCounts]
WITH SCHEMABINDING
AS
SELECT RequestID,
	Sum(case when QueryStatusTypeId = 2 then 1 else 0 end) as Submitted,
	Sum(case when QueryStatusTypeId = 3 OR QueryStatusTypeId = 14 then 1 else 0 end) as Completed,
	Sum(case when QueryStatusTypeId = 4 then 1 else 0 end) as AwaitingRequestApproval,
	Sum(case when QueryStatusTypeId = 10 then 1 else 0 end) as AwaitingResponseApproval,
	Sum(case when QueryStatusTypeId = 5 then 1 else 0 end) as RejectedRequest,
	Sum(case when QueryStatusTypeId = 12 then 1 else 0 end) as RejectedBeforeUploadResults,
	Sum(case when QueryStatusTypeId = 13 then 1 else 0 end) as RejectedAfterUploadResults,
	COUNT_BIG(*) as Total
FROM
	dbo.RequestDataMarts
GROUP BY RequestID");
        }
        
        public override void Down()
        {
        }
    }
}
