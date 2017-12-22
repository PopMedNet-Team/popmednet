namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAcleEntriesTrigger : DbMigration
    {
        public override void Up()
        {
            Sql("DROP TRIGGER [dbo].[ACLEntries_UpdateItem]");
            Sql(@"CREATE TRIGGER [dbo].[ACLEntries_UpdateItem] 
    ON  [dbo].[ACLEntries]
    AFTER UPDATE
AS 
BEGIN
	UPDATE AclEntries SET ChangedOn = GETUTCDATE() where AclEntries.Id IN (SELECT id from inserted)
END");

        }
        
        public override void Down()
        {
        }
    }
}
