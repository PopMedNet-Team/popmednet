namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixOrganizationDescriptiontonvarcharmax : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Organizations", new string[] { "OrganizationDescription" });
            AlterColumn("dbo.Organizations", "OrganizationDescription", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Organizations", "OrganizationDescription", c => c.String(maxLength: 455));
        }
    }
}
