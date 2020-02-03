namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                {
                    ID = c.Guid(nullable: false, identity: true),
                    MessageText = c.String(nullable: false),
                    Created = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                })
                .Index(t => t.Created)
                .PrimaryKey(t => t.ID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
