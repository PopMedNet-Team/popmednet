namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAuthenticationTable : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM EVENTS WHERE ID = '7A1E80BC-7F0B-4B87-8320-DCF00B185C84' AND NAME = 'Authentication')
INSERT INTO EVENTS (ID, NAME, DESCRIPTION) VALUES ('7A1E80BC-7F0B-4B87-8320-DCF00B185C84', 'Authentication', 'Logs the Authentication Details.')");
            CreateTable(
                "dbo.LogsUserAuthentication",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        ID = c.Guid(nullable: false),
                        Success = c.Boolean(nullable: false),
                        IPAddress = c.String(maxLength: 40),
                        Enviorment = c.String(maxLength: 10),
                        Details = c.String(maxLength: 500),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .Index(t => t.EventID);
            Sql(@"ALTER TABLE dbo.LogsUserAuthentication ADD CONSTRAINT PK_LogsUserAuthentication PRIMARY KEY (UserID, TimeStamp, ID) ON AuditLogs");

        }
        
        public override void Down()
        {
            Sql(@"IF EXISTS(SELECT NULL FROM EVENTS WHERE ID = '7A1E80BC-7F0B-4B87-8320-DCF00B185C84' AND NAME = 'Authentication')
DELETE FROM [Events] WHERE ID = '7A1E80BC-7F0B-4B87-8320-DCF00B185C84'");
            DropIndex("dbo.LogsUserAuthentication", new[] { "EventID" });
            DropTable("dbo.LogsUserAuthentication");
        }
    }
}
