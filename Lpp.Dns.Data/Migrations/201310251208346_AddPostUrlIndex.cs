namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostUrlIndex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SsoEndpoints", "PostUrl", c => c.String(false, 255));
            CreateIndex("SsoEndpoints", "PostUrl");
        }
        
        public override void Down()
        {
            DropIndex("SsoEndpoints", new string[] { "PostURl" });
        }
    }
}
