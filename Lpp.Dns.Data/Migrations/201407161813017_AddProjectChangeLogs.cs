namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectChangeLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectChangeLogs",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        ProjectID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        Description = c.String(),
                    })                
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);

            Sql(@"ALTER TABLE dbo.ProjectChangeLogs ADD CONSTRAINT PK_ProjectChangeLogs PRIMARY KEY (UserID, TimeStamp, ProjectID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectChangeLogs", "ProjectID", "dbo.Projects");
            DropIndex("dbo.ProjectChangeLogs", new[] { "ProjectID" });
            DropTable("dbo.ProjectChangeLogs");
        }
    }
}
