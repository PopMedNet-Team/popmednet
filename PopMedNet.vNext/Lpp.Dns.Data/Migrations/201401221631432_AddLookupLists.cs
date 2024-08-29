namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLookupLists : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.LookupListCategories",
            //    c => new
            //        {
            //            ListId = c.Int(nullable: false),
            //            CategoryId = c.Int(nullable: false),
            //            CategoryName = c.String(nullable: false, maxLength: 500, unicode: false),
            //        })
            //    .PrimaryKey(t => new { t.ListId, t.CategoryId });
            
            //CreateTable(
            //    "dbo.LookupLists",
            //    c => new
            //        {
            //            ListId = c.Int(nullable: false, identity: true),
            //            ListName = c.String(nullable: false, maxLength: 50, unicode: false),
            //        })
            //    .PrimaryKey(t => t.ListId);
            
            //CreateTable(
            //    "dbo.LookupListVlaues",
            //    c => new
            //        {
            //            ListId = c.Int(nullable: false),
            //            CategoryId = c.Int(nullable: false),
            //            ItemName = c.String(nullable: false, maxLength: 500, unicode: false),
            //            ItemCode = c.String(nullable: false, maxLength: 200, unicode: false),
            //            ItemCodeWithNoPeriod = c.String(nullable: false, maxLength: 200, unicode: false),
            //            ID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.ListId, t.CategoryId, t.ItemName, t.ItemCode });
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.LookupListVlaues");
            //DropTable("dbo.LookupLists");
            //DropTable("dbo.LookupListCategories");
        }
    }
}
