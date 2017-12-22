namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsForWBDSync : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Networks",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Url = c.String(nullable: false, maxLength: 450),
                    })
                .Index(t => t.Url)
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Organizations", "Primary", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Organizations", "X509PublicKey", c => c.String(maxLength: 255));
            AddColumn("dbo.Organizations", "X509PrivateKey", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "X509PrivateKey");
            DropColumn("dbo.Organizations", "X509PublicKey");
            DropColumn("dbo.Organizations", "Primary");
            DropTable("dbo.Networks");
        }
    }
}
