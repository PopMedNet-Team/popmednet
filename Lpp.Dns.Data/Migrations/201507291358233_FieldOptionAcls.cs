namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldOptionAcls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclGlobalFieldOptions",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        FieldIdentifier = c.String(nullable: false, maxLength: 80),
                        Permission = c.Int(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.FieldIdentifier, t.Permission })
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID);
            
            CreateTable(
                "dbo.AclProjectFieldOptions",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        FieldIdentifier = c.String(nullable: false, maxLength: 80),
                        Permission = c.Int(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.FieldIdentifier, t.Permission, t.ProjectID })
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclProjectFieldOptions", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectFieldOptions", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclGlobalFieldOptions", "SecurityGroupID", "dbo.SecurityGroups");
            DropIndex("dbo.AclProjectFieldOptions", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectFieldOptions", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclGlobalFieldOptions", new[] { "SecurityGroupID" });
            DropTable("dbo.AclProjectFieldOptions");
            DropTable("dbo.AclGlobalFieldOptions");
        }
    }
}
