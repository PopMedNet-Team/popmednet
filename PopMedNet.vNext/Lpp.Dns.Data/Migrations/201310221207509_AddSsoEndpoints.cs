namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSsoEndpoints : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SsoEndpoints",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false),
                        PostUrl = c.String(nullable: false),
                        oAuthKey = c.String(maxLength: 150),
                        oAuthHash = c.String(maxLength:150),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name)
                .Index(t => t.oAuthKey)
                ;
        }
        
        public override void Down()
        {
            DropIndex("dbo.SsoEndpoints", new string[] { "Name" });
            DropIndex("dbo.SsoEndpoints", new string[] { "oAuthKey" });
            DropTable("dbo.SsoEndpoints");
        }
    }
}
