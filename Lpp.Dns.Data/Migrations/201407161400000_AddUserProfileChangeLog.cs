namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProfileChangeLog : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DataMartChangeLogs", newName: "LogsDataMartChange");
            RenameTable(name: "dbo.UserChangeLogs", newName: "LogsUserChange");
            CreateTable(
                "dbo.LogsProfileUpdated",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        UserChangedID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        Description = c.String(),
                    })                
                .ForeignKey("dbo.Users", t => t.UserChangedID, cascadeDelete: true)
                .Index(t => t.UserChangedID);

            Sql(@"ALTER TABLE dbo.LogsProfileUpdated ADD CONSTRAINT PK_LogsProfileUpdated PRIMARY KEY (UserID, TimeStamp, UserChangedID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsProfileUpdated", "UserChangedID", "dbo.Users");
            DropIndex("dbo.LogsProfileUpdated", new[] { "UserChangedID" });
            DropTable("dbo.LogsProfileUpdated");
            RenameTable(name: "dbo.LogsUserChange", newName: "UserChangeLogs");
            RenameTable(name: "dbo.LogsDataMartChange", newName: "DataMartChangeLogs");
        }
    }
}
