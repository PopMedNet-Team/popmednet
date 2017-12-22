namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestTypeTerms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestTypeTerms",
                c => new
                    {
                        RequestTypeID = c.Guid(nullable: false),
                        TermID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestTypeID, t.TermID })
                .ForeignKey("dbo.Terms", t => t.TermID, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .Index(t => t.RequestTypeID)
                .Index(t => t.TermID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestTypeTerms", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.RequestTypeTerms", "TermID", "dbo.Terms");
            DropIndex("dbo.RequestTypeTerms", new[] { "TermID" });
            DropIndex("dbo.RequestTypeTerms", new[] { "RequestTypeID" });
            DropTable("dbo.RequestTypeTerms");
        }
    }
}
