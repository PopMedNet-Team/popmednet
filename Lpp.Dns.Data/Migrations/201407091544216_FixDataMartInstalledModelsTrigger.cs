namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDataMartInstalledModelsTrigger : DbMigration
    {
        public override void Up()
        {
            Sql(@"  ALTER TRIGGER [dbo].[DataMartInstalledModels_InsertUpdateDeleteItem] 
                        ON  [dbo].[DataMartInstalledModels]
                        AFTER INSERT, UPDATE, DELETE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
		                    UPDATE Requests SET UpdatedOn = GETUTCDATE() WHERE Requests.ID IN (SELECT RequestDataMarts.RequestID FROM inserted JOIN RequestDataMarts ON inserted.DataMartId = RequestDataMarts.DataMartId)
	                    ELSE
		                    UPDATE Requests SET UpdatedOn = GETDATE() WHERE REquests.ID IN (SELECT RequestDataMarts.RequestID FROM deleted JOIN RequestDataMarts ON deleted.DataMartId = RequestDataMarts.DataMartId)
                    END");
        }
        
        public override void Down()
        {
        }
    }
}
