namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestAssignmentChangeLog : DbMigration
    {
        public override void Up()
        {
            /* Create the Request Assignment Change event */
            Sql("IF NOT EXISTS(SELECT NULL FROM [Events] WHERE ID = '45DA0001-7E63-4578-9A19-A43B0100F7C8') INSERT INTO [Events] (ID, Name, Description) VALUES ('45DA0001-7E63-4578-9A19-A43B0100F7C8', 'Request Assigned', 'Users will be notified whenever a user is assigned to a request.')");
            //organization location
            Sql("IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '45DA0001-7E63-4578-9A19-A43B0100F7C8' AND Location = 3) INSERT INTO EventLocations (EventID, Location) VALUES ('45DA0001-7E63-4578-9A19-A43B0100F7C8', 3)");
            //project location
            Sql("IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '45DA0001-7E63-4578-9A19-A43B0100F7C8' AND Location = 4) INSERT INTO EventLocations (EventID, Location) VALUES ('45DA0001-7E63-4578-9A19-A43B0100F7C8', 4)");
            //user location
            Sql("IF NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '45DA0001-7E63-4578-9A19-A43B0100F7C8' AND Location = 9) INSERT INTO EventLocations (EventID, Location) VALUES ('45DA0001-7E63-4578-9A19-A43B0100F7C8', 9)");

            CreateTable(
                "dbo.LogsRequestAssignmentChange",
                c => new
                {
                    UserID = c.Guid(nullable: false),
                    TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                    RequestID = c.Guid(nullable: false),
                    RequestUserUserID = c.Guid(nullable: false),
                    WorkflowRoleID = c.Guid(nullable: false),
                    Reason = c.Int(nullable: false),
                    EventID = c.Guid(nullable: false),
                    Description = c.String(),
                })
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.RequestUserUserID, cascadeDelete: true)
                .ForeignKey("dbo.WorkflowRoles", t => t.WorkflowRoleID, cascadeDelete: true)
                .Index(t => t.RequestID)
                .Index(t => t.UserID)
                .Index(t => t.WorkflowRoleID)
                .Index(t => t.EventID);

            Sql(@"ALTER TABLE dbo.LogsRequestAssignmentChange ADD CONSTRAINT PK_LogsRequestAssignmentChange PRIMARY KEY (UserID, TimeStamp, RequestID, RequestUserUserID, WorkflowRoleID) ON AuditLogs"); 
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestAssignmentChange", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.LogsRequestAssignmentChange", "RequestUserUserID", "dbo.Users");
            DropForeignKey("dbo.LogsRequestAssignmentChange", "WorkflowRoleID", "dbo.WorkflowRoles");
            DropIndex("dbo.LogsRequestAssignmentChange", new[] { "EventID" });
            DropIndex("dbo.LogsRequestAssignmentChange", new[] { "WorkflowRoleID" });
            DropIndex("dbo.LogsRequestAssignmentChange", new[] { "UserID" });
            DropIndex("dbo.LogsRequestAssignmentChange", new[] { "RequestID" });
            DropTable("dbo.LogsRequestAssignmentChange");

            Sql("DELETE FROM EventLocations WHERE EventID = '45DA0001-7E63-4578-9A19-A43B0100F7C8'");
            Sql("DELETE FROM [Events] WHERE ID = '45DA0001-7E63-4578-9A19-A43B0100F7C8'");
        }
    }
}
