namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtTriggersForQueryiesUpdate : DbMigration
    {
        public override void Up()
        {
            Sql(@"  CREATE TRIGGER [dbo].[RequestRoutingInstances_InsertUpdateDeleteItem] 
                        ON  [dbo].[requestRoutingInstances]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT RequestId FROM inserted)
	                    ELSE
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT RequestId FROM inserted)
                    END");
            Sql(@"  CREATE TRIGGER [dbo].[QueryDataMarts_InsertUpdateDeleteItem] 
                        ON  [dbo].[QueriesDataMarts]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT QueryId FROM inserted)
	                    ELSE
		                    UPDATE Queries SET Updated = GETDATE() WHERE Queries.QueryID IN (SELECT QueryId FROM inserted)
                    END");
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[RequestRoutingInstances_InsertUpdateDeleteItem]");
            Sql("DROP TRIGGER [dbo].[QueryDataMarts_InsertUpdateDeleteItem]");
        }
    }
}
