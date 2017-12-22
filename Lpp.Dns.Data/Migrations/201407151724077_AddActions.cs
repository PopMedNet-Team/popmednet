namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionReferences",
                c => new
                    {
                        ActionID = c.Guid(nullable: false),
                        ItemID = c.Guid(nullable: false),
                        Item = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActionID, t.ItemID })
                .ForeignKey("dbo.Actions", t => t.ActionID, cascadeDelete: true)
                .Index(t => t.ActionID);
            
            CreateTable(
                "dbo.Actions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 255),
                        Location = c.String(maxLength: 255),
                        Body = c.String(),
                        DueDate = c.DateTime(),
                        CreatedOn = c.DateTime(nullable: false),
                        StartOn = c.DateTime(),
                        EndOn = c.DateTime(),
                        EstimatedCompletedOn = c.DateTime(),
                        Priority = c.Byte(nullable: false),
                        Status = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        PercentComplete = c.Double(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ActionUsers",
                c => new
                    {
                        ActionID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActionID, t.UserID })
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Actions", t => t.ActionID, cascadeDelete: true)
                .Index(t => t.ActionID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActionUsers", "ActionID", "dbo.Actions");
            DropForeignKey("dbo.ActionUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.ActionReferences", "ActionID", "dbo.Actions");
            DropIndex("dbo.ActionUsers", new[] { "UserID" });
            DropIndex("dbo.ActionUsers", new[] { "ActionID" });
            DropIndex("dbo.ActionReferences", new[] { "ActionID" });
            DropTable("dbo.ActionUsers");
            DropTable("dbo.Actions");
            DropTable("dbo.ActionReferences");
        }
    }
}
