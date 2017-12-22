namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnusedEvents : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM Events WHERE ID = 'D5EF0001-4122-477E-9C55-A2210142C609'");
            Sql(@"DELETE FROM Events WHERE ID = 'D2460001-F0FA-4BAA-AEE1-A22200CCADB4'");
        }
        
        public override void Down()
        {
        }
    }
}
