namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataMartChangeLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataMartChangeLogs",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        DataMartID = c.Guid(nullable: false),
                        Reason = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .ForeignKey("dbo.DataMarts", t => t.DataMartID, cascadeDelete: true)
                .Index(t => t.DataMartID);

            Sql(@"ALTER TABLE dbo.DataMartChangeLogs ADD CONSTRAINT PK_DataMartLogs PRIMARY KEY (UserID, TimeStamp, DataMartID) ON AuditLogs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataMartChangeLogs", "DataMartID", "dbo.DataMarts");
            DropIndex("dbo.DataMartChangeLogs", new[] { "DataMartID" });
            DropPrimaryKey("dbo.UserChangeLogs");
            DropTable("dbo.DataMartChangeLogs");
            AddPrimaryKey("dbo.UserChangeLogs", new[] { "UserID", "TimeStamp" });
        }
    }
}
