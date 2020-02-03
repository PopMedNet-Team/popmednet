namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkflowUpdates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkflowActivitySecurityGroups",
                c => new
                    {
                        WorkflowActivityID = c.Guid(nullable: false),
                        SecurityGroupID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.WorkflowActivityID, t.SecurityGroupID })
                .ForeignKey("dbo.WorkflowActivities", t => t.WorkflowActivityID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.WorkflowActivityID)
                .Index(t => t.SecurityGroupID);
            
            AddColumn("dbo.WorkflowActivities", "Start", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.WorkflowActivities", "End", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkflowActivitySecurityGroups", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.WorkflowActivitySecurityGroups", "WorkflowActivityID", "dbo.WorkflowActivities");
            DropIndex("dbo.WorkflowActivitySecurityGroups", new[] { "SecurityGroupID" });
            DropIndex("dbo.WorkflowActivitySecurityGroups", new[] { "WorkflowActivityID" });
            DropColumn("dbo.WorkflowActivities", "End");
            DropColumn("dbo.WorkflowActivities", "Start");
            DropTable("dbo.WorkflowActivitySecurityGroups");
        }
    }
}
