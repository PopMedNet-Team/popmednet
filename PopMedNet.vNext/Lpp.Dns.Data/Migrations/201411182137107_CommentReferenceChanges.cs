namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentReferenceChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentReferences", "ItemTitle", c => c.String());
            Sql("UPDATE CommentReferences SET ItemTitle = Item WHERE Item IS NOT NULL");
            DropColumn("dbo.CommentReferences", "Item");            
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommentReferences", "Item", c => c.String());
            Sql("UPDATE CommentReferences SET Item = ItemTitle WHERE Item IS NOT NULL");
            DropColumn("dbo.CommentReferences", "ItemTitle");
        }
    }
}
