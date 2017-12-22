namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateFiles5 : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM Documents WHERE Data IS NULL");
            Sql(@"UPDATE Documents SET Length = DATALENGTH(Data)");
        }
        
        public override void Down()
        {
        }
    }
}
