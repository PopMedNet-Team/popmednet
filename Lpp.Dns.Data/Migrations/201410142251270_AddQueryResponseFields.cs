namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQueryResponseFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Query", c => c.String());
            AddColumn("dbo.RequestDataMartResponses", "ResponseData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestDataMartResponses", "ResponseData");
            DropColumn("dbo.Requests", "Query");
        }
    }
}
