namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixLookupListValue : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.LookupListVlaues", newName: "LookupListValues");
        }
        
        public override void Down()
        {
            //RenameTable(name: "dbo.LookupListValues", newName: "LookupListVlaues");
        }
    }
}
