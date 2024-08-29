namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixActivatedOnNotSet : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[Users_InsertUpdateDelete] 
		ON  [dbo].[Users]
		AFTER INSERT, UPDATE, DELETE
	AS 
	BEGIN
		IF EXISTS (SELECT *
          FROM
             INSERTED I JOIN DELETED D ON I.ID = D.ID
          WHERE
             I.LastUpdatedOn = D.LastUpdatedOn)
        BEGIN
            UPDATE Users SET LastUpdatedOn = GETUTCDATE() WHERE Users.ID IN (SELECT ID FROM inserted)
        END
		IF EXISTS (SELECT *
          FROM
             INSERTED I JOIN DELETED D ON I.ID = D.ID
          WHERE
             I.isActive = 1 AND D.isActive = 0)
        BEGIN
			UPDATE Users SET ActivatedOn = GETUTCDATE() WHERE ActivatedOn IS NULL AND Users.ID IN (SELECT inserted.ID FROM inserted join deleted ON inserted.ID = deleted.ID WHERE inserted.isActive = 1 AND deleted.isActive = 0)
		END
	END");

            Sql(@"UPDATE Users SET ActivatedOn = GETUTCDATE() WHERE ActivatedOn IS NULL AND isActive = 1");
        }
        
        public override void Down()
        {
        }
    }
}
