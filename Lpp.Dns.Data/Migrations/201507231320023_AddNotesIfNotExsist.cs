namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesIfNotExsist : DbMigration
    {
        public override void Up()
        {
            Sql("if not exists (select column_name from INFORMATION_SCHEMA.columns where table_name = 'Templates' and column_name = 'Notes') alter table Templates add Notes varchar(max)");
        }
        
        public override void Down()
        {
        }
    }
}
