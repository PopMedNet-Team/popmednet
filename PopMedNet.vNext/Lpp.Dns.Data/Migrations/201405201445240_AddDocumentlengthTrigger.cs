namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentlengthTrigger : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE TRIGGER [dbo].[Documents_Update] 
    ON  [dbo].[Documents]
    AFTER UPDATE
AS 
BEGIN
	IF(EXISTS((SELECT I.ID
          FROM
             INSERTED I JOIN DELETED D ON I.ID = D.ID
          WHERE
             I.Data <> D.Data)))
	BEGIN
	UPDATE Documents SET Length = DATALENGTH(Data) WHERE Documents.ID IN (SELECT I.ID
          FROM
             INSERTED I JOIN DELETED D ON I.ID = D.ID
          WHERE
             I.Data <> D.Data)
	END
END");
        }
        
        public override void Down()
        {
            Sql(@"DROP TRIGGER [dbo].[Documents_Update]");
        }
    }
}
