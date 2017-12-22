namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDepreciatedColumnsFromRequest : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Requests", "ActivityDueDate", "DueDate");
            DropColumn("dbo.Requests", "isAdminQuery");
            DropColumn("dbo.Requests", "ActivityPriority");
            DropColumn("dbo.Requests", "ActivityOfQuery");
            DropColumn("dbo.Requests", "RequestorEmail");
            CreateIndex("dbo.Requests", "DueDate");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Requests", new string[] {"DueDate"});
            AddColumn("dbo.Requests", "RequestorEmail", c => c.String(maxLength: 255));
            AddColumn("dbo.Requests", "ActivityOfQuery", c => c.String(maxLength: 255));
            AddColumn("dbo.Requests", "ActivityPriority", c => c.String(maxLength: 50));
            RenameColumn("dbo.Requests", "DueDate", "ActivityDueDate");
            AddColumn("dbo.Requests", "isAdminQuery", c => c.Boolean(nullable: false));
        }
    }
}
