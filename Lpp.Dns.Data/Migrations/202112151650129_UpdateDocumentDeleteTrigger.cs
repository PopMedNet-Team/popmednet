namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDocumentDeleteTrigger : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER[dbo].[Documents_Delete]
            ON[dbo].[Documents]
                    for Delete
                AS
                BEGIN

                if (SELECT COUNT(*)

                    FROM Documents

                    WHERE RevisionSetID IN(SELECT deleted.revisionsetid from deleted)) < 2

                    BEGIN
                        delete from RequestDocuments Where RevisionSetID IN(select DELETED.RevisionSetID from DELETED)

                    END
                END
            GO");
        }
        
        public override void Down()
        {
            Sql(@"ALTER TRIGGER[dbo].[Documents_Delete]
                +ON[dbo].[Documents]
                    for Delete
                AS
                BEGIN
                    delete from RequestDocuments Where RevisionSetID IN(select DELETED.RevisionSetID from DELETED)
                END
            GO");
        }
    }
}
