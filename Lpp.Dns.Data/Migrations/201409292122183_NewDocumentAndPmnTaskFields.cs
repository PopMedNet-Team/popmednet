namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDocumentAndPmnTaskFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "WorkflowActivityID", c => c.Guid(nullable:true));
            AddColumn("dbo.Documents", "RevisionSetID", c => c.Guid(nullable:true));
            CreateIndex("dbo.Tasks", "WorkflowActivityID");
            AddForeignKey("dbo.Tasks", "WorkflowActivityID", "dbo.WorkflowActivities", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "WorkflowActivityID", "dbo.WorkflowActivities");
            DropIndex("dbo.Tasks", new[] { "WorkflowActivityID" });
            DropColumn("dbo.Documents", "RevisionSetID");
            DropColumn("dbo.Tasks", "WorkflowActivityID");
        }
    }
}
