namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableFileStreaming : DbMigration
    {
        public override void Up()
        {
            Sql(@"  EXEC sp_configure filestream_access_level, 2
                    RECONFIGURE", true);

            //Add the file group
            Sql(@"  DECLARE @SQL nvarchar(max) = 'ALTER DATABASE [' + db_name() + '] ADD FILEGROUP Files CONTAINS FILESTREAM'
                    EXECUTE sp_executesql @SQL", true);

            //Add the file/folder
            Sql(@"  DECLARE @folder nvarchar(max) 
                    SELECT TOP 1 @folder = SUBSTRING(filename, 1, len(filename) - CHARINDEX('\',REVERSE(filename)) +1) FROM sys.sysfiles
                    SET @folder = @folder + db_name() + '_Files'
                    DECLARE @SQL nvarchar(max) 
                    SET @SQL = 'ALTER DATABASE [' + db_name() + '] ADD FILE (NAME=''Files'', FILENAME=''' + @folder + ''') TO FILEGROUP Files'
                    PRINT @SQL
                    EXECUTE sp_executesql @SQL", true);

            //Add ID column to Documents table and make it the rowcoluniqueidentifier
            Sql(@"ALTER TABLE [dbo].[Documents] ADD [ID] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL UNIQUE DEFAULT ([dbo].[newsqlguid]())");

            //Drop the pk int
            Sql(@"ALTER TABLE Documents DROP CONSTRAINT Pk_Documents_DocId");
            //Make the ID column the PK.
            Sql(@"ALTER TABLE Documents ADD CONSTRAINT Pk_Documents_ID PRIMARY KEY (ID)");

            //Add Data Column as file stream
            Sql(@"ALTER TABLE Documents ADD [Data] [varbinary](max) FILESTREAM NULL");

            //Add Mime Type
            AddColumn("dbo.Documents", "MimeType", c => c.String(false, 100));
        }
        
        public override void Down()
        {
        }
    }
}
