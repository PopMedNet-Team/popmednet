namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentRef : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentReferences",
                c => new
                {
                    CommentID = c.Guid(nullable: false),
                    ItemID = c.Guid(nullable: false),
                    Item = c.String(),
                    Type = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.CommentID, t.ItemID })
                .ForeignKey("dbo.Comments", t => t.CommentID, cascadeDelete: true)
                .Index(t => t.CommentID);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentReferences", "CommentID", "dbo.Comments");
            DropIndex("dbo.CommentReferences", new[] { "CommentID" });
            DropTable("dbo.CommentReferences");

        }
    }
}
