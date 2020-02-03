namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestDataMartMetadataLog : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS(SELECT NULL FROM [Events] WHERE ID = '7535EE61-767E-4C36-BF45-6927B9AFE7C6') INSERT INTO [Events] (ID, [Name], [Description]) VALUES ('7535EE61-767E-4C36-BF45-6927B9AFE7C6', 'Request DataMart Metadata Change', 'Users will be notified whenever there is a change to a request datamarts metadata.')");
            Sql("IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '7535EE61-767E-4C36-BF45-6927B9AFE7C6' AND Location = 11) INSERT INTO EventLocations (EventID, Location) VALUES ('7535EE61-767E-4C36-BF45-6927B9AFE7C6', 11)");

            CreateTable(
                "dbo.LogsRequestDataMartMetadataChange",
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

            Sql(@"ALTER TABLE dbo.LogsRequestDataMartMetadataChange ADD CONSTRAINT PK_LogsRequestDataMartMetadataChange PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs"); 
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestDataMartMetadataChange", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsRequestDataMartMetadataChange", "RequestID", "dbo.Requests");
            DropIndex("dbo.LogsRequestDataMartMetadataChange", new[] { "EventID" });
            DropIndex("dbo.LogsRequestDataMartMetadataChange", new[] { "TaskID" });
            DropIndex("dbo.LogsRequestDataMartMetadataChange", new[] { "RequestID" });
            DropTable("dbo.LogsRequestDataMartMetadataChange");

            Sql("DELETE FROM EventLocations WHERE EventID = '7535EE61-767E-4C36-BF45-6927B9AFE7C6'");
            Sql("DELETE FROM [Events] WHERE ID = '7535EE61-767E-4C36-BF45-6927B9AFE7C6'");
        }
    }
}
