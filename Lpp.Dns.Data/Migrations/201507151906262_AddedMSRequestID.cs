namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMSRequestID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "MSRequestID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "MSRequestID");
        }
    }
}
