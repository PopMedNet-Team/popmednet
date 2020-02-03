namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Description", c => c.String());
            AddColumn("dbo.Documents", "ParentDocumentID", c => c.Guid());
            AddColumn("dbo.Documents", "UploadedByID", c => c.Guid());
            AddColumn("dbo.Documents", "RevisionDescription", c => c.String());
            AddColumn("dbo.Documents", "MajorVersion", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.Documents", "MinorVersion", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Documents", "BuildVersion", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Documents", "RevisionVersion", c => c.Int(nullable: false, defaultValue: 0));
            CreateIndex("dbo.Documents", "ParentDocumentID");
            CreateIndex("dbo.Documents", "UploadedByID");
            AddForeignKey("dbo.Documents", "ParentDocumentID", "dbo.Documents", "ID");
            AddForeignKey("dbo.Documents", "UploadedByID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "UploadedByID", "dbo.Users");
            DropForeignKey("dbo.Documents", "ParentDocumentID", "dbo.Documents");
            DropIndex("dbo.Documents", new[] { "UploadedByID" });
            DropIndex("dbo.Documents", new[] { "ParentDocumentID" });
            DropColumn("dbo.Documents", "RevisionVersion");
            DropColumn("dbo.Documents", "BuildVersion");
            DropColumn("dbo.Documents", "MinorVersion");
            DropColumn("dbo.Documents", "MajorVersion");
            DropColumn("dbo.Documents", "RevisionDescription");
            DropColumn("dbo.Documents", "UploadedByID");
            DropColumn("dbo.Documents", "ParentDocumentID");
            DropColumn("dbo.Documents", "Description");
        }
    }
}
