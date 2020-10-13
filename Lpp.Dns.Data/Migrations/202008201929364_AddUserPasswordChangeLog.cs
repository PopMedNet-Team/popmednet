namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPasswordChangeLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsUserPasswordChange",
                c => new
                {
                    UserID = c.Guid(nullable: false),
                    TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                    UserChangedID = c.Guid(nullable: false),
                    OriginalPassword = c.String(maxLength: 100),
                    Method = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.UserID, t.TimeStamp, t.UserChangedID })
                .ForeignKey("dbo.Users", t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserChangedID)
                .Index(t => t.UserID)
                .Index(t => new { t.TimeStamp, t.UserChangedID, t.OriginalPassword });

            Sql(@"
                if exists (select 1 from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'UserPasswordTrace')
	                DROP TABLE UserPasswordTrace
             ");

            Sql("INSERT INTO Permissions(ID, Name, Description) VALUES('AB53BECB-4ADE-44E8-995F-15511C4D0D19', 'Expire All Passwords', 'Grants the user the ability to expire the passwords for all active users of an Organization or Network via a single command.')");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('AB53BECB-4ADE-44E8-995F-15511C4D0D19', 0)");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('AB53BECB-4ADE-44E8-995F-15511C4D0D19', 3)");
        }

        public override void Down()
        {
            Sql("DELETE FROM PermissionLocations WHERE PermissionID = 'AB53BECB-4ADE-44E8-995F-15511C4D0D19'");
            Sql("DELETE FROM Permissions WHERE ID = 'AB53BECB-4ADE-44E8-995F-15511C4D0D19'");
            DropForeignKey("dbo.LogsUserPasswordChange", "UserChangedID", "dbo.Users");
            DropForeignKey("dbo.LogsUserPasswordChange", "UserID", "dbo.Users");
            DropIndex("dbo.LogsUserPasswordChange", new[] { "TimeStamp", "UserChangedID", "OriginalPassword" });
            DropIndex("dbo.LogsUserPasswordChange", new[] { "UserID" });
            DropTable("dbo.LogsUserPasswordChange");
        }
    }
}
