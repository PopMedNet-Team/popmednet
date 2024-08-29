namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPackageIdentifierToRequestTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestTypes", "PackageIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestTypes", "PackageIdentifier");
        }
    }
}
