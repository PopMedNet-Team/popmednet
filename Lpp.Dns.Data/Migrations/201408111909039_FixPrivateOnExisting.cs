namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPrivateOnExisting : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[Requests_InsertUpdateItem] 
                        ON  [dbo].[RequestDataMarts]
                        AFTER INSERT, UPDATE
                    AS 
                    BEGIN
	                    IF ((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN
							UPDATE Requests SET Private = 0 WHERE Requests.Status >= 300 AND Requests.Status <> 9999
						END
                    END");

            Sql(@"UPDATE Requests SET Private = 0 WHERE Status >= 300 AND Status <> 9999");
        }
        
        public override void Down()
        {
        }
    }
}
