namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSearchTablesForOrganizationDataMart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestDataMartSearchResults",
                c => new
                    {
                        SearchRequestId = c.Int(nullable: false),
                        ResultDataMartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SearchRequestId, t.ResultDataMartId })
                .ForeignKey("dbo.Queries", t => t.SearchRequestId, cascadeDelete: true)
                .ForeignKey("dbo.DataMarts", t => t.ResultDataMartId, cascadeDelete: true)
                .Index(t => t.SearchRequestId)
                .Index(t => t.ResultDataMartId);
            
            CreateTable(
                "dbo.RequestOrganzationSearchResults",
                c => new
                    {
                        SearchRequestId = c.Int(nullable: false),
                        ResultOrganizationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SearchRequestId, t.ResultOrganizationId })
                .ForeignKey("dbo.Queries", t => t.SearchRequestId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.ResultOrganizationId, cascadeDelete: true)
                .Index(t => t.SearchRequestId)
                .Index(t => t.ResultOrganizationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestOrganzationSearchResults", "ResultOrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.RequestDataMartSearchResults", "ResultDataMartId", "dbo.DataMarts");
            DropForeignKey("dbo.RequestOrganzationSearchResults", "SearchRequestId", "dbo.Queries");
            DropForeignKey("dbo.RequestDataMartSearchResults", "SearchRequestId", "dbo.Queries");
            DropIndex("dbo.RequestOrganzationSearchResults", new[] { "ResultOrganizationId" });
            DropIndex("dbo.RequestDataMartSearchResults", new[] { "ResultDataMartId" });
            DropIndex("dbo.RequestOrganzationSearchResults", new[] { "SearchRequestId" });
            DropIndex("dbo.RequestDataMartSearchResults", new[] { "SearchRequestId" });
            DropTable("dbo.RequestOrganzationSearchResults");
            DropTable("dbo.RequestDataMartSearchResults");
        }
    }
}
