namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeactivatedInfoToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DeactivatedOn", c => c.DateTime());
            AddColumn("dbo.Users", "DeactivatedByID", c => c.Guid());
            AddColumn("dbo.Users", "DeactivationReason", c => c.String());
            CreateIndex("dbo.Users", "DeactivatedOn");
            CreateIndex("dbo.Users", "DeactivatedByID");
            AddForeignKey("dbo.Users", "DeactivatedByID", "dbo.Users", "ID");

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
			UPDATE Users SET ActivatedOn = GETUTCDATE(), DeactivatedOn = NULL, DeactivatedByID = NULL, DeactivationReason = NULL WHERE ActivatedOn IS NULL AND Users.ID IN (SELECT inserted.ID FROM inserted join deleted ON inserted.ID = deleted.ID WHERE inserted.isActive = 1 AND deleted.isActive = 0)
		END
	END");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "DeactivatedByID", "dbo.Users");
            DropIndex("dbo.Users", new[] { "DeactivatedByID" });
            DropIndex("dbo.Users", new[] { "DeactivatedOn" });
            DropColumn("dbo.Users", "DeactivationReason");
            DropColumn("dbo.Users", "DeactivatedByID");
            DropColumn("dbo.Users", "DeactivatedOn");
        }
    }
}
