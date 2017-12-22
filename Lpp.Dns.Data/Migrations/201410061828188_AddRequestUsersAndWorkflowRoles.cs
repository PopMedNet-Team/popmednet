namespace Lpp.Dns.Data.Migrations
{
    using Lpp.Utilities;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestUsersAndWorkflowRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkflowRoles",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        WorkflowID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Workflows", t => t.WorkflowID, cascadeDelete: true)
                .Index(t => t.WorkflowID);
            
            CreateTable(
                "dbo.RequestUsers",
                c => new
                    {
                        RequestID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        WorkflowRoleID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestID, t.UserID, t.WorkflowRoleID })
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.WorkflowRoles", t => t.WorkflowRoleID, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.RequestID)
                .Index(t => t.UserID)
                .Index(t => t.WorkflowRoleID);

            Sql("INSERT INTO WorkflowRoles (ID, WorkflowID, Name, Description) VALUES ('B96BD897-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Requestor', 'The requestor of the workflow')");
            Sql("INSERT INTO WorkflowRoles (ID, WorkflowID, Name, Description) VALUES ('B96BD807-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Primary Analyst', 'The primary analyst of the workflow')");
            Sql("INSERT INTO WorkflowRoles (ID, WorkflowID, Name, Description) VALUES ('B96BD817-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Secondary Analyst', 'The secondary analyst of the workflow')");
            Sql("INSERT INTO WorkflowRoles (ID, WorkflowID, Name, Description) VALUES ('B96BD827-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Epidemiologist', 'The Epidemiologist of the workflow')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestUsers", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.WorkflowRoles", "WorkflowID", "dbo.Workflows");
            DropForeignKey("dbo.RequestUsers", "WorkflowRoleID", "dbo.WorkflowRoles");
            DropForeignKey("dbo.RequestUsers", "UserID", "dbo.Users");
            DropIndex("dbo.RequestUsers", new[] { "WorkflowRoleID" });
            DropIndex("dbo.RequestUsers", new[] { "UserID" });
            DropIndex("dbo.RequestUsers", new[] { "RequestID" });
            DropIndex("dbo.WorkflowRoles", new[] { "WorkflowID" });
            DropTable("dbo.RequestUsers");
            DropTable("dbo.WorkflowRoles");
        }
    }
}
