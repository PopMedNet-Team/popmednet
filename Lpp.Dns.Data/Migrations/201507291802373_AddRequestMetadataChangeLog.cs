namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestMetadataChangeLog : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS(SELECT NULL FROM [Events] WHERE ID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1') INSERT INTO [Events] (ID, [Name], [Description]) VALUES ('29AEE006-1C2A-4304-B3C9-8771D96ACDF1', 'Request Metadata Change', 'Users will be notified whenever there is a change to a requests metadata.')");
            Sql("IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1' AND Location = 3) INSERT INTO EventLocations (EventID, Location) VALUES ('29AEE006-1C2A-4304-B3C9-8771D96ACDF1', 3)");
            Sql("IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1' AND Location = 4) INSERT INTO EventLocations (EventID, Location) VALUES ('29AEE006-1C2A-4304-B3C9-8771D96ACDF1', 4)");
            Sql("IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1' AND Location = 11) INSERT INTO EventLocations (EventID, Location) VALUES ('29AEE006-1C2A-4304-B3C9-8771D96ACDF1', 11)");

            CreateTable(
                "dbo.LogsRequestMetadataChange",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestID = c.Guid(nullable: false),
                        TaskID = c.Guid(),
                        ChangeDetail = c.String(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Tasks", t => t.TaskID)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID)
                .Index(t => t.TaskID)
                .Index(t => t.EventID);

            Sql(@"ALTER TABLE dbo.LogsRequestMetadataChange ADD CONSTRAINT PK_LogsRequestMetadataChange PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs"); 
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestMetadataChange", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.LogsRequestMetadataChange", "TaskID", "dbo.Tasks");
            DropIndex("dbo.LogsRequestMetadataChange", new[] { "EventID" });
            DropIndex("dbo.LogsRequestMetadataChange", new[] { "TaskID" });
            DropIndex("dbo.LogsRequestMetadataChange", new[] { "RequestID" });
            DropTable("dbo.LogsRequestMetadataChange");

            Sql("DELETE FROM EventLocations WHERE EventID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1'");
            Sql("DELETE FROM [Events] WHERE ID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1'");
        }
    }
}
