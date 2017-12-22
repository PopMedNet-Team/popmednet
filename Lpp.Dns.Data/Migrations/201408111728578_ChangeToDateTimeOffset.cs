namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeToDateTimeOffset : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserEventSubscriptions", new[] { "LastRunTime" });
            DropIndex("dbo.UserEventSubscriptions", new[] { "NextDueTime" });
            AlterColumn("dbo.UserEventSubscriptions", "LastRunTime", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.UserEventSubscriptions", "NextDueTime", c => c.DateTimeOffset(precision: 7));
            CreateIndex("dbo.UserEventSubscriptions", "LastRunTime");
            CreateIndex("dbo.UserEventSubscriptions", "NextDueTime");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserEventSubscriptions", new[] { "NextDueTime" });
            DropIndex("dbo.UserEventSubscriptions", new[] { "LastRunTime" });
            AlterColumn("dbo.UserEventSubscriptions", "NextDueTime", c => c.DateTime());
            AlterColumn("dbo.UserEventSubscriptions", "LastRunTime", c => c.DateTime());
            CreateIndex("dbo.UserEventSubscriptions", "NextDueTime");
            CreateIndex("dbo.UserEventSubscriptions", "LastRunTime");
        }
    }
}
