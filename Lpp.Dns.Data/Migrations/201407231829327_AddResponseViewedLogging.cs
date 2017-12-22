namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResponseViewedLogging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsResponseViewed",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        ResponseID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.RequestDataMartResponses", t => t.ResponseID, cascadeDelete: true)
                .Index(t => t.ResponseID);

            Sql(@"ALTER TABLE dbo.LogsResponseViewed ADD CONSTRAINT PK_LogsResponseViewed PRIMARY KEY (UserID, TimeStamp, ResponseID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsResponseViewed", "ResponseID", "dbo.RequestDataMartResponses");
            DropIndex("dbo.LogsResponseViewed", new[] { "ResponseID" });
            DropTable("dbo.LogsResponseViewed");
        }
    }
}
