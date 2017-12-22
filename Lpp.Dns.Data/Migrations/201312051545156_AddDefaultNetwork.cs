namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultNetwork : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Networks (ID, Name, Url) VALUES (dbo.newsqlguid(), 'Default Network', 'http://localhost:14586/')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Networks WHERE Name = 'Default Network'");
        }
    }
}
