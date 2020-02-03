namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportAggregationLevel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportAggregationLevels",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        NetworkID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 80),
                        DeletedOn = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Networks", t => t.NetworkID, cascadeDelete: false)
                .Index(t => t.NetworkID);
            
            AddColumn("dbo.Requests", "ReportAggregationLevelID", c => c.Guid());
            CreateIndex("dbo.Requests", "ReportAggregationLevelID");
            AddForeignKey("dbo.Requests", "ReportAggregationLevelID", "dbo.ReportAggregationLevels", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ReportAggregationLevelID", "dbo.ReportAggregationLevels");
            DropForeignKey("dbo.ReportAggregationLevels", "NetworkID", "dbo.Networks");
            DropIndex("dbo.ReportAggregationLevels", new[] { "NetworkID" });
            DropIndex("dbo.Requests", new[] { "ReportAggregationLevelID" });
            DropColumn("dbo.Requests", "ReportAggregationLevelID");
            DropTable("dbo.ReportAggregationLevels");
        }
    }
}
