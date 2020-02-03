namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDemographicTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Demographics",
                c => new
                    {
                        Country = c.String(nullable: false, maxLength: 2),
                        State = c.String(nullable: false, maxLength: 2),
                        Town = c.String(nullable: false, maxLength: 50),
                        Region = c.String(nullable: false, maxLength: 50),
                        Gender = c.String(nullable: false, maxLength: 1),
                        AgeGroup = c.Int(nullable: false),
                        Ethnicity = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Country, t.State, t.Town, t.Region, t.Gender, t.AgeGroup, t.Ethnicity });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Demographics");
        }
    }
}
