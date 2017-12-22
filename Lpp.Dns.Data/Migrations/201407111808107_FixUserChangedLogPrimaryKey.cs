namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserChangedLogPrimaryKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserChangeLogs");

            Sql(@"ALTER TABLE dbo.UserChangeLogs ADD CONSTRAINT PK_UserChangeLogs PRIMARY KEY (UserID, TimeStamp, UserChangedID) ON AuditLogs");
        }
        
        public override void Down()
        {
        }
    }
}
