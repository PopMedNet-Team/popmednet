namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDMCVersionToAuditLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsUserAuthentication", "DMCVersion", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogsUserAuthentication", "DMCVersion");
        }
    }
}
