namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPCORIDataModelToOrganization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "DataModelPCORI", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "DataModelPCORI");
        }
    }
}
