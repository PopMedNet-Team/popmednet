namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Terms1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataModelSupportedTerms",
                c => new
                    {
                        DataModelID = c.Guid(nullable: false),
                        TermID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DataModelID, t.TermID })
                .ForeignKey("dbo.Terms", t => t.TermID, cascadeDelete: true)
                .ForeignKey("dbo.DataModels", t => t.DataModelID, cascadeDelete: true)
                .Index(t => t.DataModelID)
                .Index(t => t.TermID);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(maxLength: 255),
                        Description = c.String(),
                        OID = c.String(maxLength: 100),
                        ReferenceUrl = c.String(maxLength: 450),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name)
                .Index(t => t.OID)
                .Index(t => t.ReferenceUrl);
            
            CreateTable(
                "dbo.ProjectTemplates",
                c => new
                    {
                        ProjectID = c.Guid(nullable: false),
                        TemplateID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.TemplateID })
                .ForeignKey("dbo.Templates", t => t.TemplateID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.TemplateID);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(maxLength: 255),
                        Description = c.String(),
                        CreatedByID = c.Guid(nullable: false),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Data = c.String(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.CreatedByID, cascadeDelete: true)
                .Index(t => t.Name)
                .Index(t => t.CreatedByID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Templates", "CreatedByID", "dbo.Users");
            DropForeignKey("dbo.ProjectTemplates", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectTemplates", "TemplateID", "dbo.Templates");
            DropForeignKey("dbo.DataModelSupportedTerms", "DataModelID", "dbo.DataModels");
            DropForeignKey("dbo.DataModelSupportedTerms", "TermID", "dbo.Terms");
            DropIndex("dbo.Templates", new[] { "CreatedByID" });
            DropIndex("dbo.Templates", new[] { "Name" });
            DropIndex("dbo.ProjectTemplates", new[] { "TemplateID" });
            DropIndex("dbo.ProjectTemplates", new[] { "ProjectID" });
            DropIndex("dbo.Terms", new[] { "ReferenceUrl" });
            DropIndex("dbo.Terms", new[] { "OID" });
            DropIndex("dbo.Terms", new[] { "Name" });
            DropIndex("dbo.DataModelSupportedTerms", new[] { "TermID" });
            DropIndex("dbo.DataModelSupportedTerms", new[] { "DataModelID" });
            DropTable("dbo.Templates");
            DropTable("dbo.ProjectTemplates");
            DropTable("dbo.Terms");
            DropTable("dbo.DataModelSupportedTerms");
        }
    }
}
