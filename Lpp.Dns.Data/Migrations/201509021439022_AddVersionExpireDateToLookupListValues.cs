namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVersionExpireDateToLookupListValues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LookupListValues", "ExpireDate", c => c.DateTime());
            AddColumn("dbo.LookupListValues", "Version", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LookupListValues", "Version");
            DropColumn("dbo.LookupListValues", "ExpireDate");
        }
    }
}
