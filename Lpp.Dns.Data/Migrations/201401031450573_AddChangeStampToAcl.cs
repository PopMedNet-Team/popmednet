namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChangeStampToAcl : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TABLE AclEntries ADD ChangedOn DateTime NOT NULL CONSTRAINT DF_AclEntries_ChangedOn DEFAULT GETUTCDATE()");

            Sql(@"CREATE TRIGGER [dbo].[ACLEntries_UpdateItem] 
    ON  [dbo].[ACLEntries]
    AFTER UPDATE
AS 
BEGIN
	UPDATE AclEntires SET ChangedOn = GETUTCDATE() where AclEntires.Id IN (SELECT id from inserted)
END");
        }
        
        public override void Down()
        {

        }
    }
}
