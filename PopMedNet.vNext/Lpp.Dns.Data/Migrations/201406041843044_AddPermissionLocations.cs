namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissionLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PermissionLocations",
                c => new
                    {
                        PermissionID = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PermissionID, t.Type })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .Index(t => t.PermissionID);

            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) SELECT ID, Type FROM Permissions");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermissionLocations", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.PermissionLocations", new[] { "PermissionID" });
            DropTable("dbo.PermissionLocations");
        }
    }
}
