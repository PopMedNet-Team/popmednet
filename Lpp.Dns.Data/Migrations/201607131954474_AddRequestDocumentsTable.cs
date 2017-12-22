namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestDocumentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestDocuments",
                c => new
                    {
                        RevisionSetID = c.Guid(nullable: false),
                        ResponseID = c.Guid(nullable: false),
                        DocumentType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RevisionSetID, t.ResponseID })
                .ForeignKey("dbo.RequestDataMartResponses", t => t.ResponseID)
                .Index(t => t.ResponseID);

            Sql(@"CREATE TRIGGER [dbo].[Documents_Delete] 
                    ON  [dbo].[Documents]
                    for Delete
                AS 
                BEGIN
	                delete from RequestDocuments Where RevisionSetID = (select DELETED.RevisionSetID from DELETED)
                END");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestDocuments", "ResponseID", "dbo.RequestDataMartResponses");
            DropIndex("dbo.RequestDocuments", new[] { "ResponseID" });
            DropTable("dbo.RequestDocuments");
        }
    }
}
