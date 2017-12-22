namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateFiles2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "FileName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Documents", "Length", c => c.Long(nullable: false));

            Sql(@"UPDATE Documents SET Data = null

DECLARE @FileSegments AS CURSOR
SET @FileSegments = CURSOR FOR SELECT Documents.ID, Files.MimeType, Files.LastSegmentFill, FileSegments.Data, CASE WHEN FileSegments.[Index] = NumberOfSegments - 1 THEN 1 ELSE 0 END AS LastSegment, Files.Created, Files.SegmentSize, FileSegments.[Index], Files.FileName FROM Documents JOIN Files ON Documents.FileId = Files.Id JOIN FileSegments ON Files.Id = FileSegments.FileId ORDER BY FileSegments.[Index]

OPEN @FileSegments

DECLARE @DocumentID AS UniqueIdentifier
DECLARE @MimeType AS nvarchar(100)
DECLARE @LastSegmentFill AS int
DECLARE @Data AS varbinary(max)
DECLARE @LastSegment AS bit
DECLARE @Created AS DateTime2
DECLARE @SegmentSize AS int
DECLARE @Index as int
DECLARE @FileName AS nvarchar(max)

FETCH NEXT FROM @FileSegments INTO @DocumentID, @MimeType, @LastSegmentFill, @Data, @LastSegment, @Created, @SegmentSize, @Index, @FileName
WHILE @@FETCH_STATUS = 0
BEGIN
	IF (@LastSegment = 1)
	BEGIN
		UPDATE Documents SET MimeType = @MimeType, Data = CASE WHEN Data IS NULL THEN SUBSTRING(@Data, 0, @LastSegmentFill + 1) ELSE Data + SUBSTRING(@Data, 0, @LastSegmentFill + 1) END, [Length] = @SegmentSize * (@Index - 1) + @LastSegmentFill + 1, FileName = @FileName  WHERE Documents.ID = @DocumentID
	END
	ELSE 
	BEGIN
		Update Documents SET Data = CASE WHEN Data IS NULL THEN @Data ELSE Data + @Data END WHERE Documents.ID = @DocumentID
	END

	FETCH NEXT FROM @FileSegments INTO @DocumentID, @MimeType, @LastSegmentFill, @Data, @LastSegment, @Created, @SegmentSize, @Index, @FileName
END

CLOSE @FileSegments
DEALLOCATE @FileSegments");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Documents' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'FileID')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Documents DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'FileSegments' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'FileID')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE FileSegments DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            DropIndex("dbo.Documents", new[] { "FileID" });
            DropIndex("dbo.FileSegments", new[] { "FileID" });
            CreateIndex("dbo.Documents", "FileName");
            DropColumn("dbo.Documents", "FileID");
            DropTable("dbo.Files");
            DropTable("dbo.FileSegments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FileSegments",
                c => new
                    {
                        FileID = c.Int(nullable: false),
                        Index = c.Int(nullable: false),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => new { t.FileID, t.Index });
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        MimeType = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        SegmentSize = c.Int(nullable: false),
                        NumberOfSegments = c.Int(nullable: false),
                        LastSegmentFill = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Documents", "FileID", c => c.Int(nullable: false));
            DropIndex("dbo.Documents", new[] { "FileName" });
            DropColumn("dbo.Documents", "Length");
            DropColumn("dbo.Documents", "FileName");
            CreateIndex("dbo.FileSegments", "FileID");
            CreateIndex("dbo.Documents", "FileID");
            AddForeignKey("dbo.FileSegments", "FileID", "dbo.Files", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Documents", "FileID", "dbo.Files", "Id", cascadeDelete: true);
        }
    }
}
