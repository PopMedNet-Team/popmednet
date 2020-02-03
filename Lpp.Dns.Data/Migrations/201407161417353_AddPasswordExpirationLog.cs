namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPasswordExpirationLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsPasswordExpiration",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        ExpiringUserID = c.Guid(nullable: false),
                        Description = c.String(),
                    })                
                .ForeignKey("dbo.Users", t => t.ExpiringUserID, cascadeDelete: true)
                .Index(t => t.ExpiringUserID);

            Sql(@"ALTER TABLE dbo.LogsPasswordExpiration ADD CONSTRAINT PK_LogsPasswordExpiration PRIMARY KEY (UserID, TimeStamp, ExpiringUserID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsPasswordExpiration", "ExpiringUserID", "dbo.Users");
            DropIndex("dbo.LogsPasswordExpiration", new[] { "ExpiringUserID" });
            DropTable("dbo.LogsPasswordExpiration");
        }
    }
}
