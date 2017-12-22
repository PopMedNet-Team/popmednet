namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNewRequestDraftSubmittedLogsTable3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogsNewRequestDraftSubmitted", "TaskID", "dbo.Tasks");
            DropIndex("dbo.LogsNewRequestDraftSubmitted", new[] { "TaskID" });
            DropColumn("dbo.LogsNewRequestDraftSubmitted", "TaskID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LogsNewRequestDraftSubmitted", "TaskID", c => c.Guid());
            CreateIndex("dbo.LogsNewRequestDraftSubmitted", "TaskID");
            AddForeignKey("dbo.LogsNewRequestDraftSubmitted", "TaskID", "dbo.Tasks", "ID");
        }
    }
}
