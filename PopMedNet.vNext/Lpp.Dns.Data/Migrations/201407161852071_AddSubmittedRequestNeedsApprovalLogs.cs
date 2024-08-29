namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmittedRequestNeedsApprovalLogs : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProjectChangeLogs", newName: "LogsProjectChange");
            RenameTable(name: "dbo.GroupChangeLogs", newName: "LogsGroupChange");
            RenameTable(name: "dbo.OrganizationChangedLogs", newName: "LogsOrganizationChange");
            CreateTable(
                "dbo.LogsSubmittedRequestNeedsApproval",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID);

            Sql(@"ALTER TABLE dbo.LogsSubmittedRequestNeedsApproval ADD CONSTRAINT PK_LogsSubmittedRequestNeedsApproval PRIMARY KEY (UserID, TimeStamp, RequestID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsSubmittedRequestNeedsApproval", "RequestID", "dbo.Requests");
            DropIndex("dbo.LogsSubmittedRequestNeedsApproval", new[] { "RequestID" });
            DropTable("dbo.LogsSubmittedRequestNeedsApproval");
            RenameTable(name: "dbo.LogsOrganizationChange", newName: "OrganizationChangedLogs");
            RenameTable(name: "dbo.LogsGroupChange", newName: "GroupChangeLogs");
            RenameTable(name: "dbo.LogsProjectChange", newName: "ProjectChangeLogs");
        }
    }
}
