namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoggingFile : DbMigration
    {
        public override void Up()
        {
            //This creates the new file so that we have an audit logs table.
            Sql(@"DECLARE @SQL NVARCHAR(MAX) = 'ALTER DATABASE [' + db_name() + '] ADD FILEGROUP AuditLogs'
PRINT @SQL
EXECUTE sp_executesql @SQL", true);

            Sql(@"DECLARE @folder nvarchar(max) 
SELECT TOP 1 @folder = SUBSTRING(filename, 1, len(filename) - CHARINDEX('\',REVERSE(filename)) +1) FROM sys.sysfiles
                    SET @folder = @folder + db_name() + '_Logs.ndf'

DECLARE @SQL NVARCHAR(MAX) = 'ALTER DATABASE [' + db_name() + '] ADD FILE 
(
    NAME = AuditLogs,
    FILENAME = ''' + @folder + ''',
    SIZE = 5MB,
    FILEGROWTH = 5MB
) TO FILEGROUP AuditLogs'

PRINT @SQL
EXECUTE sp_executesql @SQL", true);


        }
        
        public override void Down()
        {
        }
    }
}
