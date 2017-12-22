namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLogTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RegistryChangeLogs", newName: "LogsRegistryChange");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.LogsRegistryChange", newName: "RegistryChangeLogs");
        }
    }
}
