namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToRequestWorkflowActivityChangeLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsRequestWorkflowActivityChange", "EmailBody", c => c.String());
            AddColumn("dbo.LogsRequestWorkflowActivityChange", "MyEmailBody", c => c.String());
            AddColumn("dbo.LogsRequestWorkflowActivityChange", "Subject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogsRequestWorkflowActivityChange", "Subject");
            DropColumn("dbo.LogsRequestWorkflowActivityChange", "MyEmailBody");
            DropColumn("dbo.LogsRequestWorkflowActivityChange", "EmailBody");
        }
    }
}
