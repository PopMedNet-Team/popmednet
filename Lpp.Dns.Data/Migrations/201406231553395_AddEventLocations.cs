namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventLocations",
                c => new
                    {
                        EventID = c.Guid(nullable: false),
                        Location = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventID, t.Location })
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .Index(t => t.EventID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventLocations", "EventID", "dbo.Events");
            DropIndex("dbo.EventLocations", new[] { "EventID" });
            DropTable("dbo.EventLocations");
        }
    }
}
