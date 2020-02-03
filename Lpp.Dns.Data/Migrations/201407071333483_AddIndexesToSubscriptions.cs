namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexesToSubscriptions : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserEventSubscriptions", "LastRunTime");
            CreateIndex("dbo.UserEventSubscriptions", "NextDueTime");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserEventSubscriptions", new[] { "NextDueTime" });
            DropIndex("dbo.UserEventSubscriptions", new[] { "LastRunTime" });
        }
    }
}
