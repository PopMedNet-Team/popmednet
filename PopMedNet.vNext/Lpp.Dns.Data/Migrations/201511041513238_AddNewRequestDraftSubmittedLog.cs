namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewRequestDraftSubmittedLog : DbMigration
    {
        public override void Up()
        {

            Sql(@"IF NOT EXISTS(SELECT NULL FROM [Events] WHERE ID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869') INSERT INTO [Events] (ID, Name, Description) VALUES ('6549439E-E3E4-4F4C-92CF-88FB81FF8869', 'New Request Draft Submitted', 'Users will be notified when a new request draft is submitted that they have permission to see.')");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869' AND Location = 3) INSERT INTO EventLocations (EventID, Location) VALUES ('6549439E-E3E4-4F4C-92CF-88FB81FF8869', 3)");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869' AND Location = 4) INSERT INTO EventLocations (EventID, Location) VALUES ('6549439E-E3E4-4F4C-92CF-88FB81FF8869', 4)");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869' AND Location = 11) INSERT INTO EventLocations (EventID, Location) VALUES ('6549439E-E3E4-4F4C-92CF-88FB81FF8869', 11)");

            CreateTable(
                "dbo.LogsNewRequestDraftSubmitted",
                c => new
                {
                    UserID = c.Guid(nullable: false),
                    TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                    TaskID = c.Guid(nullable: false),
                    EventID = c.Guid(nullable: false),
                    Description = c.String(),
                })
                .ForeignKey("dbo.Tasks", t => t.TaskID, cascadeDelete: true)
                .Index(t => t.TaskID)
                .Index(t => t.EventID);

            Sql(@"ALTER TABLE dbo.LogsNewRequestDraftSubmitted ADD CONSTRAINT PK_LogsNewRequestDraftSubmitted PRIMARY KEY (UserID, TimeStamp, TaskID) ON AuditLogs"); 
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsNewRequestDraftSubmitted", "TaskID", "dbo.Tasks");
            DropIndex("dbo.LogsNewRequestDraftSubmitted", new[] { "EventID" });
            DropIndex("dbo.LogsNewRequestDraftSubmitted", new[] { "TaskID" });
            DropTable("dbo.LogsNewRequestDraftSubmitted");

            Sql("DELETE FROM EventLocations WHERE EventID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869'");
            Sql("DELETE FROM Events WHERE ID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869')");

        }
    }
}
