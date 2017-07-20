namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExternalKeyToActivities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "ExternalKey", c => c.Int(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "ExternalKey");
        }
    }
}
