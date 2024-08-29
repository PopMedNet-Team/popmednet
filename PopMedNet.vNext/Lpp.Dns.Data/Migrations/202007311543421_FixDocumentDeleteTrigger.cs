namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDocumentDeleteTrigger : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[Documents_Delete] 
                    ON  [dbo].[Documents]
                    for Delete
                AS 
                BEGIN
	                delete from RequestDocuments Where RevisionSetID IN (select DELETED.RevisionSetID from DELETED)
                END");
        }

        public override void Down()
        {
            Sql(@"ALTER TRIGGER [dbo].[Documents_Delete] 
                    ON  [dbo].[Documents]
                    for Delete
                AS 
                BEGIN
	                delete from RequestDocuments Where RevisionSetID = (select DELETED.RevisionSetID from DELETED)
                END");
        }
    }
}
