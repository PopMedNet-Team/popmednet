namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShrinkDb : DbMigration
    {
        public override void Up()
        {
            // PMN5.0 TWEAKS: SHRINK MANUALLY FROM SMSS.
            // This has the potential of hanging up the database forever.

//            Sql(@"DECLARE @SQL nvarchar(max) = 'DBCC SHRINKDATABASE (' + db_name() + ', 10)'
//                    EXECUTE sp_executesql @SQL", true);
        }
        
        public override void Down()
        {
        }
    }
}
