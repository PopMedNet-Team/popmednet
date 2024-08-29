namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegistrySearchResults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestRegistrySearchResults",
                c => new
                    {
                        SearchRequestId = c.Int(nullable: false),
                        ResultRegistryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.SearchRequestId, t.ResultRegistryId })
                .ForeignKey("dbo.Registries", t => t.ResultRegistryId, cascadeDelete: true)
                .ForeignKey("dbo.Queries", t => t.SearchRequestId, cascadeDelete: true)
                .Index(t => t.ResultRegistryId)
                .Index(t => t.SearchRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestRegistrySearchResults", "SearchRequestId", "dbo.Queries");
            DropForeignKey("dbo.RequestRegistrySearchResults", "ResultRegistryId", "dbo.Registries");
            DropIndex("dbo.RequestRegistrySearchResults", new[] { "SearchRequestId" });
            DropIndex("dbo.RequestRegistrySearchResults", new[] { "ResultRegistryId" });
            DropTable("dbo.RequestRegistrySearchResults");
        }
    }
}
