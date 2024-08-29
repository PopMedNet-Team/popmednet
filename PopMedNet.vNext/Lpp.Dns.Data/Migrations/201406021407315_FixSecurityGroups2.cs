namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityGroups2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SecurityGroups", "OwnerID", c => c.Guid(nullable: false));
            CreateIndex("dbo.SecurityGroups", "OwnerID");

            Sql(@"  ALTER TRIGGER [dbo].[Users_InsertUpdateDelete] 
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
	END");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SecurityGroups", new[] { "OwnerID" });
            AlterColumn("dbo.SecurityGroups", "OwnerID", c => c.Guid());
        }
    }
}
