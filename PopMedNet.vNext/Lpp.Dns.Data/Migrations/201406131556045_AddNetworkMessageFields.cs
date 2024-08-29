namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNetworkMessageFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NetworkMessages", "Subject", c => c.String(nullable: false));
            RenameColumn("dbo.NetworkMessages", "Created", "CreatedOn");
            CreateIndex("dbo.NetworkMessages", "CreatedOn");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NetworkMessages", new string[] { "CreatedOn" });
            RenameColumn("dbo.NetworkMessages", "CreatedOn", "Created");
            DropColumn("dbo.NetworkMessages", "Subject");
        }
    }
}
