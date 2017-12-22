namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupToProjects2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Projects", "GroupID", "Groups");
            AlterColumn("Projects", "GroupID", c => c.Int());
            AddForeignKey("Projects", "GroupID", "Groups", "GroupID");
            CreateIndex("Projects", "GroupID");
        }
        
        public override void Down()
        {
            
        }
    }
}
