namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOMOPToOrganization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "DataModelOMOP", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "DataModelOMOP");
        }
    }
}
