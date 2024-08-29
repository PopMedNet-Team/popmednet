namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestSearchTermClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestSearchTerms", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion")); 
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestSearchTerms", "Timestamp");
        }
    }
}
