namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDemographicByZCTA : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Demographics", new[] { "PostalCode" });
            CreateTable(
                "dbo.DemographicsByZCTA",
                c => new
                    {
                        Zip = c.String(nullable: false, maxLength: 5),
                        Sex = c.String(nullable: false, maxLength: 1),
                        AgeGroup = c.Int(nullable: false),
                        Ethnicity = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Zip, t.Sex, t.AgeGroup, t.Ethnicity });
            
            DropColumn("dbo.Demographics", "PostalCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Demographics", "PostalCode", c => c.String(maxLength: 10));
            DropTable("dbo.DemographicsByZCTA");
            CreateIndex("dbo.Demographics", "PostalCode");
        }
    }
}
