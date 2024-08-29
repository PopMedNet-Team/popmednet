namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostalCodeToDemographics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demographics", "PostalCode", c => c.String(maxLength: 10, nullable: true));
            CreateIndex("dbo.Demographics", "PostalCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Demographics", new[] { "PostalCode" });
            DropColumn("dbo.Demographics", "PostalCode");
        }
    }
}
