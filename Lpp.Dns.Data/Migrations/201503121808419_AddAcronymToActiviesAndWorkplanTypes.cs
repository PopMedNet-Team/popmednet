namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAcronymToActiviesAndWorkplanTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Acronym", c => c.String(maxLength: 50));
            AddColumn("dbo.WorkplanTypes", "Acronym", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkplanTypes", "Acronym");
            DropColumn("dbo.Activities", "Acronym");
        }
    }
}
