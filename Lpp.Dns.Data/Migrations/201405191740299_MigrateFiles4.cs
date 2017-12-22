namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateFiles4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Documents", new[] {"ItemID"});
            AlterColumn("dbo.Documents", "ItemID", c => c.Guid(nullable: false));
            CreateIndex("dbo.Documents", "ItemID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Documents", new[] { "ItemID" });
            AlterColumn("dbo.Documents", "ItemID", c => c.Guid());
            CreateIndex("dbo.Documents", "ItemID");
        }
    }
}
