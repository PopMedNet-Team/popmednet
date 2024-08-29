namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissionLocations2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Permissions", "IX_Type");
            DropColumn("dbo.Permissions", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permissions", "Type", c => c.Int(nullable: false));
            CreateIndex("Permissions", "Type");
        }
    }
}
