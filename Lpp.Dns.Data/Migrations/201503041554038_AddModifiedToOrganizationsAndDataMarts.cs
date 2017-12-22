namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModifiedToOrganizationsAndDataMarts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "ModifiedOn", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            AddColumn("dbo.Organizations", "ModifiedOn", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));

            Sql("CREATE TRIGGER [dbo].[Organizations_UpdateItem] ON [dbo].[Organizations] AFTER UPDATE AS BEGIN IF NOT UPDATE(ModifiedOn) Update Organizations Set ModifiedOn = GETUTCDATE() Where Organizations.ID IN (Select ID FROM inserted) END ");
            Sql("CREATE TRIGGER [dbo].[DataMarts_UpdateItem] ON [dbo].[DataMarts] AFTER UPDATE AS BEGIN IF NOT UPDATE(ModifiedOn) Update DataMarts Set ModifiedOn = GETUTCDATE() Where DataMarts.ID IN (Select ID FROM inserted) END");
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[DataMarts_UpdateItem]");
            Sql("DROP TRIGGER [dbo].[Organizations_UpdateItem]");

            DropColumn("dbo.Organizations", "ModifiedOn");
            DropColumn("dbo.DataMarts", "ModifiedOn");
        }
    }
}
