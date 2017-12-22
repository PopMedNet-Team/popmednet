namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRequestStatusChangeLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsRequestStatusChange", "EmailBody", c => c.String());
            AddColumn("dbo.LogsRequestStatusChange", "MyEmailBody", c => c.String());
            AddColumn("dbo.LogsRequestStatusChange", "Subject", c => c.String());
            AddColumn("dbo.LogsRequestStatusChange", "Reason", c => c.Int(nullable: false));
            DropTable("LogsRequestWorkflowActivityChange");
        }

        public override void Down()
        {
            DropColumn("dbo.LogsRequestStatusChange", "Reason");
            DropColumn("dbo.LogsRequestStatusChange", "Subject");
            DropColumn("dbo.LogsRequestStatusChange", "MyEmailBody");
            DropColumn("dbo.LogsRequestStatusChange", "EmailBody");
        }
    }
}
