namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixStreamingCopyOfDocumentsFailing : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[Documents_Update] 
    ON  [dbo].[Documents]
    AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON
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
        }
    }
}
