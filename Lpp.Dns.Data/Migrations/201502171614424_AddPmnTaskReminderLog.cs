namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPmnTaskReminderLog : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM [Events] WHERE ID = '02930001-027B-439E-AE7C-A44200FA221C') INSERT INTO [Events] (ID, Name, Description) VALUES ('02930001-027B-439E-AE7C-A44200FA221C', 'Task Reminder', 'Users will be reminded of active workflow tasks they are assigned to.')");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '02930001-027B-439E-AE7C-A44200FA221C' AND Location = 3) INSERT INTO EventLocations (EventID, Location) VALUES ('02930001-027B-439E-AE7C-A44200FA221C', 3)");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '02930001-027B-439E-AE7C-A44200FA221C' AND Location = 4) INSERT INTO EventLocations (EventID, Location) VALUES ('02930001-027B-439E-AE7C-A44200FA221C', 4)");

            CreateTable(
                "dbo.LogsTaskReminder",
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

            Sql(@"ALTER TABLE dbo.LogsTaskReminder ADD CONSTRAINT PK_LogsTaskReminder PRIMARY KEY (UserID, TimeStamp, TaskID) ON AuditLogs");   
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsTaskReminder", "TaskID", "dbo.Tasks");
            DropIndex("dbo.LogsTaskReminder", new[] { "EventID" });
            DropIndex("dbo.LogsTaskReminder", new[] { "TaskID" });
            DropTable("dbo.LogsTaskReminder");

            Sql("DELETE FROM EventLocations WHERE EventID = '02930001-027B-439E-AE7C-A44200FA221C'");
            Sql("DELETE FROM Events WHERE ID = '02930001-027B-439E-AE7C-A44200FA221C')");
        }
    }
}
