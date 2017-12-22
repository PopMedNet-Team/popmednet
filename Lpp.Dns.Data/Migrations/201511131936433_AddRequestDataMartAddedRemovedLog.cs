namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestDataMartAddedRemovedLog : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM [Events] WHERE ID = '8E074A96-D6B6-44D4-9E1A-998392AB7C23') INSERT INTO [Events] (ID, Name, Description) VALUES ('8E074A96-D6B6-44D4-9E1A-998392AB7C23', 'Request DataMart Added or Removed', 'Users will be notified on the History tab of a Request when a DataMart has been added to or removed from the request.')");
            
            CreateTable(
                "dbo.LogsRequestDataMartAddedRemoved",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestDataMartID = c.Guid(nullable: false),
                        TaskID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.RequestDataMarts", t => t.RequestDataMartID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskID)
                .Index(t => t.RequestDataMartID)
                .Index(t => t.TaskID)
                .Index(t => t.EventID);

            Sql(@"ALTER TABLE dbo.LogsRequestDataMartAddedRemoved ADD CONSTRAINT PK_LogsRequestDataMartAddedRemoved PRIMARY KEY (UserID, TimeStamp, RequestDataMartID) ON AuditLogs"); 
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestDataMartAddedRemoved", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsRequestDataMartAddedRemoved", "RequestDataMartID", "dbo.RequestDataMarts");
            DropIndex("dbo.LogsRequestDataMartAddedRemoved", new[] { "EventID" });
            DropIndex("dbo.LogsRequestDataMartAddedRemoved", new[] { "TaskID" });
            DropIndex("dbo.LogsRequestDataMartAddedRemoved", new[] { "RequestDataMartID" });
            DropTable("dbo.LogsRequestDataMartAddedRemoved");

            Sql("DELETE FROM Events WHERE ID = '8E074A96-D6B6-44D4-9E1A-998392AB7C23')");
        }
    }
}
