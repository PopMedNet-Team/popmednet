namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdapterPackageVersionToRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "AdapterPackageVersion", c => c.String(maxLength: 20));
            //set to the current version for all existing requests that have a status greater than Draft.
            //version gets set when the request is submitted, so any draft will get updated upon submit.
            Sql("UPDATE dbo.Requests SET AdapterPackageVersion = '4.0.3.0' WHERE CancelledOn IS NULL AND IsDeleted = 0 AND Status > 200");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "AdapterPackageVersion");
        }
    }
}
