namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCommentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Text = c.String(),
                        ItemID = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedByID = c.Guid(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.CreatedByID, cascadeDelete: true)
                .Index(t => t.CreatedByID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "CreatedByID", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "CreatedByID" });
            DropTable("dbo.Comments");
        }
    }
}
