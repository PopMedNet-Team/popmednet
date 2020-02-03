namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddACLTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ACL",
                c => new
                    {
                        ObjectID = c.Int(nullable: false),
                        GroupUserID = c.Guid(nullable: false),
                        AccessControlType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ObjectID, t.GroupUserID, t.AccessControlType });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ACL");
        }
    }
}
