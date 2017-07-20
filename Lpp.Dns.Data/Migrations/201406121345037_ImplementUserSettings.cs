namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImplementUserSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 128),
                        Setting = c.String(),
                    })
                .PrimaryKey(t => new { t.UserID, t.Key })
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            DropColumn("dbo.Users", "ClientSettingsXml");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ClientSettingsXml", c => c.String());
            DropForeignKey("dbo.UserSettings", "UserID", "dbo.Users");
            DropIndex("dbo.UserSettings", new[] { "UserID" });
            DropTable("dbo.UserSettings");
        }
    }
}
