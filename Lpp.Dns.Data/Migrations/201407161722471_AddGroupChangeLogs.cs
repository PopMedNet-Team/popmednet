namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupChangeLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupChangeLogs",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        GroupID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Groups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
            Sql(@"ALTER TABLE dbo.GroupChangeLogs ADD CONSTRAINT PK_GroupChangeLogs PRIMARY KEY (UserID, TimeStamp, GroupID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupChangeLogs", "GroupID", "dbo.Groups");
            DropIndex("dbo.GroupChangeLogs", new[] { "GroupID" });
            DropTable("dbo.GroupChangeLogs");
        }
    }
}
