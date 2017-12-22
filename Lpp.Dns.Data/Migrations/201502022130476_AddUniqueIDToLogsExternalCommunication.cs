namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueIDToLogsExternalCommunication : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.LogsExternalCommunication");
            AddColumn("dbo.LogsExternalCommunication", "ID", c => c.Guid(nullable: false));
            Sql("UPDATE dbo.LogsExternalCommunication SET ID = dbo.NewSqlGuid()");
            AddPrimaryKey("dbo.LogsExternalCommunication", new[] { "UserID", "TimeStamp", "ID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.LogsExternalCommunication");
            DropColumn("dbo.LogsExternalCommunication", "ID");
            AddPrimaryKey("dbo.LogsExternalCommunication", new[] { "UserID", "TimeStamp" });
        }
    }
}
