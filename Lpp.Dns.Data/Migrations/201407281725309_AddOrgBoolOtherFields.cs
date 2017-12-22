namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrgBoolOtherFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "OtherClaims", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "DataModelOther", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "DataModelOther");
            DropColumn("dbo.Organizations", "OtherClaims");
        }
    }
}
