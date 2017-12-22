namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRegistrationStatusChanged : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsUserRegistrationChanged",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RegisteredUserID = c.Guid(nullable: false),
                        Description = c.String(),
                    })                
                .ForeignKey("dbo.Users", t => t.RegisteredUserID, cascadeDelete: true)
                .Index(t => t.RegisteredUserID);

            Sql(@"ALTER TABLE dbo.LogsUserRegistrationChanged ADD CONSTRAINT PK_LogsUserRegistrationChanged PRIMARY KEY (UserID, TimeStamp, RegisteredUserID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsUserRegistrationChanged", "RegisteredUserID", "dbo.Users");
            DropIndex("dbo.LogsUserRegistrationChanged", new[] { "RegisteredUserID" });
            DropTable("dbo.LogsUserRegistrationChanged");
        }
    }
}
