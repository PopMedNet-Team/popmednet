namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToOrganizationRegistries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrganizationsRegistries", "Description", c => c.String(maxLength: 200));
            CreateIndex("dbo.OrganizationsRegistries", "Description");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrganizationsRegistries", new string[] { "Description" });
            DropColumn("dbo.OrganizationsRegistries", "Description");
        }
    }
}
