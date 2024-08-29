namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectRequestTypeWorkflowActivityAcls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclProjectRequestTypeWorkflowActivities",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        RequestTypeID = c.Guid(nullable: false),
                        WorkflowActivityID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.RequestTypeID, t.WorkflowActivityID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.WorkflowActivities", t => t.WorkflowActivityID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.RequestTypeID)
                .Index(t => t.WorkflowActivityID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclProjectRequestTypeWorkflowActivities", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectRequestTypeWorkflowActivities", "WorkflowActivityID", "dbo.WorkflowActivities");
            DropForeignKey("dbo.AclProjectRequestTypeWorkflowActivities", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectRequestTypeWorkflowActivities", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.AclProjectRequestTypeWorkflowActivities", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.AclProjectRequestTypeWorkflowActivities", new[] { "WorkflowActivityID" });
            DropIndex("dbo.AclProjectRequestTypeWorkflowActivities", new[] { "RequestTypeID" });
            DropIndex("dbo.AclProjectRequestTypeWorkflowActivities", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectRequestTypeWorkflowActivities", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectRequestTypeWorkflowActivities", new[] { "SecurityGroupID" });
            DropTable("dbo.AclProjectRequestTypeWorkflowActivities");
        }
    }
}
