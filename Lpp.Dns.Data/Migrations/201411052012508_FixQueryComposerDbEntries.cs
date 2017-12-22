namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixQueryComposerDbEntries : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE DataModels SET Name = 'Query Composer' WHERE ID = '455C772A-DF9B-4C6B-A6B0-D4FD4DD98488'");
            Sql("DELETE FROM RequestTypes WHERE ID = 'A3044773-8387-4C1B-8139-92B281D0467C'");
        }
        
        public override void Down()
        {
        }
    }
}
