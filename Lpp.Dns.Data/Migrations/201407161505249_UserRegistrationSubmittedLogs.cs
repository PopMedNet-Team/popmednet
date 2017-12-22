namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRegistrationSubmittedLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsUserRegistrationSubmitted",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RegisteredUserID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.Users", t => t.RegisteredUserID, cascadeDelete: true)
                .Index(t => t.RegisteredUserID);

            Sql(@"ALTER TABLE dbo.LogsUserRegistrationSubmitted ADD CONSTRAINT PK_LogsUserRegistrationSubmitted PRIMARY KEY (UserID, TimeStamp, RegisteredUserID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsUserRegistrationSubmitted", "RegisteredUserID", "dbo.Users");
            DropIndex("dbo.LogsUserRegistrationSubmitted", new[] { "RegisteredUserID" });
            DropTable("dbo.LogsUserRegistrationSubmitted");
        }
    }
}
