namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRejectionFieldsToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RejectedOn", c => c.DateTime());
            AddColumn("dbo.Users", "RejectedByID", c => c.Guid());
            CreateIndex("dbo.Users", "RejectedByID");
            AddForeignKey("dbo.Users", "RejectedByID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RejectedByID", "dbo.Users");
            DropIndex("dbo.Users", new[] { "RejectedByID" });
            DropColumn("dbo.Users", "RejectedByID");
            DropColumn("dbo.Users", "RejectedOn");
        }
    }
}
