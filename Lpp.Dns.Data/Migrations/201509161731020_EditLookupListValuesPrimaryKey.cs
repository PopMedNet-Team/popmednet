namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditLookupListValuesPrimaryKey : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [dbo].[LookupListValues] DROP CONSTRAINT [PK_LookupListValues]");
            AddPrimaryKey("dbo.LookupListValues", "ID");
            CreateIndex("dbo.LookupListValues", new[] { "ListId", "CategoryId", "ItemName", "ItemCode" }, false, "IX_LookupListValues", false);
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.LookupListValues");
            AddPrimaryKey("dbo.LookupListValues", new[] { "ListId", "CategoryId", "ItemName", "ItemCode" });
            DropIndex("dbo.LookupListValues", new[] { "ListId", "CategoryId", "ItemName", "ItemCode" });
        }
    }
}
