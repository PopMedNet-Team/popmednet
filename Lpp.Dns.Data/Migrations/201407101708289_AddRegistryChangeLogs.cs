namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegistryChangeLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistryChangeLogs",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RegistryID = c.Guid(nullable: false),
                    })                
                .ForeignKey("dbo.Registries", t => t.RegistryID, cascadeDelete: true);

            //This forces the table onto the right file group
            Sql(@"ALTER TABLE dbo.RegistryChangeLogs ADD CONSTRAINT PK_RegistryChangeLogs PRIMARY KEY (UserID, TimeStamp, RegistryID) ON AuditLogs");
            Sql(@"CREATE INDEX IX_RegistryID ON dbo.RegistryChangeLogs (RegistryID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegistryChangeLogs", "RegistryID", "dbo.Registries");
            DropIndex("dbo.RegistryChangeLogs", new[] { "RegistryID" });
            DropTable("dbo.RegistryChangeLogs");
        }
    }
}
