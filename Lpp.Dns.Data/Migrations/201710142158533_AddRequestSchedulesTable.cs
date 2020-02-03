namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestSchedulesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestSchedules",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RequestID = c.Guid(nullable: false),
                        ScheduleID = c.String(),
                        ScheduleType = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestSchedules", "RequestID", "dbo.Requests");
            DropIndex("dbo.RequestSchedules", new[] { "RequestID" });
            DropTable("dbo.RequestSchedules");
        }
    }
}
