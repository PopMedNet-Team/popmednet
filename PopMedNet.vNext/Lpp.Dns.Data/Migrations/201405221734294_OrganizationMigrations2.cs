namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationMigrations2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Organizations", "ParentOrganizationID");
        }
        
        public override void Down()
        {
        }
    }
}
