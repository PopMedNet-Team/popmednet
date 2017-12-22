namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResponseNeedsApprovalLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsUploadedResultNeedsApproval",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        RequestDataMartID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.RequestDataMarts", t => t.RequestDataMartID, cascadeDelete: true)
                .Index(t => t.RequestDataMartID);
            Sql(@"ALTER TABLE dbo.LogsUploadedResultNeedsApproval ADD CONSTRAINT PK_LogsUploadedResultNeedsApproval PRIMARY KEY (UserID, TimeStamp, RequestDataMartID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogsUploadedResultNeedsApproval", "RequestDataMartID", "dbo.RequestDataMarts");
            DropIndex("dbo.LogsUploadedResultNeedsApproval", new[] { "RequestDataMartID" });
            DropTable("dbo.LogsUploadedResultNeedsApproval");
        }
    }
}
