namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEHRSDataTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationElectronicHealthRecordSystems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrganizationID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        System = c.Int(nullable: false),
                        StartYear = c.Int(),
                        EndYear = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .Index(t => t.OrganizationID)
                .Index(t => t.Type)
                .Index(t=> t.System)
                .Index(t => t.StartYear)
                .Index(t => t.EndYear);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationElectronicHealthRecordSystems", "OrganizationID", "dbo.Organizations");
            DropIndex("dbo.OrganizationElectronicHealthRecordSystems", new[] { "OrganizationID" });
            DropIndex("dbo.OrganizationElectronicHealthRecordSystems", new[] { "Type" });
            DropIndex("dbo.OrganizationElectronicHealthRecordSystems", new[] { "System" });
            DropIndex("dbo.OrganizationElectronicHealthRecordSystems", new[] { "StartYear" });
            DropIndex("dbo.OrganizationElectronicHealthRecordSystems", new[] { "EndYear" });
            DropTable("dbo.OrganizationElectronicHealthRecordSystems");
        }
    }
}
