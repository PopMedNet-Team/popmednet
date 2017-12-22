namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestCommentChangeLog : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM [Events] WHERE ID = 'E7160001-D933-476E-A706-A43C0137D4E9') INSERT INTO [Events] (ID, Name, Description) VALUES ('E7160001-D933-476E-A706-A43C0137D4E9', 'Request Comment Change', 'Users will be notified when a new request comment is added.')");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = 'E7160001-D933-476E-A706-A43C0137D4E9' AND Location = 3) INSERT INTO EventLocations (EventID, Location) VALUES ('E7160001-D933-476E-A706-A43C0137D4E9', 3)");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = 'E7160001-D933-476E-A706-A43C0137D4E9' AND Location = 4) INSERT INTO EventLocations (EventID, Location) VALUES ('E7160001-D933-476E-A706-A43C0137D4E9', 4)");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = 'E7160001-D933-476E-A706-A43C0137D4E9' AND Location = 9) INSERT INTO EventLocations (EventID, Location) VALUES ('E7160001-D933-476E-A706-A43C0137D4E9', 9)");

            CreateTable(
                "dbo.LogsRequestCommentChange",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        CommentID = c.Guid(nullable: false),
                        TaskID = c.Guid(),
                        Reason = c.Int(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Comments", t => t.CommentID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskID)
                .Index(t => t.CommentID)
                .Index(t => t.TaskID)
                .Index(t => t.EventID);

            Sql(@"ALTER TABLE dbo.LogsRequestCommentChange ADD CONSTRAINT PK_LogsRequestCommentChange PRIMARY KEY (UserID, TimeStamp, CommentID) ON AuditLogs");            
        }
        
        public override void Down()
        {   
            DropForeignKey("dbo.LogsRequestCommentChange", "TaskID", "dbo.Tasks");
            DropForeignKey("dbo.LogsRequestCommentChange", "CommentID", "dbo.Comments");
            DropIndex("dbo.LogsRequestCommentChange", new[] { "EventID" });
            DropIndex("dbo.LogsRequestCommentChange", new[] { "TaskID" });
            DropIndex("dbo.LogsRequestCommentChange", new[] { "CommentID" });
            DropTable("dbo.LogsRequestCommentChange");

            Sql("DELETE FROM EventLocations WHERE EventID = 'E7160001-D933-476E-A706-A43C0137D4E9'");
            Sql("DELETE FROM Events WHERE ID = 'E7160001-D933-476E-A706-A43C0137D4E9')");
        }
    }
}
