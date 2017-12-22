namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataMartVitalSignsLength : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "VitalSignsLength", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMarts", "VitalSignsLength");
        }
    }
}
