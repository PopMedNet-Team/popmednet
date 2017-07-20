namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecurityGroupUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SecurityGroupUsers",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.UserID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SecurityGroupUsers");
        }
    }
}
