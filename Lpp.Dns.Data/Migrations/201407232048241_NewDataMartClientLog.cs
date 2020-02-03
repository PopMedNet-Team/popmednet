namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDataMartClientLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsResultsReminder",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestID = c.Guid(nullable: false),
                        Description = c.String(),
                    })                
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID);
            Sql(@"ALTER TABLE dbo.LogsResultsReminder ADD CONSTRAINT PK_LogsResultsReminder PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs");

            CreateTable(
                "dbo.LogsNewDataMartClient",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        LastModified = c.DateTime(nullable: false),
                        Description = c.String(),
                    });

            Sql(@"ALTER TABLE dbo.LogsNewDataMartClient ADD CONSTRAINT PK_LogsNewDataMartClient PRIMARY KEY (UserID, TimeStamp, LastModified) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsResultsReminder", "RequestID", "dbo.Requests");
            DropIndex("dbo.LogsResultsReminder", new[] { "RequestID" });
            DropTable("dbo.LogsNewDataMartClient");
            DropTable("dbo.LogsResultsReminder");
        }
    }
}
