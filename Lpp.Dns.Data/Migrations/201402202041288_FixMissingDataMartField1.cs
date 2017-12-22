namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMissingDataMartField1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "DemographicsOther", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMarts", "DemographicsOther");
        }
    }
}
