namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserChangeAuditLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserChangeLogs",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        UserChangedID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.UserID, t.TimeStamp })
                .ForeignKey("dbo.Users", t => t.UserChangedID, cascadeDelete: true)
                .Index(t => t.UserChangedID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserChangeLogs", "UserChangedID", "dbo.Users");
            DropIndex("dbo.UserChangeLogs", new[] { "UserChangedID" });
            DropTable("dbo.UserChangeLogs");
        }
    }
}
