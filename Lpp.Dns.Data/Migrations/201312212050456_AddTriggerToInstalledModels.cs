namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTriggerToInstalledModels : DbMigration
    {
        public override void Up()
        {
            Sql(@"  CREATE TRIGGER [dbo].[DataMartInstalledModels_InsertUpdateDeleteItem] 
                        ON  [dbo].[DataMartInstalledModels]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT QueriesDataMarts.QueryId FROM inserted JOIN QueriesDataMarts ON inserted.DataMartId = QueriesDataMarts.DataMartId)
	                    ELSE
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT QueriesDataMarts.QueryId FROM deleted JOIN QueriesDataMarts ON deleted.DataMartId = QueriesDataMarts.DataMartId)
                    END");

            Sql(@"  ALTER TRIGGER [dbo].[QueryDataMarts_InsertUpdateDeleteItem] 
                        ON  [dbo].[QueriesDataMarts]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT QueryId FROM inserted)
	                    ELSE
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT QueryId FROM deleted)
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
