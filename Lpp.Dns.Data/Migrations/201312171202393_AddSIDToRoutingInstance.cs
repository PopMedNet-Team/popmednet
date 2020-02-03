namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSIDToRoutingInstance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestRoutingInstances", "SID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestRoutingInstances", "SID");
        }
    }
}
