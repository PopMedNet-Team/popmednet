namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganizationChangeLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationChangedLogs",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        OrganizationID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        Description = c.String(),
                    })                
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .Index(t => t.OrganizationID);

            Sql(@"ALTER TABLE dbo.OrganizationChangedLogs ADD CONSTRAINT PK_OrganizationChangedLogs PRIMARY KEY (UserID, TimeStamp, OrganizationID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationChangedLogs", "OrganizationID", "dbo.Organizations");
            DropIndex("dbo.OrganizationChangedLogs", new[] { "OrganizationID" });
            DropTable("dbo.OrganizationChangedLogs");
        }
    }
}
