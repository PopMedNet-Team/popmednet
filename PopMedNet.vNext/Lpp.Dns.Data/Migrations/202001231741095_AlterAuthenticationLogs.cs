namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAuthenticationLogs : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.LogsUserAuthentication", "Enviorment", "Environment");
            AlterColumn("dbo.LogsUserAuthentication", "Environment", c => c.String(maxLength: 50));
            AddColumn("dbo.LogsUserAuthentication", "Source", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            RenameColumn("dbo.LogsUserAuthentication", "Environment", "Enviorment");
            AlterColumn("dbo.LogsUserAuthentication", "Enviorment", c => c.String(maxLength: 10));
            DropColumn("dbo.LogsUserAuthentication", "Source");
        }
    }
}
