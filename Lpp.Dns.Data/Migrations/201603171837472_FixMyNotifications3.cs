namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMyNotifications3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserEventSubscriptions", "NextDueTimeForMy", c => c.DateTimeOffset(precision: 7));
            CreateIndex("dbo.UserEventSubscriptions", "NextDueTimeForMy");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserEventSubscriptions", new[] { "NextDueTimeForMy" });
            DropColumn("dbo.UserEventSubscriptions", "NextDueTimeForMy");
        }
    }
}
