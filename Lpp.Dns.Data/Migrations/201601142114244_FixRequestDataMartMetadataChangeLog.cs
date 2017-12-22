namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRequestDataMartMetadataChangeLog : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("LogsRequestDataMartMetadataChange");
            AddColumn("dbo.LogsRequestDataMartMetadataChange", "RequestDataMartID", c => c.Guid(nullable: false));
            CreateIndex("dbo.LogsRequestDataMartMetadataChange", "RequestDataMartID");
            AddForeignKey("dbo.LogsRequestDataMartMetadataChange", "RequestDataMartID", "dbo.RequestDataMarts", "ID");

            Sql(@"ALTER TABLE dbo.LogsRequestDataMartMetadataChange ADD CONSTRAINT PK_LogsRequestDataMartMetadataChange PRIMARY KEY (UserID, TimeStamp, RequestID, RequestDataMartID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsRequestDataMartMetadataChange", "RequestDataMartID", "dbo.RequestDataMarts");
            DropIndex("dbo.LogsRequestDataMartMetadataChange", new[] { "RequestDataMartID" });
            DropPrimaryKey("LogsRequestDataMartMetadataChange");
            DropColumn("dbo.LogsRequestDataMartMetadataChange", "RequestDataMartID");
            AddPrimaryKey("dbo.LogsRequestDataMartMetadataChange", new[] { "UserID", "TimeStamp", "RequestID" });
        }
    }
}
