namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMyNotifications2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserEventSubscriptions", "Frequency", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserEventSubscriptions", "Frequency", c => c.Int(nullable: false));
        }
    }
}
