namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToRegistryChangeLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogsRegistryChange", "Reason", c => c.Int(nullable: false));
            AddColumn("dbo.LogsRegistryChange", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogsRegistryChange", "Description");
            DropColumn("dbo.LogsRegistryChange", "Reason");
        }
    }
}
