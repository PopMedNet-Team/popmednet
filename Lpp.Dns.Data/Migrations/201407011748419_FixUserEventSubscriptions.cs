namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserEventSubscriptions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subscriptions", "UserID", "dbo.Users");
            DropIndex("dbo.Subscriptions", new[] { "UserID" });
            CreateTable(
                "dbo.UserEventSubscriptions",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        LastRunTime = c.DateTime(),
                        NextDueTime = c.DateTime(),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.EventID })
                .ForeignKey("dbo.Events", t => t.EventID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.EventID);
            

            //Copy the data from the old table to the new one
            Sql(@"INSERT INTO UserEventSubscriptions (UserID, EventID, LastRunTime, NextDueTime, Frequency) SELECT UserID, CAST(SUBSTRING(FiltersDefinitionXml, 18, 36) AS uniqueidentifier) AS EventID, LastRunTime, NextDueTime, Schedule  FROM Subscriptions ");

            DropTable("dbo.Subscriptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        LastRunTime = c.DateTime(),
                        NextDueTime = c.DateTime(),
                        FiltersDefinitionXml = c.String(),
                        Schedule = c.Int(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.UserEventSubscriptions", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserEventSubscriptions", "EventID", "dbo.Events");
            DropIndex("dbo.UserEventSubscriptions", new[] { "EventID" });
            DropIndex("dbo.UserEventSubscriptions", new[] { "UserID" });
            DropTable("dbo.UserEventSubscriptions");
            CreateIndex("dbo.Subscriptions", "UserID");
            AddForeignKey("dbo.Subscriptions", "UserID", "dbo.Users", "ID", cascadeDelete: true);
        }
    }
}
