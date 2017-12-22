namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSIDToActivities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "SID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
            CreateIndex("dbo.Activities", "SID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Activities", new string[] { "SID" });
            DropColumn("dbo.Activities", "SID");
        }
    }
}
