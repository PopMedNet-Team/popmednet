namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletedOnAndUserIdentifierToRequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "CompletedOn", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Requests", "UserIdentifier", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "UserIdentifier");
            DropColumn("dbo.Requests", "CompletedOn");
        }
    }
}
