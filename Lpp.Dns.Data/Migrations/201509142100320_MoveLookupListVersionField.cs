namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveLookupListVersionField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LookupLists", "Version", c => c.String(maxLength: 200));
            DropColumn("dbo.LookupListValues", "Version");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LookupListValues", "Version", c => c.String(maxLength: 200));
            DropColumn("dbo.LookupLists", "Version");
        }
    }
}
