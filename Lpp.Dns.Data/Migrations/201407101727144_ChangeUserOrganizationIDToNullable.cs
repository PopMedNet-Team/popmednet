namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserOrganizationIDToNullable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "OrganizationRequested", c => c.String(maxLength: 255));
            AlterColumn("dbo.Users", "OrganizationID", c => c.Guid(true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "OrganizationRequested");
        }
    }
}
