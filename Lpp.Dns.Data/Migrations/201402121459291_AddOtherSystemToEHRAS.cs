namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtherSystemToEHRAS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrganizationElectronicHealthRecordSystems", "Other", c => c.String(maxLength: 80));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrganizationElectronicHealthRecordSystems", "Other");
        }
    }
}
