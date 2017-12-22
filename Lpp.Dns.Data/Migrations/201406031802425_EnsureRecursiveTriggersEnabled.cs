namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnsureRecursiveTriggersEnabled : DbMigration
    {
        public override void Up()
        {
            Sql(@"  DECLARE @SQL nvarchar(max) = 'ALTER DATABASE [' + db_name() + '] SET RECURSIVE_TRIGGERS ON'
                    EXECUTE sp_executesql @SQL", true);
        }
        
        public override void Down()
        {
            Sql(@"  DECLARE @SQL nvarchar(max) = 'ALTER DATABASE [' + db_name() + '] SET RECURSIVE_TRIGGERS OFF'
                    EXECUTE sp_executesql @SQL", true);
        }
    }
}
