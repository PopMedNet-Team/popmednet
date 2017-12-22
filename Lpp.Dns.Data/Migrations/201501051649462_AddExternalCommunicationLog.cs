namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExternalCommunicationLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogsExternalCommunication",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        TimeStamp = c.DateTimeOffset(nullable: false, precision: 7),
                        Reason = c.Int(nullable: false),
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.UserID, t.TimeStamp })
                .Index(t => t.EventID);

            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('CA4ABB08-A023-448E-A0F2-03D79D0B8E5C', 'External Communication Failed', 'Creates an entry in the log when external communication fails with Aqueduct.')");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LogsExternalCommunication", new[] { "EventID" });
            DropTable("dbo.LogsExternalCommunication");

            Sql("Delete from Events Where ID = 'CA4ABB08-A023-448E-A0F2-03D79D0B8E5C'");
        }
    }
}
