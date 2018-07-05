namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePropertiesFromRequestDataMart : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RequestDataMarts", "RequestTime");
            DropColumn("dbo.RequestDataMarts", "ResponseTime");
            DropColumn("dbo.RequestDataMarts", "RespondedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestDataMarts", "ResponseTime", c => c.DateTime());
            AddColumn("dbo.RequestDataMarts", "RequestTime", c => c.DateTime());
            AddColumn("dbo.RequestDataMarts", "RespondedBy", c => c.Int(nullable:true));
        }
    }
}
