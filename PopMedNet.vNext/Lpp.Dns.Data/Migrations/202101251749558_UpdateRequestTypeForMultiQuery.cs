namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestTypeForMultiQuery : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestTypes", "TemplateID", "dbo.Templates");
            DropIndex("dbo.RequestTypes", new[] { "TemplateID" });
            AddColumn("dbo.RequestTypes", "Notes", c => c.String());
            AddColumn("dbo.RequestTypes", "SupportMultiQuery", c => c.Boolean(nullable: false));
            AddColumn("dbo.Templates", "Order", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Templates", "RequestTypeID", c => c.Guid(nullable: true));
            CreateIndex("dbo.Templates", "RequestTypeID");
            AddForeignKey("dbo.Templates", "RequestTypeID", "dbo.RequestTypes", "ID", cascadeDelete: true);

            //Update the existing templates to the first requesttype, each template should only have a single request type at the moment due to business rules
            Sql("UPDATE Templates SET RequestTypeID = (SELECT TOP 1 ID FROM RequestTypes WHERE RequestTypes.TemplateID = Templates.ID) WHERE [Type] = 1");
            //Remove orphan templates
            Sql("DELETE FROM Templates WHERE RequestTypeID IS NULL AND [Type] = 1");

            DropColumn("dbo.RequestTypes", "TemplateID");

            //Update the notes from the original template to the request type notes
            Sql("UPDATE RequestTypes SET Notes = (SELECT TOP 1 Notes FROM Templates WHERE Templates.RequestTypeID = RequestTypes.ID)");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestTypes", "TemplateID", c => c.Guid(nullable: true));
            DropForeignKey("dbo.Templates", "RequestTypeID", "dbo.RequestTypes");
            DropIndex("dbo.Templates", new[] { "RequestTypeID" });

            //Set the template ID to the first template belonging to the request type
            Sql("UPDATE RequestTypes SET TemplateID = (SELECT TOP 1 t.ID FROM Templates t WHERE t.RequestTypeID = RequestTypes.ID ORDER BY t.[Order] DESC)");
            //Remove any of the other templates belonging to the request type that are not the first template
            Sql("DELETE FROM Templates WHERE Templates.[Order] <> 0");

            DropColumn("dbo.Templates", "RequestTypeID");
            DropColumn("dbo.Templates", "Order");
            DropColumn("dbo.RequestTypes", "SupportMultiQuery");
            DropColumn("dbo.RequestTypes", "Notes");
            CreateIndex("dbo.RequestTypes", "TemplateID");
        }
    }
}
