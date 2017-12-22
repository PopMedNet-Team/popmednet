namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSelfReferencesToActivities : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Activities", "ParentID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Activities", new[] { "ParentID" });
        }
    }
}
