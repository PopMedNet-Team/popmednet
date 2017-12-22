namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddACLAndSecurtiyGroupsTables : DbMigration
    {
        public override void Up()
        {
            //Table already exists
            //CreateTable(
            //    "dbo.SecurityGroups",
            //    c => new
            //        {
            //            SID = c.Guid(nullable: false, identity: true),
            //            DisplayName = c.String(nullable: false),
            //            OrganizationID = c.Int(nullable: false),
            //            Kind = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.SID);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.SecurityGroups");
        }
    }
}
